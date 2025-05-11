using UnityEngine;
using System.Collections;

public class AssaultRifle : Weapon
{
    [Header("Visual Effects")]
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float effectDuration = 0.5f;

    public override void VisualShot(Vector3 hitPoint)
    {
        StartCoroutine(ShowShotEffects(hitPoint));
    }

    private IEnumerator ShowShotEffects(Vector3 hitPoint)
    {
        muzzleFlash.SetActive(true);
        lineRenderer.SetPosition(0, muzzlePoint.position);
        lineRenderer.SetPosition(1, hitPoint);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(effectDuration);

        muzzleFlash.SetActive(false);
        lineRenderer.enabled = false;
    }
}