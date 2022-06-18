using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject panelTienda;
    public GameObject arma;
    private PlayerManager playerManager;
    private int precioBase = 1; // TODO cambiar
    private bool armaComprada = false;
    public TextMeshProUGUI txtMensaje;
    public TextMeshProUGUI txtCompra;

    private void Start()
    {
        playerManager = GameObject.Find("PlayerArmature").GetComponent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int precio = precioSegunDificultad();
            txtCompra.text = "Compra este arma por " + precio + " puntos!";
            if (!armaComprada) panelTienda.SetActive(true);
            else mostrarMensaje("Ya tienes este arma");
        }
    }

    private int precioSegunDificultad()
    {
        int precio = precioBase;
        int dif = GameObject.Find("GameManager").GetComponent<GameManager>().dificultad;
        switch (dif)
        {
            case 2:
                precio = (int)Math.Ceiling(precioBase * 1.25f);
                break;
            case 3:
                precio = (int)Math.Ceiling(precioBase * 1.5f);
                break;
        }
        return precio;
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
        int precio = precioSegunDificultad();
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
