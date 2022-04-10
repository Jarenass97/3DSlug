using System.Collections;
using UnityEngine;

[System.Serializable]
public class Game
{
    public static Game current;    
    private GameObject player;
    private GameObject manager;

    public Game(GameObject player, GameObject manager)
    {
        this.player = player;
        this.manager = manager;
    }
    public GameObject getPlayer()
    {
        return player;
    }
    public GameObject getManager()
    {
        return manager;
    }
}
