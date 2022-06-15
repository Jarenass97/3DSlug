using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class EnemyHealth : MonoBehaviour
{
    public int vidaMaxima = 25;
    private int vidaActual;
    private Animator animator;
    private zombieMovement enemyMovement;
    private GameManager gameManager;
    private GameObject Player;
    private ThirdPersonController tpc;
    private bool isDead = false;
    void Start()
    {
        vidaActual = vidaMaxima;
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<zombieMovement>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("PlayerArmature");
        tpc = Player.GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void recibeDamage(int damage)
    {
        if (!isDead)
        {
            vidaActual -= damage;
            if (vidaActual < 0) vidaActual = 0;
            else if (vidaActual > vidaMaxima) vidaActual = vidaMaxima;
            StartCoroutine(damageAnim());
        }
    }

    IEnumerator damageAnim()
    {        
        animator.SetBool("isTakingDamage", true);
        if (vidaActual > 0)
        {            
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("isTakingDamage", false);
        }
        else
        {            
            animator.SetBool("isDead", true);
            enemyMovement.morir();
            gameManager.enemyDeath();
            isDead = true;
            aprovisionar();
            yield return new WaitForSeconds(5);            
            Destroy(gameObject);            
        }                        
    }
    private void aprovisionar()
    {
        int prob = Random.Range(1, 101);
        if (prob <= 10)
        {
            tpc.addGranadas();
        }
    }
}
