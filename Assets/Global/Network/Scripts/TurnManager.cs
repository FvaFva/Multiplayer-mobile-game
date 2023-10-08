using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkMatch))]
public class TurnManager : MonoBehaviour
{
    private NetworkMatch _networkMatch;
    private Match _match;

    private void Awake()
    {
        if(TryGetComponent(out NetworkMatch networkMatch) == false)
        {
            Debug.LogWarning("have no component");
            gameObject.SetActive(false);
        }

        _networkMatch = networkMatch;
    }

    public void RegisterMatch(Match match)
    {
        _match = match;
        _networkMatch.matchId = _match.ID.ToGuid();
    }
}
