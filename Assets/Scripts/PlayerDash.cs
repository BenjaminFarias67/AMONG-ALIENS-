using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    [Header("Configurações do Dash")]
    public float dashDistance = 5f;
    public float dashCooldown = 2f;

    [Header("Estamina")]
    public Slider staminaBar;

    private CharacterController characterController;
    private float stamina = 1f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        stamina = 1f;

        if (staminaBar != null)
        {
            staminaBar.minValue = 0f;
            staminaBar.maxValue = 1f;
            staminaBar.value = stamina;
        }
    }

    void Update()
    {
        // Recarrega a estamina gradualmente.
        if (stamina < 1f)
        {
            stamina += Time.deltaTime / dashCooldown;
            stamina = Mathf.Clamp01(stamina);
        }

        // Atualiza a barra visual.
        if (staminaBar != null)
        {
            staminaBar.value = stamina;
        }

        // O dash só funciona quando a estamina está cheia.
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame &&
            stamina >= 1f)
        {
            Dash();
        }
    }

    void Dash()
    {
        if (characterController == null)
        {
            return;
        }

        // Gasta toda a estamina.
        stamina = 0f;

        // Executa o dash.
        characterController.Move(
            transform.forward * dashDistance
        );
    }
}
