using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundArea : MonoBehaviour
{

    public bool playerInsideGroundArea;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            
            Camera.main.GetComponent<GameplayController>().currentGroundObject = transform.parent.gameObject;
            playerInsideGroundArea = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerInsideGroundArea = false;
        }
    }

}
