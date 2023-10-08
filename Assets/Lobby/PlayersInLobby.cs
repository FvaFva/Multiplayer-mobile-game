using System;
using System.Collections.Generic;

public class PlayersInLobby
{
    private readonly List<Player> _guests = new List<Player>();
    private Player _local;

    public IEnumerable<Player> Guests => _guests;
    public Player Local => _local;
    public event Action IncomeLocal;
    public event Action<Player> IncomeGuest;

    public void SetLocal(Player local)
    {
        _local ??= local;
    }

    public void AddPlayer(Player player)
    {
        if(_guests.Contains(player) || player == _local)
            return; 
        
        _guests.Add(player);
        IncomeGuest?.Invoke(player);
    }
}