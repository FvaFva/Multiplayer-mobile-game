using System.Collections.Generic;

public class Match
{
    private readonly List<Player> _players = new List<Player>();
    private Player _host;
    private string _id;
    private bool _isHosted;

    public IEnumerable<Player> Players => _players;
    public string ID => _id;
    public bool IsHosted => _isHosted;

    public void AddPlayer(Player player)
    {
        if (_players.Contains(player) == false)
            _players.Add(player);
    }

    public void Host(Player player, string id)
    {
        _host = player;
        _isHosted = true;
        _players.Add(player);
        _id = id;
    }
}
