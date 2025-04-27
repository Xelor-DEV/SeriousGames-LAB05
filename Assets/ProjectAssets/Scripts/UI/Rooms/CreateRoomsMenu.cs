using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomName;

    [Header("Max Players")]
    [SerializeField] private int maxPlayers;
    [SerializeField] private int minPlayers;
    [SerializeField] private int currentMaxPlayers;
    [SerializeField] private TMP_Text currentMaxPlayersText;

    private void Start()
    {
        UpdateCurrentMaxPlayers(0);
    }

    public void UpdateCurrentMaxPlayers(int amount)
    {
        currentMaxPlayers = Mathf.Clamp(
            currentMaxPlayers + amount,
            minPlayers,
            maxPlayers
        );

        currentMaxPlayersText.text = currentMaxPlayers.ToString();
    }

    // OnClick
    public void CreateRoom()
    {
        //JoinOrCreateRoom
        if (PhotonNetwork.IsConnected == true)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = currentMaxPlayers;
            PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);
            UIManager.Instance.RoomName.text = roomName.text;
            CleanInputs();
        }
    }

    public void CleanInputs()
    {
        roomName.text = string.Empty;
        currentMaxPlayers = minPlayers;
        currentMaxPlayersText.text = currentMaxPlayers.ToString();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room Successfully");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Creation Failed: " + message);
    }
}
