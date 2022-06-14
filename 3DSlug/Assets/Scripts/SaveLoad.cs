using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveLoad
{
    private static string fileName = Application.persistentDataPath + "/datosGuardados.swz";

    public static void save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(fileName);
        bf.Serialize(file, Partida.current);
        file.Close();
    }

    public static void Load()
    {
        if (existeFichero())
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            Partida.current = (Partida)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void borrarDatos()
    {
        File.Delete(fileName);
    }

    public static bool existeFichero()
    {
        return File.Exists(fileName);
    }
}
