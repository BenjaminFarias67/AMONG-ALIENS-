using UnityEngine;

public class AlienAI : MonoBehaviour
{
    public Transform player;

    public float detectionDistance = 8f;
    public float attackDistance = 1.5f;

    public float speed = 2.5f;

    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    private float nextAttackTime = 0f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

       

        bool isChasing = distance <= detectionDistance;

        animator.SetBool("IsChasing", isChasing);

        if (isChasing)
        {
            Vector3 targetPosition = new Vector3(
                player.position.x,
                transform.position.y,
                player.position.z
            );

            if (distance > attackDistance)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    speed * Time.deltaTime
                );
            }
            else
            {
                Attack();
            }
        }

        transform.LookAt(new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        ));
    }

    void Attack()
    {
       
        if (Time.time >= nextAttackTime)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }

            nextAttackTime = Time.time + attackCooldown;
        }
    }
}


