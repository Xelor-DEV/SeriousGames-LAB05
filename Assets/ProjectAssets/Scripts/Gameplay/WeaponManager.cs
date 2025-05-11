using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons Settings")]
    [SerializeField] private int maxWeapons = 2;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private int currentWeaponIndex;

    [Header("References")]
    [SerializeField] private PhotonView photonView;
    [SerializeField] private PlayerController playerController;

    private void OnValidate()
    {
        if (weapons.Length > maxWeapons)
        {
            System.Array.Resize(ref weapons, maxWeapons);
        }
    }

    private void Start()
    {
        currentWeaponIndex = 0;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(photonView.IsMine == false || playerController.IsAiming == false || PhotonNetwork.IsConnected == false)
        {
            return;
        }

        if (context.performed == true)
        {
            if (currentWeaponIndex < weapons.Length && weapons[currentWeaponIndex] != null)
            {
                Vector3 hitPoint = weapons[currentWeaponIndex].RealShot();
                photonView.RPC("RPC_VisualShot", RpcTarget.Others, hitPoint);
            }
        }
    }

    [PunRPC]
    private void RPC_VisualShot(Vector3 hitPoint)
    {
        weapons[currentWeaponIndex]?.VisualShot(hitPoint);
    }
}