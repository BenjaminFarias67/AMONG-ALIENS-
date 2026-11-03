
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCorrida : MonoBehaviour
{
    [Header("Configuracoes da Corrida")]
    public float velocidadeNormal = 5f;
    public float velocidadeCorrendo = 9f;

    private FirstPersonController movimento;

    void Start()
    {
        movimento = GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (movimento == null)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.leftShiftKey.isPressed)
        {
            movimento.MoveSpeed = velocidadeCorrendo;
        }
        else
        {
            movimento.MoveSpeed = velocidadeNormal;
        }
    }
}
