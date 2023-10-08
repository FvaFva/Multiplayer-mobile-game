using UnityEngine;
using TMPro;

public class UserView : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerName;

    private Player _player;

    public void SetPlayer(Player player)
    {
        _player = player;
        _playerName.text = "Name";
    }
}
