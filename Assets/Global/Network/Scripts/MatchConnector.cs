using Mirror;
using System;

public class MatchConnector
{
    private readonly MatchesPool _matches = new MatchesPool();
    private readonly Random _random = new Random();

    private TurnManager _turnManager;

    public event Action Hosted;
    public event Action<bool> TriedJoin;
    public event Action Began;

    public void BindTurnManager(TurnManager prefab) 
    { 
        _turnManager ??= prefab;
    }

    public string Host(Player host)
    {
        string id = GetRandomID();
        Match match = _matches.CreateMatch(id);
        match.AddPlayer(host);
        Hosted?.Invoke();
        return id;
    }

    public bool TryJoin(string id, Player guest)
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

        TriedJoin?.Invoke(found);
        return found;
    }

    public void BeginGame(string id)
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