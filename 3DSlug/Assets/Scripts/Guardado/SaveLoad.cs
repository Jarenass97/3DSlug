using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    private static string fileName = Application.persistentDataPath + "/datosGuardados.3ds";
    public static void Save()
    {
        Debug.Log("GUARDANDO");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(fileName);
        Game.current = new Game(GameObject.Find("Player"), GameObject.Find("GameManager"));
        bf.Serialize(file, Game.current);
        file.Close();
    }
    public static void Load()
    {        
        if (File.Exists(fileName))
        {
            Debug.Log("CARGANDO");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            Game.current = (Game)bf.Deserialize(file);
            Object.Destroy(GameObject.Find("Player"));
            Object.Destroy(GameObject.Find("GameManager"));
            reinstanciarObjeto(Game.current.getPlayer());
            reinstanciarObjeto(Game.current.getManager());
            file.Close();
        }
    }

    private static void reinstanciarObjeto(GameObject gameObject)
    {
        Object.Instantiate(
           gameObject,
           gameObject.transform.position,
           gameObject.transform.rotation
           );
    }

    internal static void deleteFile()
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }
}