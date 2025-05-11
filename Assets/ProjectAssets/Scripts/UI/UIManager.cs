using UnityEngine;
using TMPro;
using Photon.Pun;

public class UIManager : MonoBehaviourPunCallbacks
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject[] menus;
    [SerializeField] private int initIndex;

    [Header("MainLobby")]
    [SerializeField] private TMP_Text nickName;

    [Header("CurrentRoomMenu")]
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private GameObject startArena;

    public TMP_Text NickName
    {
        get
        {
            return nickName;
        }
        set
        {
            nickName = value;
        }
    }

    public TMP_Text RoomName
    {
        get
        {
            return roomName;
        }
        set
        {
            roomName = value;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ActivateMenu(initIndex);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient == true)
        {
            startArena.SetActive(true);
        }
        else
        {
            startArena.SetActive(false);
        }
    }

    public void ActivateMenu(int menuIndex)
    {
        for(int i = 0; i < menus.Length; ++i)
        {
            if (menus[i] == menus[menuIndex])
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
}