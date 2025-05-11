using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AnimatorManager animatorManager;
    [SerializeField] private Transform cameraPivot;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float runningSpeed = 10f;
    [SerializeField] private float aimMovementSpeed = 3f;
    [SerializeField] private float aimRunningSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Aim Settings")]
    [SerializeField] private float verticalCameraSensitivity = 2f;
    [SerializeField] private float horizontalCameraSensitivity = 100f;
    [SerializeField] private float maxVerticalAngle = 80f;
    [SerializeField] private float minVerticalAngle = -30f;

    [Header("Network Settings")]
    [SerializeField] private bool isClone;
    [SerializeField] private bool isSinglePlayer;

    [Header("Animation Events")]
    public UnityEvent<float> onMovementStateChanged;

    // Estados privados
    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;
    private bool isMoving;
    private bool isAiming;

    // Variables de control
    private Vector2 inputVector;
    private Vector2 lookInput;
    private Rigidbody rb;
    private bool isRunning = false;
    private float currentVerticalRotation;

    // Propiedades públicas
    public bool IsGrounded => isGrounded;
    public bool IsJumping => isJumping;
    public bool IsFalling => isFalling;
    public bool IsMoving => isMoving;
    public bool IsAiming => isAiming;
    public Transform CameraPivot => cameraPivot;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isClone && !isSinglePlayer) return;

        // Actualizar estado de movimiento
        isMoving = inputVector != Vector2.zero;

        float currentState = CalculateMovementState();
        onMovementStateChanged?.Invoke(currentState);
        CheckGround();
    }

    private void FixedUpdate()
    {
        if (isClone && !isSinglePlayer) return;


        MoveCharacter();
        RotateCharacter();
    }

    private void LateUpdate()
    {
        if (isAiming) HandleCameraRotation();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isClone && !isSinglePlayer) return;
        inputVector = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (isClone && !isSinglePlayer) return;

        if (context.performed == true)
        {
            isRunning = true;
        }
        else if (context.canceled == true)
        {
            isRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isClone && !isSinglePlayer) return;

        if (context.performed && isGrounded)
        {
            isJumping = true;
            isGrounded = false; // Añadir esta línea
            isFalling = false;  // Resetear caída al iniciar salto

            Vector3 newVelocity = rb.linearVelocity;
            newVelocity.y = jumpForce;
            rb.linearVelocity = newVelocity;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (isClone && !isSinglePlayer) return;

        if (context.performed)
        {
            StartAiming();
        }
        else if (context.canceled)
        {
            StopAiming();
        }
    }


    public void OnLook(InputAction.CallbackContext context) // Nuevo método para input de mira
    {
        if (isClone && !isSinglePlayer) return;

        lookInput = context.ReadValue<Vector2>();
    }

    private void StartAiming()
    {
        isAiming = true;
        currentVerticalRotation = cameraPivot.localEulerAngles.x;

        ArenaController.Instance.AimCamera.Priority = 20;
        ArenaController.Instance.MovementCamera.Priority = 10;
        ArenaController.Instance.CrossHair.SetActive(true);
        animatorManager.SetAimLayerWeight(1f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void StopAiming()
    {
        isAiming = false;
        ArenaController.Instance.MovementCamera.Priority = 20;
        ArenaController.Instance.AimCamera.Priority = 10;
        ArenaController.Instance.CrossHair.SetActive(false);
        animatorManager.SetAimLayerWeight(0f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private float CalculateMovementState()
    {
        if (inputVector != Vector2.zero)
        {
            return isRunning ? 2f : 1f;
        }
        return 0f;
    }
    private void HandleCameraRotation()
    {
        // Rotación vertical en el pivote
        currentVerticalRotation -= lookInput.y * verticalCameraSensitivity;
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, minVerticalAngle, maxVerticalAngle);
        cameraPivot.localRotation = Quaternion.Euler(currentVerticalRotation, 0f, 0f);

        // Rotación horizontal del personaje
        float mouseX = lookInput.x * horizontalCameraSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }
    private void MoveCharacter()
    {
        Transform cameraTransform = isAiming ?
            ArenaController.Instance.AimCamera.transform :
            ArenaController.Instance.MovementCamera.transform;

        Vector3 movementDirection = CalculateMovementDirection(cameraTransform);

        // Lógica corregida (versión optimizada)
        float currentSpeed = isAiming
            ? (isRunning ? aimRunningSpeed : aimMovementSpeed)
            : (isRunning ? runningSpeed : movementSpeed);

        rb.linearVelocity = new Vector3(
            movementDirection.x * currentSpeed,
            rb.linearVelocity.y,
            movementDirection.z * currentSpeed
        );
    }
    private Vector3 CalculateMovementDirection(Transform cameraTransform)
    {
        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        return (forward * inputVector.y + right * inputVector.x).normalized;
    }
    private void RotateCharacter()
    {
        if (isAiming) return;

        if (rb.linearVelocity.magnitude > 0.1f)
        {
            Vector3 lookDirection = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }

    public void SetCloneStatus(bool cloneStatus)
    {
        isClone = cloneStatus;
    }

    private void CheckGround()
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.Raycast(rayStart, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded)
        {
            isFalling = false;
            isJumping = false; // Siempre resetear al tocar suelo
        }
        else
        {
            isFalling = rb.linearVelocity.y < 0f;

            // Si estaba saltando y comienza a caer
            if (isJumping && isFalling)
            {
                isJumping = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.red : Color.green;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;
        Gizmos.DrawLine(rayStart, rayStart + Vector3.down * groundCheckDistance);
    }
}