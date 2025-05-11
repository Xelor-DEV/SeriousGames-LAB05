using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PhotonView photonView;
    [SerializeField] private SkinnedMeshRenderer characterRenderer;

    [Header("Health UI")]
    [SerializeField] private Image cloneHealthBar;
    [SerializeField] private TMP_Text cloneHealthText;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damageEffectDuration = 0.1f;
    [SerializeField] private Material damageMaterial;

    private float currentHealth;
    [SerializeField] private Material[] originalMaterials;

    public PhotonView PhotonView
    {
        get
        {
            return photonView;
        }
    }

    private void Awake()
    {
        if(characterRenderer == null)
        {
            characterRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        originalMaterials = new Material[characterRenderer.materials.Length];
        characterRenderer.materials.CopyTo(originalMaterials, 0);

        currentHealth = maxHealth;
    }

    [PunRPC] // Añadir este atributo
    public void TakeDamage(float damage)
    {
        if (photonView.IsMine) // Solo el dueño aplica el daño real
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0);
            photonView.RPC("RPC_UpdateHealth", RpcTarget.All, currentHealth);
        }
    }

    [PunRPC]
    private void RPC_UpdateHealth(float newHealth)
    {
        currentHealth = newHealth;
        UpdateAllHealthUI();
        StartCoroutine(ShowDamageEffect());

        // Añadir lógica de respawn cuando la vida llega a 0
        if (currentHealth <= 0 && photonView.IsMine)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        // Obtener punto de spawn aleatorio
        Transform spawnPoint = ArenaController.Instance.PlayerSpawnPoint();

        // Teletransportar jugador y clones
        photonView.RPC("RPC_RespawnPlayer", RpcTarget.All,
                      spawnPoint.position,
                      spawnPoint.rotation);
    }

    [PunRPC]
    private void RPC_RespawnPlayer(Vector3 newPosition, Quaternion newRotation)
    {
        // Actualizar posición y rotación
        transform.SetPositionAndRotation(newPosition, newRotation);

        // Restaurar vida
        currentHealth = maxHealth;
        UpdateAllHealthUI();
    }


    private void UpdateAllHealthUI()
    {
        float normalizedHealth = currentHealth / maxHealth;

        if (cloneHealthBar != null)
        {
            cloneHealthBar.fillAmount = normalizedHealth;
        }
        if (cloneHealthText != null)
        {
            cloneHealthText.text = $"{Mathf.RoundToInt(normalizedHealth * 100)}%";
        }

        if (photonView.IsMine)
        {
            ArenaController.Instance.UpdateHealthUI(normalizedHealth, currentHealth);
        }
    }

    private IEnumerator ShowDamageEffect()
    {
        Material[] damageMats = new Material[originalMaterials.Length];

        for (int i = 0; i < damageMats.Length; ++i)
        {
            damageMats[i] = damageMaterial;
        }

        characterRenderer.materials = damageMats;

        yield return new WaitForSeconds(damageEffectDuration);

        characterRenderer.materials = originalMaterials;
    }
}