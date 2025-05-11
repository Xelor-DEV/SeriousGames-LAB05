using UnityEngine;
using Photon.Pun;

public class CurrentRoomMenu : MonoBehaviour
{
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        UIManager.Instance.ActivateMenu(1);
    }

    public void LoadArena(string sceneName)
    {
        if(PhotonNetwork.IsMasterClient == true && PhotonNetwork.IsConnected == true)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
    }
}