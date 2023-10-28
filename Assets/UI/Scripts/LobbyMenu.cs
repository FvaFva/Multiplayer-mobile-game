using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _joinButton;
    [SerializeField] private Canvas _lobbyCanvas;
    [SerializeField] private UserViewHub _userViewHub;
    [SerializeField] private UserView _userViewPrefab;
    [SerializeField] private TMP_Text _idText;
    [SerializeField] private Button _beginButton;

    [Inject] private LocalPlayerRouter _local;

    public bool InGame { get; private set; }

    private void OnEnable()
    {
        _local.IncomeInMatch += ShowMatch;
    }

    private void OnDisable()
    {
        _local.IncomeInMatch -= ShowMatch;
    }

    public void Host()
    {
        SetInteractableMainElements(false);
        _local.HostMatch();
    }
    
    public void Join()
    {
        SetInteractableMainElements(false);
    }

    public void StartGamer()
    {
    }

    public void ShowMatch(MatchInfo match)
    {
        _lobbyCanvas.enabled = true;
        _idText.text = match.ID;

        foreach(Player player in match.Players)
            SpawnPlayerUIPrefab(player);
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
