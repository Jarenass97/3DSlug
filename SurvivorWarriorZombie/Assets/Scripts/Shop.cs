using Assets.Scripts;
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
    public int precioBase;
    private bool armaComprada = false;
    public TextMeshProUGUI txtMensaje;
    private TextMeshProUGUI txtCompra;
    public Afinidad afinidad;

    private void Start()
    {
        playerManager = GameObject.Find("PlayerArmature").GetComponent<PlayerManager>();
        txtCompra = panelTienda.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int precio = precioSegunDificultad();
            txtCompra.text = setMensaje(precio);
            if (!armaComprada) panelTienda.SetActive(true);
            else mostrarMensaje("Ya está en tu posesión");
        }
    }

    private string setMensaje(int precio)
    {
        string text = "";
        switch (afinidad)
        {
            case Afinidad.SIN_AFINIDAD:
                text = "Compra este arma por " + precio + " puntos!";
                break;
            case Afinidad.VENENO:
                text = "Concede afinidad tóxica a tu pistola por " + precio + " puntos!";
                break;
            case Afinidad.FUEGO:
                text = "Concede afinidad ínflamable a tu pistola por " + precio + " puntos!";
                break;
        }
        return text;
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
            playerManager.venderArma(arma, precio, afinidad);
            armaComprada = true;
            mostrarMensaje("Compra realizada con éxito");
            panelTienda.SetActive(false);
        }
        else
        {
            mostrarMensaje("No tienes suficientes puntos");
            panelTienda.SetActive(false);
        }
    }
    private void mostrarMensaje(string mensaje)
    {
        txtMensaje.GetComponent<Mensaje>().mostrar(mensaje);
    }

    public void setNewObjects(GameObject shopPanel, TextMeshProUGUI txtMensaje)
    {
        this.panelTienda = shopPanel;
        this.txtMensaje = txtMensaje;
        txtCompra = panelTienda.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        playerManager = GameObject.Find("PlayerArmature").GetComponent<PlayerManager>();
    }
}
