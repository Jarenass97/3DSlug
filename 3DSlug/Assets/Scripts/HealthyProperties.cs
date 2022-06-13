using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthyProperties : MonoBehaviour
{
    private int healthPoints;
    void Start()
    {
        int prob = Random.Range(0, 101);
        if (prob > 80) healthPoints = Random.Range(50, 101);
        else healthPoints = Random.Range(20, 51);
    }

    public int getHealthPoints()
    {
        return healthPoints;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int vida = healthPoints;
            other.GetComponent<PlayerManager>().addVida(vida);
            GameObject.Find("GameManager").GetComponent<GameManager>().cogerHealthy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
