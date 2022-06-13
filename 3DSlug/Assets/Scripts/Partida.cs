using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Partida
{
    public static Partida current;
    public int scene;
    public int ronda;
    public int puntos;
    public int dificultad;
    public int granadas;

    public Partida()
    {
        this.scene = 0;
        this.ronda = 0;
        this.puntos = 0;
        this.dificultad = 0;
        this.granadas = 0;
    }

    public static void setScene(int scene)
    {
        current.scene = scene;
    }

    public static int getScene()
    {
        return current.scene;
    }
    public static void setRonda(int ronda)
    {
        current.ronda = ronda;
    }

    public static int getRonda()
    {
        return current.ronda;
    }
    public static void setPuntos(int puntos)
    {
        current.puntos = puntos;
    }

    public static int getPuntos()
    {
        return current.puntos;
    }
    public static void setDificultad(int dificultad)
    {
        current.dificultad = dificultad;
    }

    public static int getDificultad()
    {
        return current.dificultad;
    }
    public static void setGranadas(int granadas)
    {
        current.granadas = granadas;
    }

    public static int getGranadas()
    {
        return current.granadas;
    }

    public static bool isOnFirstScene()
    {
        return current.scene == 0;
    }
}
