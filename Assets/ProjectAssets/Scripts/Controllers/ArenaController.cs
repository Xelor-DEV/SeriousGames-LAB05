using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.Cinemachine;

public class ArenaController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static ArenaController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private CinemachineCamera movementCamera;
    [SerializeField] private CinemachineCamera aimCamera;
    [SerializeField] private CinemachineThirdPersonFollow thirdPersonFollow;
    [SerializeField] private Camera sceneCamera;
    [Header("User Interface")]
    [SerializeField] private TMP_Text nickname;
    [SerializeField] private GameObject crossHair;

    [Header("Health UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthText;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] playerSpawnPoints;

    private GameObject currentPlayer;
    private int[] precomputedSpawnIndices = new int[1000];
    private int currentSpawnIndex = 0;

    public CinemachineCamera MovementCamera
    {
        get
        {
            return movementCamera;
        }
    }

    public CinemachineCamera AimCamera
    {
        get
        {
            return aimCamera;
        }
    }

    public CinemachineThirdPersonFollow ThirdPersonFollow
    {
        get
        {
            return thirdPersonFollow;
        }
    }

    public Camera SceneCamera
    {
        get
        {
            return sceneCamera;
        }
    }

    public GameObject CurrentPlayer
    {
        set
        {
            currentPlayer = value;
        }
    }

    public GameObject CrossHair
    {
        get
        {
            return crossHair;
        }
    }

    #region UnityMethods
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }
    #endregion

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {

        for (int i = 0; i < precomputedSpawnIndices.Length; ++i)
        {
            precomputedSpawnIndices[i] = Random.Range(0, playerSpawnPoints.Length);
        }

        nickname.text = "NickName: " + PhotonNetwork.NickName;

        currentPlayer = FactoryBuilder.BuilderPlayer(characterDatabase.CurrentCharacter.characterPrefab.name, PhotonNetwork.NickName, PlayerSpawnPoint());

        if (currentPlayer.GetComponent<PhotonView>().IsMine == true)
        {

        }

        print("OnSceneFinishedLoading...");
    }

    public Transform PlayerSpawnPoint()
    {
        if (playerSpawnPoints == null || playerSpawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points are defined!");
            return null;
        }

        int index = precomputedSpawnIndices[currentSpawnIndex];

        currentSpawnIndex = (currentSpawnIndex + 1) % precomputedSpawnIndices.Length;

        return playerSpawnPoints[index];
    }

    public void UpdateHealthUI(float normalizedHealth, float currentHealth)
    {
        healthBar.fillAmount = normalizedHealth;
        healthText.text = $"{Mathf.RoundToInt(currentHealth)}%";
    }
}