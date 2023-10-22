using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void PlayGame() 
    {
        SceneManager.LoadScene(1); //this should load the index 1 from the scene builder
    }
    public void QuitGame() 
    {
        Application.Quit(); // this will quit the current game
    }
}
