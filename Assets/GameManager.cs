using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    
    private int mazeSeed = 1;
    
    private int difficulty = 0; //0 = easy, 1 = medium, 2 = hard
    
    public TMP_InputField seedInput;
    //singleton
    public static GameManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
        
        //persist across scenes
        DontDestroyOnLoad(gameObject);
    }
    
    public int GetMazeSeed()
    {
        return mazeSeed;
    }
    
    public void SetMazeSeed()
    {
        int seed = int.Parse(seedInput.text);
        //make sure a valid int is passed
        if (seed < 1)
        {
            seed = 1;
        }
        mazeSeed = seed;
    }
    
    public void ChangeDifficulty(int diff)
    {
        Debug.Log(diff);
        difficulty = diff;
    }
    
    public int GetDifficulty()
    {
        return difficulty;
    }
    
   
}
