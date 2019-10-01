using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioClip clip;
    public AudioClip clip2;
    public AudioClip clip3;

    private AudioSource audioS;

    private Scene currentScene;

    private void Start() //the start method is called every time a new scene is loaded, even for persistent objects (apparently)
    {
        audioS = GetComponent<AudioSource>();

        audioS.clip = clip2; //when the main menu first loads, it does not trigger a level load so this will set the default music for when the game starts
        audioS.Play();
    }

    public void SetAudio(Scene targetScene)
    {
        var previousScene = currentScene; //hold the last scene the player was at
        currentScene = targetScene;

        if (currentScene.name == "HUB scene" && previousScene.name != "Main Menu")
        {
            audioS.clip = clip2;
            audioS.Play();
        }
        else if (currentScene.name == "Main Menu" && previousScene.name != "HUB scene")
        {
            audioS.clip = clip2;
            audioS.Play();
        }
        else if (currentScene.name == "Level 1")
        {
            audioS.clip = clip; //set the audio clip for the audio source so that it can loop the audio
            audioS.Play();
        }
    }
}
