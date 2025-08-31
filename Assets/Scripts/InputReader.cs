using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private PlayerController _playerController;
    private FireballController _fireballController;
    private ClawStrike _clawStrike;
    private VenomShooter _venomShooter;
    private Animator _animator;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _fireballController = GetComponent<FireballController>();
        _clawStrike = GetComponent<ClawStrike>();
        _venomShooter = GetComponent<VenomShooter>();
        _animator = GetComponent<Animator>();
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

    public void OnDash(InputAction.CallbackContext parameter)
    {
        if (parameter.started) { _playerController.Dash(); }
    }

    public void OnJump(InputAction.CallbackContext parameter)
    {
        if (_playerController.CompareTag("SnakePlayer"))
        {
            if (parameter.started)
            {
                _playerController.StartChargeJump();
                print("StartCHARGE");
            }
            else if (parameter.canceled)
            {
                _playerController.ReleaseChargeJump();
                print("Release");
            }
            return;
        }

        if (parameter.performed) { _playerController.CommonJump(); }
    }

    public void OnClawStriking(InputAction.CallbackContext parameter)
    {
        if (parameter.started) { _clawStrike.Strike(); }
    }
    public void OnInteract(InputAction.CallbackContext parameter)
    {
        if (parameter.started) { _playerController.Interact(); }
    }

    public void OnSpitting(InputAction.CallbackContext parameter)
    {
        if (parameter.started) { _animator.SetTrigger("is_attack"); }
    }
}