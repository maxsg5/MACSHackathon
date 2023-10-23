using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloppyDisk : MonoBehaviour
{
    private GameObject floppyUI;
    private TopDownController player;

    private void Start()
    {
        floppyUI = GameObject.FindGameObjectWithTag("FloppyUI");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
            
            if (other.gameObject.CompareTag("Player"))
            {
                //change opacity of floppy disk UI
                floppyUI.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                player.hasFloppyDisk = true;
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
    }
}
