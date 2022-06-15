using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMira : MonoBehaviour
{
    private ThirdPersonController tpc;

    private void Start()
    {
        tpc = GameObject.Find("PlayerArmature").GetComponent<ThirdPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!tpc.isShotting())
        {
            if (other.CompareTag("enemy"))
            {
                EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.recibeDamage(5);
                tpc.disparar();
            }
        }
    }
}
