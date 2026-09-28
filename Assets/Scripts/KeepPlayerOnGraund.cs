using UnityEngine;

public class KeepPlayerOnGround : MonoBehaviour
{
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void LateUpdate()
    {
        if (controller == null)
            return;

        RaycastHit hit;

        if (Physics.Raycast(
            transform.position + Vector3.up,
            Vector3.down,
            out hit,
            5f))
        {
            float targetY =
                hit.point.y + controller.height / 2f;

            Vector3 position = transform.position;

            if (position.y < targetY)
            {
                position.y = targetY;
                transform.position = position;
            }
        }
    }
}