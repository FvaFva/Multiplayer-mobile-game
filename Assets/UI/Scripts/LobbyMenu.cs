using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbyMenu : NetworkBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _joinButton;
    [SerializeField] private Canvas _lobbyCanvas;
    [SerializeField] private UserViewHub _userViewHub;
    [SerializeField] private UserView _userViewPrefab;
    [SerializeField] private TMP_Text _idText;
    [SerializeField] private Button _beginButton;

    [Inject] private readonly PlayersInLobby _playersInLobby;
    [Inject] private readonly MatchConnector _matchConnector;

    public bool InGame { get; private set; }

    private void OnEnable()
    {
        _playersInLobby.IncomeGuest += SpawnPlayerUIPrefab;
        _matchConnector.Hosted += OnHostMatch;
        _matchConnector.TriedJoin += OnJoinToGame;
    }

    private void OnDisable()
    {
        _playersInLobby.IncomeGuest -= SpawnPlayerUIPrefab;
        _matchConnector.Hosted -= OnHostMatch;
        _matchConnector.TriedJoin -= OnJoinToGame;
    }

    public void Host()
    {
        SetInteractableMainElements(false);
        _playersInLobby.Local.HostGame();
    }
    
    public void Join()
    {
        SetInteractableMainElements(false);
        _playersInLobby.Local.JoinGame(_inputField.text.ToUpper());
    }

    public void StartGamer()
    {
        _playersInLobby.Local.BeginGame();
    }

    public void ChangeGameState(bool inGame)
    {
        InGame = inGame;
    }

    private void OnHostMatch()
    {
        _lobbyCanvas.enabled = true;
        _beginButton.interactable = true;
    }

    private void OnJoinToGame(bool success)
    {
        if (success)
        {
            _lobbyCanvas.enabled = true;
            SpawnPlayerUIPrefab(_playersInLobby.Local);
            _idText.text = _playersInLobby.Local.MatchID.ToString();
            _beginButton.interactable = false;
        }
        else
        {
            SetInteractableMainElements(true);
        }
    }

    private void SetInteractableMainElements(bool interactable)
    {
        _hostButton.interactable = interactable;
        _joinButton.interactable = interactable;
        _inputField.interactable = interactable;
    }

    private void SpawnPlayerUIPrefab(Player player)
    {
        GameObject newUserView = Instantiate(_userViewPrefab.gameObject, _userViewHub.transform);
        newUserView.TryGetComponent<UserView>(out UserView userView);
        userView.SetPlayer(player);
    }
}
