using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        //turn maze generator on
        GameObject mazeGenerator = GameObject.Find("Maze");
        mazeGenerator.GetComponent<MazeGenerator>().enabled = true;
    }

    public void SwitchToMainMenu()
    {
        //load main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }
}
