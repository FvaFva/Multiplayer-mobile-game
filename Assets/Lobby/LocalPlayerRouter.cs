using System;

public class LocalPlayerRouter
{
    private Player _local;
    public event Action IncomeLocal;
    public event Action<MatchInfo> IncomeInMatch;

    public void SetLocal(Player local)
    {
        if(local == null)
            return;

        _local = local;
        IncomeLocal?.Invoke();
    }

    public void HostMatch()
    {
        _local?.HostMatch();
    }

    public void ShowMatchInfo(MatchInfo match)
    {
        IncomeInMatch?.Invoke(match);
    }
}