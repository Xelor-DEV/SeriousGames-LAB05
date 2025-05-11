using UnityEngine;
using Photon.Pun;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float range = 100f;
    [SerializeField] protected LayerMask blockingLayers;
    [SerializeField] protected Transform muzzlePoint;

    public abstract void VisualShot(Vector3 hitPoint);

    public Vector3 RealShot()
    {
        Camera aimCamera = ArenaController.Instance.SceneCamera;
        Vector3 rayOrigin = aimCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Vector3 hitPoint = rayOrigin + aimCamera.transform.forward * range;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, aimCamera.transform.forward, out hit, range, blockingLayers))
        {
            HealthManager health = hit.collider.GetComponent<HealthManager>();
            if (health != null)
            {
                // Enviar RPC al PhotonView del jugador impactado
                health.PhotonView.RPC("TakeDamage", RpcTarget.All, damage);
            }
            hitPoint = hit.point;
        }

        VisualShot(hitPoint);
        return hitPoint;
    }
}