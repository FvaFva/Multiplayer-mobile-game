using Mirror;
using Zenject;

public class Network : NetworkManager
{
    [Inject] private readonly MatchConnector _connector;
}