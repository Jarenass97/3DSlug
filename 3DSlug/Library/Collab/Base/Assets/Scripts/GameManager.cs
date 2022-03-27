using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyRespawnPoints;
    public GameObject[] enemies;
    public GameObject[] healthyRespawnPoints;
    public GameObject healthyObject;
    private int numEnemies = 0;
    private int ronda = 0;
    public GameObject GamePanel;
    public GameObject GameOverPanel;
    public GameObject GameIntroPanel;
    public GameObject menuPpal;
    public GameObject menuDiff;
    public TextMeshProUGUI contadorEnemigos;
    public TextMeshProUGUI contadorRondas;
    public TextMeshProUGUI resultadoPartida;
    public TextMeshProUGUI txtButtonDificultad;
    private bool isGameOver = false;
    private ThirdPersonController tpc;
    private ArrayList healthyRespawnsUtilizados = new ArrayList();
    private bool isInGame = false;
    private int dificultad = 1;
    void Start()
    {
        tpc = GameObject.Find("PlayerArmature").GetComponent<ThirdPersonController>();
        txtButtonDificultad.text = "Dificultad: F�cil";
        comenzarPartida();//borrar
    }

    void Update()
    {

    }

    private void nextLevel()
    {
        if (!isGameOver && isInGame)
        {
            ronda++;
            numEnemies = Random.Range(dificultad * ronda, (2 * dificultad * ronda) + 1);
            contadorEnemigos.text = "Enemigos: " + numEnemies;
            contadorRondas.text = "Ronda: " + ronda;
            respawnEnemies();
        }
    }

    private void respawnEnemies()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            int enemyIndex = Random.Range(0, enemies.Length);
            int respawnPointIndex = Random.Range(0, enemyRespawnPoints.Length);
            Instantiate(
                enemies[enemyIndex],
                enemyRespawnPoints[respawnPointIndex].transform.position,
                enemyRespawnPoints[respawnPointIndex].transform.rotation
                );
        }
    }

    public void enemyDeath()
    {
        numEnemies--;
        if (numEnemies == 0) nextLevel();
        else contadorEnemigos.text = "Enemigos: " + numEnemies;
    }

    IEnumerator generateHealthy()
    {
        int seconds = 30;
        yield return new WaitForSeconds(seconds);
        while (!isGameOver)
        {
            int numResp = 0;
            if (healthyRespawnsUtilizados.Count != healthyRespawnPoints.Length)
            {
                numResp = Random.Range(1, (healthyRespawnPoints.Length - healthyRespawnsUtilizados.Count) + 1);
            }
            for (int i = 0; i < numResp; i++)
            {
                GameObject rp;
                do
                {
                    rp = healthyRespawnPoints[Random.Range(0, healthyRespawnPoints.Length)];
                } while (healthyRespawnsUtilizados.Contains(rp));
                healthyRespawnsUtilizados.Add(rp);
                Instantiate(healthyObject, rp.transform);
            }
            yield return new WaitForSeconds(seconds);
        }
    }

    public void cogerHealthy(GameObject healthy)
    {
        foreach (GameObject rp in healthyRespawnsUtilizados)
        {
            if (rp.transform.position.x == healthy.transform.position.x
                && rp.transform.position.z == healthy.transform.position.z)
            {
                healthyRespawnsUtilizados.Remove(rp);
                break;
            }
        }
    }

    public void gameOver()
    {
        isGameOver = true;
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(true);
        resultadoPartida.text = "Has llegado a la ronda " + ronda + ", �Vuelve a intentarlo!";
        tpc.enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void comenzarPartida()
    {
        GameIntroPanel.SetActive(false);
        GamePanel.SetActive(true);
        isInGame = true;
        nextLevel();
        StartCoroutine(generateHealthy());
    }

    public void elegirDificultad()
    {
        menuPpal.SetActive(false);
        menuDiff.SetActive(true);
    }

    public void easy()
    {
        txtButtonDificultad.text = "Dificultad: F�cil";
        dificultad = 1;
        menuDiff.SetActive(false);
        menuPpal.SetActive(true);
    }

    public void medio()
    {
        txtButtonDificultad.text = "Dificultad: Medio";
        dificultad = 2;
        menuDiff.SetActive(false);
        menuPpal.SetActive(true);
    }

    public void dificil()
    {
        txtButtonDificultad.text = "Dificultad: Dificil";
        dificultad = 3;
        menuDiff.SetActive(false);
        menuPpal.SetActive(true);
    }

}
