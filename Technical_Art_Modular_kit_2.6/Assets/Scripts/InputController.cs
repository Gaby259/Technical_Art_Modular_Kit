using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private GameControls _gameControls;
    
    //Player Controls
    public event Action JumpEvent;
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> MouseLookEvent;

  
  private void Awake()
    {
        _gameControls = new GameControls();
    }

    private void OnEnable()
    {
        _gameControls.Player.Enable();
        
        _gameControls.Player.Move.performed += OnMovePerformed;
        _gameControls.Player.Move.canceled += OnMoveCancelled;
        _gameControls.Player.Jump.performed += OnJumpPerformed; //performed a jump call this function
        _gameControls.Player.Look.performed += OnMouseMove;
        _gameControls.Player.Look.canceled += OnMouseMoveCancelled;
        
    }
 
 
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(Vector2.zero);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke();
        
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
       MouseLookEvent?.Invoke(context.ReadValue<Vector2>());
    }
    private void OnMouseMoveCancelled(InputAction.CallbackContext context)
    {
        MouseLookEvent?.Invoke(Vector2.zero);
    }
   
}