using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShoot : MonoBehaviour
{
    private Animator animator;

    public float damage = 20f;
    public float range = 100f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("Shoot");
            Shoot();
        }
    }

    void Shoot()
    {
        Camera cameraPlayer = Camera.main;

        if (cameraPlayer == null)
            return;

        Ray ray = cameraPlayer.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("TIRO ACERTOU: " + hit.collider.name);

            AlienHealth alienHealth =
                hit.collider.GetComponentInParent<AlienHealth>();

            if (alienHealth != null)
            {
                alienHealth.TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("TIRO NÃO ACERTOU NADA.");
        }
    }
}


