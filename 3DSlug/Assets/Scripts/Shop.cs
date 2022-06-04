using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject panelTienda;
    public GameObject arma;
    private PlayerManager playerManager;
    private int precio = 5; // TODO cambiar
    private bool armaComprada = false;
    public TextMeshProUGUI txtMensaje;

    private void Start()
    {
        playerManager = GameObject.Find("PlayerArmature").GetComponent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!armaComprada) panelTienda.SetActive(true);
            else mostrarMensaje("Ya tienes este arma");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            panelTienda.SetActive(false);
        }
    }

    public void comprar()
    {
        if (playerManager.puedePagar(precio))
        {
            playerManager.venderArma(arma, precio);
            armaComprada = true;
            mostrarMensaje("Arma comprada");
            panelTienda.SetActive(false);
        }
        else
        {
            mostrarMensaje("No tienes suficientes puntos para adquirir este arma");
            panelTienda.SetActive(false);
        }
    }
    private void mostrarMensaje(string mensaje)
    {
        txtMensaje.GetComponent<Mensaje>().mostrar(mensaje);
    }
}
