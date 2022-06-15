using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject teleportDestino;    
    private ThirdPersonController playerScript;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("PlayerArmature").GetComponent<ThirdPersonController>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("ground"))
        {
            if (other.gameObject.CompareTag("Player")) StartCoroutine(transportPlayer(other));
            else other.gameObject.transform.position = teleportDestino.transform.position;
        }
    }

    IEnumerator transportPlayer(Collision player)
    {
        audio.Play();
        playerScript.enabled = false;
        player.gameObject.transform.position = teleportDestino.transform.position;
        player.gameObject.transform.rotation = teleportDestino.transform.rotation;
        yield return new WaitForSeconds(0.1f);
        playerScript.enabled = true;
    }
}
