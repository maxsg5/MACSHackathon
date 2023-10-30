using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    
    private int mazeSeed = 1;
    
    private int difficulty = 0; //0 = easy, 1 = medium, 2 = hard
    
    private TMP_InputField seedInput;
    private GameObject dropdown;
    private TMP_Dropdown difficultyDropdown;
    //singleton
    public static GameManager _instance;

    private void Awake()
    {
        seedInput = GameObject.Find("SeedInput").GetComponent<TMP_InputField>();
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
        
        //persist across scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dropdown = GameObject.FindWithTag("dropdown");
        difficultyDropdown = dropdown.GetComponent<TMP_Dropdown>();
        difficultyDropdown.onValueChanged.AddListener(delegate { ChangeDifficulty(difficultyDropdown.value); });
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "menu")
        {
            dropdown = GameObject.FindWithTag("dropdown");
            difficultyDropdown = dropdown.GetComponent<TMP_Dropdown>();
            difficultyDropdown.onValueChanged.AddListener(delegate { ChangeDifficulty(difficultyDropdown.value); });
            seedInput = GameObject.Find("SeedInput").GetComponent<TMP_InputField>();
        }
    }

    public int GetMazeSeed()
    {
        return mazeSeed;
    }
    
    public void SetMazeSeed()
    {
        int seed = Random.Range(1, 1000000);
        //check if seedInput.text has anything
        if (seedInput.text != "")
        {
            int.TryParse(seedInput.text, out seed);
        }
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
