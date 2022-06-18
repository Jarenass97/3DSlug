using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mensaje : MonoBehaviour
{
    public TextMeshProUGUI msg;
    private Animator animText;
    IEnumerator animacionTexto()
    {
        animText = msg.GetComponent<Animator>();
        msg.enabled = true;
        animText.SetBool("mostrar", true);
        yield return new WaitForSeconds(2);
        animText.SetBool("mostrar", false);
        msg.enabled = false;
    }

    internal void mostrar(string mensaje)
    {
        msg.text = mensaje;
        StartCoroutine(animacionTexto());
    }
}
