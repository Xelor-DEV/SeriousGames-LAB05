using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerListing : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;


    [SerializeField] private Player player;

    public Player Player
    {
        get
        {
            return player;
        }
    }

    public void SetPlayerInfo(Player info)
    {
        player = info;
        playerName.text = info.NickName;
    }
}