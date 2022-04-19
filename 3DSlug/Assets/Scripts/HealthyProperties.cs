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
}
