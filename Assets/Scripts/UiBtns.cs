using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBtns : MonoBehaviour
{
    public GameplayController controller;
    public Image soundOff;

    private void Start()
    {
        controller = Camera.main.GetComponent<GameplayController>();
        GetComponent<Button>().onClick.AddListener(() => SoundOnOff());
       if(SoundManager.Instance.soundOff)
        {
            soundOff.gameObject.active = true;
        }
       else
            soundOff.gameObject.active = false;
    }
    private void OnMouseOver()
    {
        controller.mouseOverUi = true;
    }

    private void OnMouseExit()
    {
        controller.mouseOverUi = false;
    }

    public void SoundOnOff()
    {
        SoundManager.Instance.SoundOnOff();
        if (SoundManager.Instance.soundOff)
        {
            soundOff.gameObject.active = true;
        }
        else
            soundOff.gameObject.active = false;
    }
}
