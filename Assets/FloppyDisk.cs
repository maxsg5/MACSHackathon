using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloppyDisk : MonoBehaviour
{
    private GameObject floppyUI;

    private void Start()
    {
        floppyUI = GameObject.FindGameObjectWithTag("FloppyUI");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
            
            if (other.gameObject.CompareTag("Player"))
            {
                //change opacity of floppy disk UI
                floppyUI.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
    }
}
