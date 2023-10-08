using UnityEngine;
using Mirror;
using Zenject;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody), typeof(NetworkMatch))]
public class Player : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _playSceneName;
    [SerializeField] private PlayerSkin _skin;

    [Inject] private UserInputRouter _inputRouter;
    [Inject] private DiContainer _container;
    [Inject] private PlayersInLobby _playersInLobby;
    [Inject] private MatchConnector _matchConnector;

    [SyncVar] private string _matchID;

    private Rigidbody _rigidBody;
    private Coroutine _exist; 
    private PlayerStateMachine _machine;
    private NetworkMatch _match;

    public string MatchID => _matchID;

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody tempRigidBody) && TryGetComponent(out NetworkMatch tempMatch))
        {
            _rigidBody = tempRigidBody;
            _match = tempMatch;
        }
        else
        {
            gameObject.SetActive(false);
        }

        this.UpdateInjections();

        if (isLocalPlayer == false)
            _playersInLobby.AddPlayer(this);
    }

    public void HostGame()
    {
        CmdHostGame();
    }

    [Command]
    private void CmdHostGame()
    {
        _matchID = _matchConnector.Host(this);
        Debug.Log("Lobby have just created");
        _match.matchId = _matchID.ToGuid();
        TargetHostGame(_matchID);
    }

    [TargetRpc]
    private void TargetHostGame(string id)
    {
        _matchID = id;
        Debug.Log($"TargetHostGame {id}");
    }

    public void JoinGame(string matchID)
    {
        CmdJoinGame(matchID);
    }

    [Command]
    private void CmdJoinGame(string id)
    {
        _matchID = id;
        if (_matchConnector.TryJoin(id, this))
        {
            Debug.Log("Lobby have just create");
            _match.matchId = id.ToGuid();
            TargetJoinGame(true, id);
        }
        else
        {
            Debug.Log("Lobby creating was failed");
            TargetJoinGame(true, id);
        }
    }

    [TargetRpc]
    private void TargetJoinGame(bool success, string id)
    {
        _matchID = id;
        Debug.Log($"TargetJoinGame {id}");
    }

    public void BeginGame()
    {
        CmdBeginGame();
    }

    public void StartGame()
    {
        TargetBeginGame();
    }

    [Command]
    private void CmdBeginGame()
    {
        _matchConnector.BeginGame(_matchID);
        Debug.Log("Game have started");
    }

    [TargetRpc]
    private void TargetBeginGame()
    {
        Debug.Log($"TargetJoinGame {_matchID}");
        StartPlaySystem();
        SceneManager.LoadScene(_playSceneName);
    }

    public override void OnStartAuthority()
    {
        LoadLocalData();
    }

    public override void OnStopAuthority()
    {
        StopPlaySystem();
    }

    private void StopPlaySystem()
    {
        _machine.Stop();
        _skin.Deactivate();
        _rigidBody.isKinematic = true;
        StopCoroutine(_exist);
    }

    private void LoadLocalData()
    {
        PlayerMovement movement = new PlayerMovement(_rigidBody, _speed, _animator);
        PlayerRelax relax = new PlayerRelax(_animator, 3f, 2);
        _machine = new PlayerStateMachine(relax, movement);
        _container.Inject(movement);
        _container.Inject(relax);
        _container.Inject(_machine);
        _playersInLobby.SetLocal(this);
    }

    private void StartPlaySystem()
    {
        _skin.Activate();
        _rigidBody.isKinematic = true;
        _inputRouter.StartSearchNewDirection();
        _machine.Start();
        _exist = StartCoroutine(_machine.Exist());
    }
}
