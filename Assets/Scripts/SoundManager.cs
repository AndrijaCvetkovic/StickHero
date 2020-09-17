using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

    public bool soundOff = false;
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void SoundOnOff()
    {
        if(soundOff)
        {
            soundOff = false;
            AudioListener.volume = 1f;
            PlayerPrefs.SetInt("soundOn", 1);
        }
        else
        {
            soundOff = true;
            AudioListener.volume = 0f;
            PlayerPrefs.SetInt("soundOn", 0);
        }
    }

}
