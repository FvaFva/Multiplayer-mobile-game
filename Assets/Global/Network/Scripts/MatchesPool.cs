using ModestTree;
using System.Collections.Generic;
using System.Linq;

public class MatchesPool
{
    private readonly List<Match> _matches = new List<Match>();

    public Match GetMatch(string id)
    {
        if(id.IsEmpty())
            return null;

        return _matches.Where(match => match.ID.Equals(id)).FirstOrDefault();
    }

    public Match CreateMatch(string id)
    {
        Match newMatch = new Match(id);
        _matches.Add(newMatch);
        return newMatch;
    }
}