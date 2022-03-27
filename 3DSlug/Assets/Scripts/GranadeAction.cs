using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeAction : MonoBehaviour
{
    public ParticleSystem explosion;
    private GameObject Player;
    private Rigidbody granadeRb;
    private SphereCollider colliderExplosion;
    private AudioSource audio;
    void Start()
    {
        Player = GameObject.Find("PlayerArmature");                
        granadeRb = GetComponent<Rigidbody>();
        colliderExplosion = GetComponent<SphereCollider>();
        audio = GetComponent<AudioSource>();
        lanzar();        
    }
    
    void lanzar()
    {
        granadeRb.AddForce(fuerzaUp(), ForceMode.Impulse);
        granadeRb.AddForce(fuerzaFront(), ForceMode.Impulse);
        granadeRb.AddTorque(RandomNumber(-10, 10), RandomNumber(-10, 10), RandomNumber(-10, 10));        
    }

    private int RandomNumber(int min, int max)
    {
        return Random.Range(min, max+1);
    }

    private Vector3 fuerzaUp()
    {
        return Vector3.up * 4;
    }
    private Vector3 fuerzaFront()
    {                        
        return Player.transform.forward * 10;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("enemy"))
        {
            audio.Play();
            StartCoroutine(explotar());
        }
    }

    IEnumerator explotar()
    {
        explosion.Play();
        colliderExplosion.enabled = true;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {            
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.recibeDaño(50);
        }
    }

}
