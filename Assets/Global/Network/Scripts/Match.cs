using Mirror;
using System.Collections.Generic;

public class Match
{
    private readonly List<Player> _players = new List<Player>();

    public Match(string id)
    {
        ID = id;
    }

    public IEnumerable<Player> Players => _players;
    public string ID {  get; private set; }

    public void AddPlayer(Player player)
    {
        if (_players.Contains(player) == false)
            _players.Add(player);
    }
}
