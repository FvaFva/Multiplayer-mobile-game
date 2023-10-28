using UnityEngine;
using Mirror;
using Zenject;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkMatch))]
public class Player : NetworkBehaviour
{
    private const string PlaySceneName = "PlayArea";

    [SerializeField] private Unit _unit;

    [Inject] private LocalPlayerRouter _local;
    [Inject] private MatchConnector _matchConnector;
    [Inject] private DiContainer _container;

    [SyncVar(hook = nameof(HandleIdUpdate))] private string _matchID;

    private NetworkMatch _networkMatch;

    private void Awake()
    {
        if (TryGetComponent(out NetworkMatch tempMatch))
        {
            _networkMatch = tempMatch;
        }
        else
        {
            gameObject.SetActive(false);
        }
        this.UpdateInjections();
    }

    public void HostMatch()
    {
        CmdHostMatch();
    }

    public void JoinGame(string matchID)
    {
        ServerJoinGame(matchID);
    }

    public void BeginGame()
    {
        CmdBeginGame();
    }

    public void StartGame()
    {
        TargetBeginGame();
    }

    #region Commands
    [Command]
    private void CmdHostMatch()
    {
        Match match = _matchConnector.CmdHost(this);
        _matchID = match.ID;
        _networkMatch.matchId = _matchID.ToGuid();
        MatchInfo info = new MatchInfo();
        info.ID = _matchID;
        info.LoadPlayers(match.Players);
        Debug.Log(info.Players.Count);
    }

    [Command]
    private void ServerJoinGame(string id)
    {
        _matchID = id;
        if (_matchConnector.CmdTryJoin(id, this))
        {
            Debug.Log($"You have just connected to {_matchID}");
            _networkMatch.matchId = id.ToGuid();
        }
        else
        {
            Debug.Log("Lobby connecting was failed");
        }
    }

    [Command]
    private void CmdBeginGame()
    {
        _matchConnector.CmdBeginGame(_matchID);
        Debug.Log("Game have started");
    }
    #endregion

    #region Client
    private void HandleIdUpdate(string oldId, string newId)
    {
        _matchID = newId;
        Debug.Log($"Player client take new match id {newId}");
    }

    [TargetRpc]
    private void ShowMatchInfo(MatchInfo match)
    {
        _local.ShowMatchInfo(match);
    }

    [TargetRpc]
    private void TargetBeginGame()
    {
        Debug.Log($"TargetJoinGame {_matchID}");
        var unit = _container.InstantiatePrefab(_unit.gameObject);
        SceneManager.LoadScene(PlaySceneName);
        NetworkServer.Spawn(unit);
    }

    public override void OnStartAuthority()
    {
        _local.SetLocal(this);
    }
    #endregion
}
