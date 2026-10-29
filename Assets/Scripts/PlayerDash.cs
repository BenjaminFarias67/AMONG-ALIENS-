using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    [Header("Configuracoes do Dash")]
    public float dashDistance = 5f;
    public float dashCooldown = 2f;

    [Header("Barra de Estamina")]
    public Slider staminaBar;

    private CharacterController characterController;
    private float stamina = 1f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        stamina = 1f;
        AtualizarBarra();
    }

    void Update()
    {
        // Recarrega a estamina durante 2 segundos.
        if (stamina < 1f)
        {
            float tempoRecarga = Mathf.Max(dashCooldown, 0.01f);

            stamina = Mathf.MoveTowards(
                stamina,
                1f,
                Time.deltaTime / tempoRecarga
            );
        }

        AtualizarBarra();

        // O dash acontece somente ao apertar R.
        // O Espaco continua livre para o pulo.
        if (Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame &&
            stamina >= 1f)
        {
            Dash();
        }
    }

    void Dash()
    {
        if (characterController == null)
        {
            Debug.LogError(
                "PlayerDash: CharacterController nao encontrado!"
            );
            return;
        }

        // Gasta toda a estamina.
        stamina = 0f;
        AtualizarBarra();

        // Executa o movimento do dash.
        characterController.Move(
            transform.forward * dashDistance
        );
    }

    void AtualizarBarra()
    {
        if (staminaBar != null)
        {
            staminaBar.minValue = 0f;
            staminaBar.maxValue = 1f;
            staminaBar.value = stamina;
        }
    }
}
