using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private InputController _inputController;
    private CharacterController _characterController;

    [Header("Movement")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PlayerControllerConfig controllerConfig;

    private Vector2 _moveInput;
    private Vector3 _currentVelocity;
    private bool _isGrounded;

    [Header("Look Rotation")]
    [SerializeField] private Transform lookTarget;
    private Vector2 _mouseRotation;

    private bool _canMove = true;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputController = GetComponent<InputController>();
    }

    void OnEnable()
    {
        if (_inputController != null)
        {
            _inputController.MoveEvent += MovementInput;
            _inputController.JumpEvent += JumpInput;
            _inputController.MouseLookEvent += RotationInput;
        }
    }

    void OnDisable()
    {
        if (_inputController != null)
        {
            _inputController.MoveEvent -= MovementInput;
            _inputController.JumpEvent -= JumpInput;
            _inputController.MouseLookEvent -= RotationInput;
        }
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (_canMove)
        {
            Movement();
            ApplyGravityAndMove();
            Rotate();
        }

        ClampRotation();
    }

    // ================= INPUT =================

    private void MovementInput(Vector2 movement)
    {
        _moveInput = movement;
    }

    private void RotationInput(Vector2 mouseRotation)
    {
        _mouseRotation = mouseRotation;
    }

    // ================= MOVEMENT =================

    private void Movement()
    {
        Vector3 targetDirection =
            transform.right * _moveInput.x +
            transform.forward * _moveInput.y;

        Vector3 targetVelocity = targetDirection * controllerConfig.MovementSpeed;

        float acceleration = IsGrounded()
            ? controllerConfig.groundAcceleration
            : controllerConfig.AirAcceleration;

        _currentVelocity = Vector3.MoveTowards(
            _currentVelocity,
            targetVelocity,
            acceleration * Time.deltaTime
        );

        // Deceleration
        if (targetDirection == Vector3.zero)
        {
            Vector3 horizontalVelocity = new Vector3(
                _currentVelocity.x,
                0,
                _currentVelocity.z
            );

            Vector3 decelerated = Vector3.MoveTowards(
                horizontalVelocity,
                Vector3.zero,
                controllerConfig.groundDeceleration * Time.deltaTime
            );

            _currentVelocity.x = decelerated.x;
            _currentVelocity.z = decelerated.z;
        }
    }

    // ================= JUMP & GRAVITY =================

    private void JumpInput()
    {
        if (!IsGrounded()) return;

        // Salto físicamente correcto
        _currentVelocity.y = Mathf.Sqrt(
            controllerConfig.jumpHeight *
            -2f *
            Physics.gravity.y *
            controllerConfig.gravity
        );
    }

    private void ApplyGravityAndMove()
    {
        if (IsGrounded() && _currentVelocity.y < 0f)
        {
            _currentVelocity.y = -2f; // mantiene contacto con el suelo
        }

        // Gravedad SIEMPRE
        _currentVelocity.y +=
            Physics.gravity.y * controllerConfig.gravity * Time.deltaTime;

        _characterController.Move(_currentVelocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        _isGrounded = Physics.SphereCast(
            origin,
            _characterController.radius * 0.9f,
            Vector3.down,
            out _,
            (_characterController.height / 2f) + 0.2f,
            groundLayer
        );

        return _isGrounded;
    }

    // ================= ROTATION =================

    private void Rotate()
    {
        transform.Rotate(
            Vector3.up,
            _mouseRotation.x * controllerConfig.mouseSensitivity * Time.deltaTime
        );

        lookTarget.Rotate(
            Vector3.right,
            -_mouseRotation.y * controllerConfig.mouseSensitivity * Time.deltaTime
        );
    }

    private void ClampRotation()
    {
        float currentX = lookTarget.localEulerAngles.x;

        if (currentX > 180)
            currentX -= 360;

        currentX = Mathf.Clamp(
            currentX,
            -controllerConfig.CameraBounds,
            controllerConfig.CameraBounds
        );

        lookTarget.localEulerAngles = new Vector3(currentX, 0f, 0f);
    }
}
