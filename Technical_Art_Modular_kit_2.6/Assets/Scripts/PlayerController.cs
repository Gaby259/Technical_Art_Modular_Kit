using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private InputController _inputController;
    private CharacterController _characterController;
    
    [Header("Movement")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private PlayerControllerConfig controllerConfig;
    private Vector2 _moveInput;
    private Vector3 _currentVelocity;
    private bool _isGrounded;
    
    [Header("Look Rotation")]
    [SerializeField] private Transform lookTarget;
    private Vector2 _mouseRotation; 
    private Vector2 _mouseSensitivity;
  
  
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
    private void OnDisable() // this is for handeling the error MissingReferenceException
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
            Jump();
            Rotate();
        }
        ClampRotation();
     
    }

   private void MovementInput (Vector2 movement)
    {
       _moveInput = movement; 
    }

    private void Movement()
    {
        Vector3 targetDirection = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        Vector3 targetVelocity = targetDirection * controllerConfig.MovementSpeed;

        float acceleration = IsGrounded() ? controllerConfig.groundAcceleration : controllerConfig.AirAcceleration;
        
        _currentVelocity =  Vector3.MoveTowards(_currentVelocity, targetVelocity, acceleration  * Time.deltaTime); //Time.deltaTime is for stopping player to float 
        Vector3 horizontalFinalVelocity = new Vector3(_currentVelocity.x, 0, _currentVelocity.z);//Ignore the Y velocity and take into account x,z 
        Vector3 deceleratedVelocity = Vector3.MoveTowards(horizontalFinalVelocity, Vector3.zero, controllerConfig.groundDeceleration* Time.deltaTime); //Vector3.MoveTowards(a, b, maxDistanceDelta)

        //DECELERATION
        if (targetDirection == Vector3.zero)// if input is realized return;
        {
            _currentVelocity.x = deceleratedVelocity.x;
            _currentVelocity.z = deceleratedVelocity.z;
        }
        
    }
    

    private void RotationInput(Vector2 mouseRotation)
    {
       _mouseRotation = mouseRotation;
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up, _mouseRotation.x * controllerConfig.mouseSensitivity);
        lookTarget.Rotate(Vector3.right, -_mouseRotation.y * controllerConfig.mouseSensitivity);
    }
    
    private void ClampRotation()
    {
        //Camera Clamp Rotation
        float currentX = lookTarget.rotation.eulerAngles.x;
        if (currentX > 180) // look at the opposite direction camerabounds
        {
            if (currentX < 360 - controllerConfig.CameraBounds)
            {
                currentX = 360 - controllerConfig.CameraBounds;
            }
        }
        else if (currentX > controllerConfig.CameraBounds)
        {
            currentX = controllerConfig.CameraBounds;

        }
        Vector3 clampRotation = transform.eulerAngles;
        clampRotation.x = currentX;
        lookTarget.eulerAngles = clampRotation;
    }
    

    private void JumpInput()
    {
        
        if (IsGrounded())
        {
            _currentVelocity.y = controllerConfig.jumpHeight;
            
        }
        
    }

    private bool IsGrounded()
    {
        _isGrounded = Physics.SphereCast(transform.position, .5f, Vector3.down, out RaycastHit hit, .6f, groundLayer);
        return _isGrounded;
    }

    private void Jump()
    {
        if (!IsGrounded()) //if the player is not touching the floor do this...
        {
            _currentVelocity.y += Physics.gravity.y * controllerConfig.gravity *Time.deltaTime;
            Debug.Log("Velocity "+_currentVelocity.y);
          
        }
        _characterController.Move(_currentVelocity * Time.deltaTime);

    }

   
  
}