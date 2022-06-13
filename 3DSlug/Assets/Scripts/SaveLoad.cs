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
        if (File.Exists(fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            Partida.current = (Partida)bf.Deserialize(file);
            file.Close();
        }
    }

    internal static void borrarDatos()
    {
        File.Delete(fileName);
    }
}
