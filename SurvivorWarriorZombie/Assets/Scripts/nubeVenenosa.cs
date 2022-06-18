using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nubeVenenosa : MonoBehaviour
{
    private int damage = 5;
    private BoxCollider bc;
    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider>();
        StartCoroutine(activarDesactivar());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            EnemyHealth eh= other.GetComponent<EnemyHealth>();
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
}
