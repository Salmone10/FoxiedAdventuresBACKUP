using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private PlayerController _playerController;
    private FireballController _fireballController;
    private ClawStrike _clawStrike;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _fireballController = GetComponent<FireballController>();
        _clawStrike = GetComponent<ClawStrike>();
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 direction = value.ReadValue<Vector2>();
        _playerController.Direction = direction;
    }

    public void OnRoll(InputAction.CallbackContext parameter)
    {
        if (parameter.started) { _playerController.Roll(); }
    }

    public void OnJump(InputAction.CallbackContext parameter)
    {
        if (parameter.performed) { _playerController.Jump(); }
    }

    public void OnClawStriking(InputAction.CallbackContext parameter)
    {
        if (parameter.started) { _clawStrike.Strike(); }
    }
    public void OnTaunting(InputAction.CallbackContext parameter)
    {
        if (parameter.started) { _playerController.Taunt(); }
    }

    public void OnInteract(InputAction.CallbackContext parameter)
    {
        
        if (parameter.started) { _playerController.Interact(); }
    }
}