using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private const float WALK_SPEED = 1.5f;
    private const float RUN_SPEED = 4.5f;
    private GameObject Player;
    public float movementSpeed;
    private float distanciaCuerpoACuerpo = 1.5f;
    private Animator animator;
    private bool isAttacking = false;
    private bool isGrowling = false;
    private bool isRunning = false;
    private AudioSource audioGrowl;
    private int damage = 15;
    private bool isDamaging = false;
    private int contDamages = 0;
    private HealthBar playerHealth;
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("PlayerArmature");
        animator = GetComponent<Animator>();
        audioGrowl = GetComponent<AudioSource>();
        StartCoroutine(growl());
        movementSpeed = WALK_SPEED;
        playerHealth = Player.GetComponent<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Vector3 playerPosition = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
            transform.LookAt(playerPosition);
            if (!isAttacking && !isGrowling || isRunning)
            {
                float distanciaActual = (Player.transform.position - transform.position).magnitude;
                if (distanciaActual > distanciaCuerpoACuerpo)
                {
                    transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
                }
                else
                {
                    StartCoroutine(attack());
                }
            }
        }
    }

    IEnumerator attack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", isAttacking);
        yield return new WaitForSeconds(0.8f);
        isDamaging = true;
        yield return new WaitForSeconds(1.4f);
        isDamaging = false;
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
        contDamages = 0;
    }

    IEnumerator growl()
    {
        yield return new WaitForSeconds(Random.Range(10, 30));
        while (!isDead)
        {
            isGrowling = true;
            animator.SetBool("isGrowling", isGrowling);
            audioGrowl.Play();
            yield return new WaitForSeconds(2);
            isGrowling = false;
            animator.SetBool("isGrowling", isGrowling);
            yield return StartCoroutine(run());
            yield return new WaitForSeconds(Random.Range(10, 30));
        }
    }

    IEnumerator run()
    {
        isRunning = true;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isWalking", !isRunning);
        movementSpeed = RUN_SPEED;
        yield return new WaitForSeconds(5);
        movementSpeed = WALK_SPEED;
        isRunning = false;
        animator.SetBool("isWalking", !isRunning);
        animator.SetBool("isRunning", isRunning);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (contDamages == 0 && isDamaging)
            {
                contDamages++;
                playerHealth.pierdeVida(damage);
            }
        }
    }

    public void morir()
    {
        isDamaging = false;
        isDead = true;
    }
}
