using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
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

    private void Start()
    {
        vidaActual = vidaMaxima;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
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
        vidaActual -= damage;
        StartCoroutine(damageAnim());
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
    private void addVida(int vida)
    {
        vidaActual += vida;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            if (other.gameObject.CompareTag("healthy"))
            {
                int vida = other.gameObject.GetComponent<HealthyProperties>().getHealthPoints();
                addVida(vida);
                gameManager.cogerHealthy(other.gameObject);
                Destroy(other.gameObject);
            }
        }
    }
}
