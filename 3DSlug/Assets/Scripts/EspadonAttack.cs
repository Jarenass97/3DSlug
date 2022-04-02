using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspadonAttack : MonoBehaviour
{
    private bool attacking = false;
    private HealthBar health;
    private AudioSource audio;

    private void Start()
    {
        health = GameObject.Find("PlayerArmature").GetComponent<HealthBar>();
        audio = GetComponent<AudioSource>();
    }
    
    public void isAttacking(bool attacking)
    {
        this.attacking = attacking;
    }
    public void growl()
    {
        audio.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!health.isDead())
        {
            if (other.gameObject.CompareTag("enemy"))
            {
                if (attacking)
                {                    
                    EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
                    enemyHealth.recibeDamage(20);
                }
            }
        }
    }
}
