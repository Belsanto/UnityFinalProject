using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float stoppingDistance = 2.0f;
    [SerializeField] private float retreatSpeed = 2.0f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float health = 50f;
    [SerializeField] private float timeToReactivate = 5f;

    private bool isDead = false;
    private Transform player;
    private Vector3 initialPosition;
    private bool isReturning = false;
    private Animator ownAnimator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = transform.position;
        ownAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < stoppingDistance && !isReturning)
            {
                Vector3 direction = player.position - transform.position;
                transform.LookAt(player);
                if (distance > attackRange)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * speed);
                    ownAnimator.SetFloat("speed", speed);
                }
                else
                {
                    ownAnimator.SetTrigger("attack" + Random.Range(1, 4));
                }
            }
            else
            {
                isReturning = true;
                ReturnToInitialPosition();
                if (Vector3.Distance(transform.position, initialPosition) < 0.05f)
                {
                    isReturning = false;
                    ownAnimator.SetFloat("speed", 0);
                }
                else
                {
                    transform.LookAt(initialPosition);
                    transform.Translate(Vector3.forward * Time.deltaTime * retreatSpeed);
                    ownAnimator.SetFloat("speed", retreatSpeed);
                }
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (!isDead)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
            else
            {
                ownAnimator.SetTrigger("damage");
            }
        }
    }

    public void Die()
    {
        isDead = true;
        initialPosition = transform.position; // Establece la nueva posición inicial como la posición actual
        ownAnimator.SetTrigger("death2");
        StartCoroutine(ReactivateAfterTime(timeToReactivate));
    }

    void ReturnToInitialPosition()
    {
        if (!isDead)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, retreatSpeed * Time.deltaTime);
        }
    }

    IEnumerator ReactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isDead = false;
        health = 50f; // Reinicia la salud
        ownAnimator.SetTrigger("wake");
    }
}
