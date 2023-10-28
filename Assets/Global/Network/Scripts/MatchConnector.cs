using Mirror;
using System;

public class MatchConnector 
{
    [SyncVar] private readonly MatchesPool _matches = new MatchesPool();
    private readonly Random _random = new Random();

    private TurnManager _turnManager;

    public void BindTurnManager(TurnManager prefab)
    {
        _turnManager ??= prefab;
    }

    [Command]
    public Match CmdHost(Player host)
    {
        string id = GetRandomID();
        Match match = _matches.CreateMatch(id, host);
        return match;
    }

    [Command]
    public bool CmdTryJoin(string id, Player guest)
    {
        Match foundMatch = _matches.GetMatch(id);
        bool found;

        if (foundMatch != null)
        {
            foundMatch.AddPlayer(guest);
            found = true;
        }
        else
        {
            found = false;
        }

        return found;
    }

    [Command]
    public void CmdBeginGame(string id)
    {
        Match foundMatch = _matches.GetMatch(id);

        if (foundMatch != null)
        {
            NetworkServer.Spawn(_turnManager.gameObject);
            _turnManager.RegisterMatch(foundMatch);

            foreach (Player player in foundMatch.Players)
                player.StartGame();
        }
    }

    private string GetRandomID()
    {
        string id = string.Empty;
        int countIterations = 5;
        int divider = 26;
        int maxRange = 36;
        int multiCast = 65;

        for (int i = 0; i < countIterations; i++)
        {
            int rand = _random.Next(0, maxRange);

            if (rand < divider)
                id += (char)(rand + multiCast);
            else
                id += (rand - divider).ToString();
        }

        return id;
    }
}