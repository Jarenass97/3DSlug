using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using StarterAssets;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

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
    private int rondaFinal = 5;
    public int rondaCambioEscena = 3;
    public GameObject GamePanel;
    public GameObject GameOverPanel;
    public GameObject GameIntroPanel;
    public GameObject GamePausePanel;
    public GameObject GameWinPanel;
    public GameObject RankingPanel;
    public GameObject ShopPanel;
    public GameObject menuPpal;
    public GameObject menuDiff;
    public GameObject loadScenePanel;
    public GameObject arma;
    public GameObject btnNuevaPartida;
    public GameObject btnRegistrar;
    public TextMeshProUGUI msg;
    public TextMeshProUGUI btnComenzar;
    public TextMeshProUGUI contadorEnemigos;
    public TextMeshProUGUI contadorRondas;
    public TextMeshProUGUI resultadoPartida;
    public TextMeshProUGUI txtButtonDificultad;
    public TextMeshProUGUI txtNumMundo;
    public TextMeshProUGUI txtRankingNombres;
    public TextMeshProUGUI txtRankingPuntos;
    public TextMeshProUGUI txtBtnCancelConfirmRanking;
    public TMP_InputField txtIdentificadorRanking;
    private bool isGameOver = false;
    private ThirdPersonController tpc;
    private ArrayList healthyRespawnsUtilizados = new ArrayList();
    private bool isInGame = false;
    private bool firstScene = true;
    private int scene = 0;
    public int dificultad = 1;
    private PlayerManager pm;
    private FirebaseFirestore db;
    private string collection = "ranking";
    void Start()
    {
        firebaseInit();
        Partida.current = new Partida();
        tpc = GameObject.Find("PlayerArmature").GetComponent<ThirdPersonController>();
        pm = GameObject.Find("PlayerArmature").GetComponent<PlayerManager>();
        txtButtonDificultad.text = "Dificultad: Fácil";
        //comenzarPartida();
        //StartCoroutine(prueba());
        comprobarDatosGuardados();
    }

    async void firebaseInit()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    //TODO eliminar
    IEnumerator prueba()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(pasarEscena(scene + 1));
    }

    private void comprobarDatosGuardados()
    {
        SaveLoad.Load();
        if (Partida.existenDatos())
        {
            btnNuevaPartida.SetActive(true);
            btnComenzar.text = "Continuar";
            dificultad = Partida.getDificultad();
            switch (dificultad)
            {
                case 1:
                    txtButtonDificultad.text = "Dificultad: Fácil";
                    break;
                case 2:
                    txtButtonDificultad.text = "Dificultad: Medio";
                    break;
                case 3:
                    txtButtonDificultad.text = "Dificultad: Difícil";
                    break;
            }
        }
    }

    public void continuar()
    {
        comprobarArma();
        scene = Partida.getScene();
        pm.addPuntos(Partida.getPuntos());
        pm.puntosTotales = Partida.getPuntosTotales();
        ronda = Partida.getRonda() - 1;
        tpc.cargarGranadas(Partida.getGranadas());
    }

    private void comprobarArma()
    {
        if (Partida.HasArma())
        {
            tpc.equiparArma(arma, Partida.getAfinidad());
            pm.activarMira();
        }
    }

    public void guardarPartida()
    {
        Partida.setHasArma(GameObject.Find("M1911") != null);
        Partida.setScene(scene);
        Partida.setPuntos(pm.puntos);
        Partida.setPuntosTotales(pm.puntosTotales);
        Partida.setRonda(ronda);
        Partida.setDificultad(dificultad);
        Partida.setGranadas(tpc.numGranadas);
        Partida.setAfinidad(GameObject.Find("laserMira").GetComponent<LaserMira>().getAfinidad());
        SaveLoad.save();
        RestartGame();
    }

    void Update()
    {
        //tpc.lanzagranada();
    }

    private void nextLevel(bool carga = false)
    {
        if (!isGameOver && isInGame)
        {
            if (firstScene && ronda == rondaCambioEscena) StartCoroutine(pasarEscena(scene + 1));
            else
            {
                if (ronda <= rondaFinal)
                {
                    if (!carga) ronda++;
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

    IEnumerator pasarEscena(int indexScene, bool carga = false)
    {
        StartCoroutine(panelCarga(indexScene + 1));
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
        SceneManager.LoadScene(indexScene);
        yield return new WaitForSeconds(0.1f);
        cargarNewRespawns();
        cargarShopsData();
        nextLevel(carga);
        if (!carga)
        {
            scene++;
        }
    }

    private void cargarShopsData()
    {
        Shop nuevaTienda = GameObject.Find("ShopVeneno").GetComponent<Shop>();
        nuevaTienda.setNewObjects(ShopPanel, msg);
    }

    IEnumerator panelCarga(int numScene)
    {
        txtNumMundo.text = "Mundo " + numScene;
        loadScenePanel.SetActive(true);
        yield return new WaitForSeconds(3);
        loadScenePanel.SetActive(false);
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
        if (pm.isDead()) { SaveLoad.borrarDatos(); }
        Time.timeScale = 1;
        SceneManager.MoveGameObjectToScene(GameObject.Find("Player"), SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(GameObject.Find("GameManager"), SceneManager.GetActiveScene());
        SceneManager.LoadScene(0);
    }

    public void comenzarPartida()
    {
        if (Partida.existenDatos()) continuar();
        GameIntroPanel.SetActive(false);
        GamePanel.SetActive(true);
        isInGame = true;
        nextLevel();
        healthyRespawnPoints = healthyRespawnPointsGraveyard;
        StartCoroutine(generateHealthy());
        if (!Partida.isOnFirstScene())
        {
            StartCoroutine(pasarEscena(Partida.getScene(), true));
        }
    }

    public void nuevaPartida()
    {
        SaveLoad.borrarDatos();
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

    public void toRanking()
    {
        GameOverPanel.SetActive(false);
        GameWinPanel.SetActive(false);
        RankingPanel.SetActive(true);
        cargarRanking();
    }

    private string idNombre = "nombre";
    private string idPuntos = "puntos";

    private void cargarRanking()
    {
        db.Collection(collection).OrderByDescending(idPuntos).Limit(10).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            string rankingNombres = "";
            string rankingPuntos = "";
            int contador = 1;
            QuerySnapshot qSnap = task.Result;
            Debug.Log(task.Result.ToString());
            foreach (DocumentSnapshot snap in qSnap)
            {
                Dictionary<string, object> reg = snap.ToDictionary();
                rankingNombres += contador++ + ". " + reg[idNombre] + "\n";
                rankingPuntos += reg[idPuntos] + "\n";
            }
            txtRankingNombres.text = rankingNombres;
            txtRankingPuntos.text = rankingPuntos;
            Debug.Log(rankingNombres);
            Debug.Log(rankingPuntos);
        });
    }

    public void registrarEnRanking()
    {
        string nombre = txtIdentificadorRanking.text;
        if (!string.IsNullOrEmpty(nombre))
        {
            Dictionary<string, object> reg = new Dictionary<string, object>
            {
                {idNombre, nombre},
                {idPuntos, pm.puntosTotales}
            };
            db.Collection(collection).Document(nombre + "-" + pm.puntosTotales)
                .SetAsync(reg).ContinueWithOnMainThread(task =>
                {
                    cargarRanking();
                    btnRegistrar.SetActive(false);
                    txtBtnCancelConfirmRanking.text = "Confirmar";
                });
        }
    }
}
