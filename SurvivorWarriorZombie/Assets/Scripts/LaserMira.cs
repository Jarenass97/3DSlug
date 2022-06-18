using Assets.Scripts;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMira : MonoBehaviour
{
    private ThirdPersonController tpc;
    private Afinidad afinidad;
    public GameObject nubeTóxica;
    private BoxCollider bc;
    private void Start()
    {
        tpc = GameObject.Find("PlayerArmature").GetComponent<ThirdPersonController>();
        bc = GetComponent<BoxCollider>();
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
                switch (afinidad)
                {
                    case Afinidad.SIN_AFINIDAD:
                        break;
                    case Afinidad.VENENO:
                        Instantiate(nubeTóxica, new Vector3(other.transform.position.x,nubeTóxica.transform.position.y,other.transform.position.z),nubeTóxica.transform.rotation);
                        StartCoroutine(espacioTiempo(3));
                        break;
                    case Afinidad.FUEGO:
                        break;
                }
            }
        }
    }
    IEnumerator espacioTiempo(int seconds)
    {
        bc.enabled = false;
        yield return new WaitForSeconds(seconds);
        bc.enabled = true;
    }

    internal void setAfinidad(Afinidad afinidad)
    {
        this.afinidad = afinidad;
    }

    internal Afinidad getAfinidad()
    {
        return afinidad;
    }
}
