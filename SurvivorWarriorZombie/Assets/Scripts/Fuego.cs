using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuego : MonoBehaviour
{
    private int damage = 5;
    private BoxCollider bc;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider>();
        StartCoroutine(activarDesactivar());
    }
    private void Update()
    {
        transform.position = new Vector3(target.transform.position.x, 0.6f, target.transform.position.z);
        if (target.GetComponent<EnemyHealth>().isDead) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            eh.recibeDamage(damage);
        }
    }

    IEnumerator activarDesactivar()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            bc.enabled = false;
            bc.enabled = true;
        }
    }

    internal void setTarget(GameObject target)
    {
        this.target = target;
    }
}
