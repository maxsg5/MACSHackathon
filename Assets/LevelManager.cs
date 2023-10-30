using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject jumpScareScreen;
    public bool isGameOver = false;
    public static LevelManager Instance;
    private void Awake()
    {
        //sets up class to be singleton, only one will be run during the whole game
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            Instance = this;
        }
        //turn maze generator on
        GameObject mazeGenerator = GameObject.Find("Maze");
        mazeGenerator.GetComponent<MazeGenerator>().enabled = true;
    }

    public void SwitchToMainMenu()
    {
        //load main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }

    public void ShowScare() 
    {
        jumpScareScreen.SetActive(true);
    }

    public void HideScare() 
    {
        jumpScareScreen.SetActive(false);
    }
}
