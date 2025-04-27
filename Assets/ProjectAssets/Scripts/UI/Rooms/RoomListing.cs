using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private TMP_Text roomPlayers;

    [SerializeField] private RoomInfo roomInfo;

    public RoomInfo RoomInfo
    {
        get
        {
            return roomInfo;
        }
    }

    public void SetRoomInfo(RoomInfo info)
    {
        roomInfo = info;
        roomName.text = info.Name;
        roomPlayers.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected == true)
        {
            PhotonNetwork.JoinRoom(RoomInfo.Name);
            UIManager.Instance.RoomName.text = RoomInfo.Name;
            UIManager.Instance.ActivateMenu(3);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomPlayers.text = roomInfo.PlayerCount+1 + "/" + roomInfo.MaxPlayers;
    }
}