using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private TMP_Text LabelNickname;
    [SerializeField] private GameObject canvas;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private bool isMine;

    private PlayerController playerController;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerController = GetComponent<PlayerController>();

        if (photonView.IsMine == true)
        {
            SetupLocalPlayer();
        }
        else
        {
            SetupClone();
        }

        isMine = photonView;
    }

    private void SetupLocalPlayer()
    {
        gameObject.tag = "Player";
        playerController.SetCloneStatus(false);

        ArenaController.Instance.MovementCamera.Follow = transform;
        ArenaController.Instance.MovementCamera.LookAt = transform;

        ArenaController.Instance.AimCamera.Follow = playerController.CameraPivot;
        ArenaController.Instance.AimCamera.LookAt = playerController.CameraPivot;

        canvas.SetActive(false);
        ArenaController.Instance.CurrentPlayer = gameObject;
    }

    private void SetupClone()
    {
        gameObject.tag = "PlayerClone";
        playerController.SetCloneStatus(true);

        Destroy(GetComponent<PlayerInput>());

        SetNicknameOnLabel(photonView.Controller.NickName);
        canvas.SetActive(true);
    }

    private void Update()
    {
        if (photonView.IsMine == false)
        {
            if (playerController.IsAiming == true)
            {
                canvas.transform.forward = -ArenaController.Instance.SceneCamera.transform.forward;
            }
            else
            {
                canvas.transform.forward = ArenaController.Instance.MovementCamera.transform.forward;
            }
        }
    }

    public void SetNicknameOnLabel(string nickname)
    {
        if (LabelNickname != null)
        {
            LabelNickname.text = nickname;
        }
    }
}