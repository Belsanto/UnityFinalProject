using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float stoppingDistance = 2.0f;
    [SerializeField] private float retreatSpeed = 2.0f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float health = 50f;
    [SerializeField] private float timeToReactivate = 20f;
    [SerializeField] private Image canvasImage;

    private float originalSpeed;
    private bool isDead = false;
    private Transform player;
    private Vector3 initialPosition;
    private bool isReturning = false;
    private Animator ownAnimator;
    private bool isStunned = false;
    private float stunTimer = 0f;
    private GameManager gameManager;
    private Collider myCollider; // Referencia al collider que deseas desactivar


    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource wakeUpSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource playerSound;
    [SerializeField] private AudioSource hurtSound;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canvasImage.DOFade(0, .2f);
        initialPosition = transform.position;
        ownAnimator = GetComponent<Animator>();
        originalSpeed = speed;
        gameManager = GameManager.Instance;
        walkSound.Play(); // walkSound
        
        // Obtén el componente Collider adjunto a este GameObject
        myCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (isStunned)
        {
            stunTimer += Time.deltaTime;
            if (stunTimer >= 1.5f)
            {
                isStunned = false;
                speed = originalSpeed;
                ownAnimator.SetFloat("speed", 1);
                stunTimer = 0f;
            }
        }

        if (!isDead)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance < stoppingDistance && !isReturning)
            {
                Vector3 direction = player.position - transform.position;
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

                if (distance > attackRange)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * speed);
                    ownAnimator.SetFloat("speed", speed);
                    // Reproducir sonido de despertar
                    wakeUpSound.Play();
                }
                else
                {
                    ownAnimator.SetFloat("speed", 0);
                    StartCoroutine(AttackCoroutine());
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
                    Vector3 targetPosition = new Vector3(initialPosition.x, transform.position.y, initialPosition.z);
                    transform.LookAt(targetPosition);
                    transform.Translate(Vector3.forward * Time.deltaTime * retreatSpeed);
                    ownAnimator.SetFloat("speed", retreatSpeed);
                    // Reproducir sonido de herida
                    hurtSound.Play();
                }
            }
        }
    }
    
    // Método para desactivar el Collider
    private void DisableCollider()
    {
        if (myCollider != null) // Asegúrate de que el Collider no sea nulo
        {
            myCollider.enabled = false; // Desactiva el Collider
        }
    }

    // Método para activar el Collider
    private void EnableCollider()
    {
        if (myCollider != null) // Asegúrate de que el Collider no sea nulo
        {
            myCollider.enabled = true; // Activa el Collider
        }
    }
    
    IEnumerator AttackCoroutine()
    {
        ownAnimator.SetTrigger("attack2");
        yield return new WaitForSeconds(0.4f); // Ajusta el tiempo según tus necesidades

        if (!isDead && !isStunned)
        {
            Debug.Log("Golpe");
            if (gameManager.IncreaseHit())
            {
                // Reproducir sonido de disparo
                attackSound.Play();
                //Debug.Log("Attack Sound");
                // Reproducir sonido de quejido del personaje
                playerSound.Play();
                //Debug.Log("Player Sound");
                
                // Realiza el fade out y el fade in de la imagen del canvas
                canvasImage.DOFade(1, 1f).OnComplete(() =>
                {
                    canvasImage.DOFade(0, 0.2f);
                });
            }
            
        }
    }


    public void TakeDamage(float amount)
    {
        if (!isDead)
        {
            if (!isStunned)
            {
                isStunned = true;
                ownAnimator.SetTrigger("stunned");
                
                ownAnimator.SetFloat("speed", 0); // Asegurarse de que el enemigo permanezca quieto durante la duración especificada
                speed = 0;
            }
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
            else
            {
                ownAnimator.SetTrigger("damage");
                // Reproducir sonido de herida
                hurtSound.Play();
                Debug.Log("Hurt Sound");
            }
        }
    }

    public void Die()
    {
        isDead = true;
        
        initialPosition = transform.position; // Establece la nueva posición inicial como la posición actual
        ownAnimator.SetTrigger("death2");
        DisableCollider();
        StartCoroutine(ReactivateAfterTime(timeToReactivate));
        // Reproducir sonido de muerte
        deathSound.Play();
        walkSound.Stop();
        Debug.Log("Death Sound");
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
        walkSound.Play();
        // Reproducir sonido de despertar
        Debug.Log("Wake Sound");
        EnableCollider();
        wakeUpSound.Play();
    }
}
