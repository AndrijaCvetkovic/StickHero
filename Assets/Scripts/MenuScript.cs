using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuScript : MonoBehaviour
{

    public GameObject imgSoundOff;
    public Text scoreTxt;

    private void Start()
    {
        int score = PlayerPrefs.GetInt("Highscore");
        if (score > 0)
        {
            scoreTxt.text = "HIGHSCORE \r\n " + score.ToString();
        }
        else
        {
            scoreTxt.gameObject.active = false;
        }

        if(PlayerPrefs.GetInt("soundOn") == 0)
        {
            SoundManager.Instance.soundOff = true;
            imgSoundOff.active = true;
            AudioListener.volume = 0f;
        }
        else
        {
            SoundManager.Instance.soundOff = false;
            imgSoundOff.active = false;
            AudioListener.volume = 1f;
        }

    }

    public void playGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void SoundOff()
    {
        SoundManager.Instance.SoundOnOff();
        if(imgSoundOff.active == false)
        {
            imgSoundOff.active = true;
            
        }
        else
            imgSoundOff.active = false;
    }

}
