using System;
using System.Collections.Generic;

[Serializable]
public struct MatchInfo
{
    public string ID;
    public List<Player> Players;

    public void LoadPlayers(IEnumerable<Player> players)
    {
        foreach (Player p in players)
        {
            Players.Add(p);
        }
    }
}
