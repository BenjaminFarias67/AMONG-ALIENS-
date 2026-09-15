using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public float dashDistance = 5f;
    public float dashCooldown = 1f;

    private CharacterController characterController;
    private float nextDashTime = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame && Time.time >= nextDashTime)
        {
            Dash();
        }
    }

    void Dash()
    {
        characterController.Move(transform.forward * dashDistance);
        nextDashTime = Time.time + dashCooldown;
    }
}

