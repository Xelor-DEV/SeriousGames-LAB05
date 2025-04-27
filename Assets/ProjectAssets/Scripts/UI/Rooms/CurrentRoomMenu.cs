using UnityEngine;
using Photon.Pun;

public class CurrentRoomMenu : MonoBehaviour
{
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        UIManager.Instance.ActivateMenu(1);
    }
}