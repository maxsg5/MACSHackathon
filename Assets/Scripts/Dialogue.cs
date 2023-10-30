using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public bool isFinalDialogue = false;
    public GameObject WinScreen;
    public AudioMixer audioSource;
    public AudioClip audioClip;
    private AudioSource sound;
    bool noAudio = false;
    
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        textDisplay.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textDisplay.text == sentences[index])
            {
                PlaySound();
                NextLine();
            }
            else
            {
                PlaySound();
                StopAllCoroutines();
                textDisplay.text = sentences[index];
            }
            
        }
    }
    
    void StartDialogue()
    {
        StartCoroutine(Type());
    }

    void PlaySound()
    {
        if (noAudio)
        {
            return;
        }
        sound.PlayOneShot(audioClip);
    }
    
    IEnumerator Type()
    {
        foreach (char letter in sentences[index])
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed); 
        }
    }
    
    void NextLine()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = string.Empty;
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = string.Empty;
            if (isFinalDialogue)
            {
                noAudio = true;
                audioSource.StopAllAudio();
                //show win screen
                WinScreen.SetActive(true);
                LevelManager.Instance.isGameOver = true;
            }
            else
            {
              //get playercontroller and set canMove to true
              GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownController>().canMove = true;
              gameObject.SetActive(false);  
            }
            
        }
    }
}
