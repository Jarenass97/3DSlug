using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Image healthBar;
    public TextMeshProUGUI healthPoints;
    private float vidaActual;
    private int vidaMaxima = 100;
    private bool dead = false;
    private GameManager gameManager;
    private Animator anim;
    public Avatar attackAvatar;
    public Avatar movementAvatar;
    private AudioSource audio;
    public int puntos = 0;
    public int puntosTotales = 0;
    public TextMeshProUGUI contadorPuntos;
    private List<GameObject> armas;
    private ThirdPersonController tpc;
    public GameObject puntoMira;

    private void Start()
    {
        vidaActual = vidaMaxima;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        armas = new List<GameObject>();
        tpc = GetComponent<ThirdPersonController>();
    }

    internal bool puedePagar(int precio)
    {
        return precio <= puntos;
    }

    internal void venderArma(GameObject arma, int precio)
    {
        addPuntos(-precio);
        armas.Add(arma);
        tpc.equiparArma(arma);
        puntoMira.SetActive(true);
    }

    void Update()
    {
        if (!dead)
        {
            if (vidaActual <= 0)
            {
                vidaActual = 0;
                dead = true;
                gameManager.gameOver();
            }
            if (vidaActual > vidaMaxima) vidaActual = vidaMaxima;
            healthBar.fillAmount = vidaActual / vidaMaxima;
            healthPoints.text = vidaActual.ToString();
        }
    }

    public bool isDead()
    {
        return dead;
    }

    public void pierdeVida(int damage)
    {
        if (!dead)
        {
            vidaActual -= damage;
            StartCoroutine(damageAnim());
        }
    }
    IEnumerator damageAnim()
    {
        anim.avatar = attackAvatar;
        anim.SetBool("isTakingDamage", true);
        audio.Play();
        yield return new WaitForSeconds(0.5f);
        if (dead) anim.SetBool("isDead", true);
        else
        {
            anim.SetBool("isTakingDamage", false);
            anim.avatar = movementAvatar;
        }
    }
    public void addVida(int vida)
    {
        vidaActual += vida;
    }

    public void addPuntos(int puntos)
    {
        this.puntos += puntos;
        contadorPuntos.text = "Puntos: " + this.puntos;
        if (puntos >= 0) puntosTotales += puntos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            if (other.gameObject.CompareTag("shop"))
            {
                shopActive = other.gameObject.GetComponent<Shop>();
            }
        }
    }
    private Shop shopActive = null;
    public void comprarArma()
    {
        shopActive.comprar();
    }

    internal void activarMira()
    {
        puntoMira.SetActive(true);
    }
}
