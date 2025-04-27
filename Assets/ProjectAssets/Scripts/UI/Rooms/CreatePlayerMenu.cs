using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreatePlayerMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text playerName;
    [SerializeField] private int mainLobbyIndex;
    public void SetNickName()
    {
        print("Connecting to server.");
        MasterManager.GameSettings.NickName = playerName.text;
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Conected to server.");
        print(PhotonNetwork.LocalPlayer.NickName);

        UIManager.Instance.ActivateMenu(mainLobbyIndex);
        UIManager.Instance.NickName.text = PhotonNetwork.NickName;

        if(PhotonNetwork.InLobby == false)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server for reason " + cause.ToString());
    }
}