using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyRespawnPointsGraveyard;
    private GameObject[] enemyRespawnPointsCastle;
    public GameObject[] enemies;
    private GameObject[] healthyRespawnPoints;
    public GameObject[] healthyRespawnPointsGraveyard;
    private GameObject[] healthyRespawnPointsCastle;
    public GameObject healthyObject;
    private int numEnemies = 0;
    private int ronda = 0;
    private int rondaFinal = 100;
    public int rondaCambioEscena = 3;
    public GameObject GamePanel;
    public GameObject GameOverPanel;
    public GameObject GameIntroPanel;
    public GameObject GamePausePanel;
    public GameObject GameWinPanel;
    public GameObject menuPpal;
    public GameObject menuDiff;
    public TextMeshProUGUI msg;
    public TextMeshProUGUI contadorEnemigos;
    public TextMeshProUGUI contadorRondas;
    public TextMeshProUGUI resultadoPartida;
    public TextMeshProUGUI txtButtonDificultad;
    private bool isGameOver = false;
    private ThirdPersonController tpc;
    private ArrayList healthyRespawnsUtilizados = new ArrayList();
    private bool isInGame = false;
    private bool firstScene = true;
    private int dificultad = 1;
    void Start()
    {
        comprobarDatosGuardados();
        tpc = GameObject.Find("PlayerArmature").GetComponent<ThirdPersonController>();
        txtButtonDificultad.text = "Dificultad: Fácil";
        //comenzarPartida();
        //StartCoroutine(prueba());
    }

    IEnumerator prueba()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(pasarEscena());
    }

    private void comprobarDatosGuardados()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            Scene scene = (Scene)bf.Deserialize(file);
            file.Close();
            SceneManager.SetActiveScene(scene);
        }
    }

    void Update()
    {
        //tpc.lanzagranada();
    }

    private void nextLevel()
    {
        if (!isGameOver && isInGame)
        {
            if (firstScene && ronda == rondaCambioEscena) StartCoroutine(pasarEscena());
            else
            {
                if (ronda <= rondaFinal)
                {
                    ronda++;
                    numEnemies = Random.Range(dificultad * ronda, (2 * dificultad * ronda) + 1);
                    contadorEnemigos.text = "Enemigos: " + numEnemies;
                    contadorRondas.text = "Ronda: " + ronda;
                    if (firstScene) respawnEnemies(enemyRespawnPointsGraveyard);
                    else respawnEnemies(enemyRespawnPointsCastle);
                }
                else finGame();
            }
        }
    }

    IEnumerator pasarEscena()
    {
        firstScene = false;
        tpc.enabled = false;
        tpc.mostrarMensaje("Ronda completada");
        yield return new WaitForSeconds(2);
        GameObject player = GameObject.Find("PlayerArmature");
        GameObject respawnPlayer = GameObject.Find("respawnPlayer");
        player.gameObject.transform.position = new Vector3(respawnPlayer.transform.position.x, player.transform.position.y, respawnPlayer.transform.position.z);
        player.gameObject.transform.rotation = respawnPlayer.transform.rotation;
        GameObject.Find("MainCamera").gameObject.transform.position = player.transform.position;
        GameObject.Find("MainCamera").gameObject.transform.rotation = player.transform.rotation;
        yield return new WaitForSeconds(0.1f);
        tpc.enabled = true;
        DontDestroyOnLoad(GameObject.Find("Player"));
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("Castle");
        yield return new WaitForSeconds(0.1f);
        cargarNewRespawns();
        nextLevel();
    }

    private void cargarNewRespawns()
    {
        enemyRespawnPointsCastle = new GameObject[7];
        for (int i = 0; i < enemyRespawnPointsCastle.Length; i++)
        {
            enemyRespawnPointsCastle[i] = GameObject.Find("RespawnCastle (" + (i + 1) + ")");
        }
        healthyRespawnPointsCastle = new GameObject[8];
        for (int i = 0; i < healthyRespawnPointsCastle.Length; i++)
        {
            healthyRespawnPointsCastle[i] = GameObject.Find("HealthyRespawnCastle (" + (i + 1) + ")");
        }
        healthyRespawnPoints = healthyRespawnPointsCastle;
        healthyRespawnsUtilizados.Clear();
    }

    private void finGame()
    {
        isInGame = false;
        GameWinPanel.SetActive(true);
        GamePanel.SetActive(false);
        destroyEnemies();
    }

    private void destroyEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject e in enemies) Destroy(e.gameObject);
    }

    public bool isGamePlay()
    {
        return isInGame;
    }

    private void respawnEnemies(GameObject[] respawnPoints)
    {
        ArrayList respawns = new ArrayList(respawnPoints);
        GameObject respawnPoint;
        for (int i = 0; i < numEnemies; i++)
        {
            if (respawns.Count == 0) respawns = new ArrayList(respawnPoints);
            int enemyIndex = Random.Range(0, enemies.Length);
            respawnPoint = (GameObject)respawns[Random.Range(0, respawns.Count)];
            respawns.Remove(respawnPoint);
            Instantiate(
                enemies[enemyIndex],
                respawnPoint.transform.position,
                respawnPoint.transform.rotation
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
        resultadoPartida.text = "Has llegado a la ronda " + ronda + ", ¡Vuelve a intentarlo!";
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
        healthyRespawnPoints = healthyRespawnPointsGraveyard;
        StartCoroutine(generateHealthy());
    }

    public void elegirDificultad()
    {
        menuPpal.SetActive(false);
        menuDiff.SetActive(true);
    }

    public void easy()
    {
        txtButtonDificultad.text = "Dificultad: Fácil";
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

    public void pauseGame()
    {
        GamePanel.SetActive(false);
        GamePausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void reanudarGame()
    {
        GamePanel.SetActive(true);
        GamePausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void save()
    {
        Scene scene = SceneManager.GetActiveScene();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, scene);
        file.Close();
        GamePanel.SetActive(true);
        GamePausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
