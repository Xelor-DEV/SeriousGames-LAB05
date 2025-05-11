using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float smoothTime = 0.1f;

    private float currentSpeed;
    private float targetSpeed;
    private float smoothVelocity;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        // Manejar velocidad de movimiento
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref smoothVelocity, smoothTime);
        animator.SetFloat("Speed", currentSpeed);

        animator.SetBool("IsMoving", playerController.IsMoving);
        animator.SetBool("IsJumping", playerController.IsJumping);
        animator.SetBool("IsGrounded", playerController.IsGrounded);
        animator.SetBool("IsFalling", playerController.IsFalling);
    }

    public void UpdateTargetSpeed(float newSpeed)
    {
        targetSpeed = newSpeed;
    }

    public void SetAimLayerWeight(float weight)
    {
        animator.SetLayerWeight(1, weight); // Ajustar el índice según tu configuración de layers
    }
}