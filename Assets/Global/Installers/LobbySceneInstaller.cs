using UnityEngine;
using Zenject;

public class LobbySceneInstaller : MonoInstaller
{
    [SerializeField] private LobbyMenu _lobbyMenu;
    [SerializeField] private TurnManager _turnManager;

    [Inject] private readonly MatchConnector _matchConnector;

    public override void InstallBindings()
    {
        _matchConnector.BindTurnManager(_turnManager);

        Container.Bind<LobbyMenu>().FromInstance(_lobbyMenu).AsSingle();
        Container.QueueForInject(_lobbyMenu);
    }
}