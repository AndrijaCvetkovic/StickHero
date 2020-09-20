using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{

    public GameObject EndWindow;
    public Image btnSoundOff;
    public GameObject stick;
    public GameObject player;
    public GameObject lastGreatedStick;
    public GameObject currentGroundObject;
    public GameObject lastGroundObject;
    public GameObject nextGameObject;

    public Transform leftBorder;

    //test 
    public GameObject groundPrefab;
    public List<GameObject> gourndPrefabs;

    public bool actionEnabled = true;
    public bool actionsStopped = false;
    public bool mousrBtnUp = true;
    public bool mouseOverUi = false;

    public Text score;
    int scorePoints = 0;
    bool failing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseOverUi == false)
        {
            if (Input.GetMouseButtonDown(0) && actionEnabled)
            {
                if(failing == false)
                    CreateNewStick();
                actionEnabled = false;
            }

            if (actionsStopped == false && lastGreatedStick != null)
            {
                lastGreatedStick.transform.localScale += new Vector3(0, 0.1f, 0);
            }

            if (Input.GetMouseButtonUp(0) && mousrBtnUp)
            {

                if (lastGreatedStick != null)
                    StartCoroutine("rotateStick");
            }
        }

        if(player.transform.localPosition.y < -600)
        {
            
            if (failing == false)
            {
                scorePoints--;
                failing = true;
                StartCoroutine("deadMethod");
            }
        }

    }

    public void CreateNewStick()
    {
        GameObject g = Instantiate(stick);
        g.transform.parent = GameObject.Find("Canvas").transform;
        g.transform.localScale = new Vector3(1, 1, 1);
        g.transform.localPosition = new Vector3(currentGroundObject.transform.localPosition.x + currentGroundObject.GetComponent<RectTransform>().sizeDelta.x/2, GameObject.Find("stickPosition").transform.localPosition.y, 0);
        g.active = true;
        lastGreatedStick = g;
        actionEnabled = false;
    }

    IEnumerator rotateStick()
    {
        actionsStopped = true;
        mousrBtnUp = false;
        int counter = 45;
        while (counter > 0)
        {
            yield return new WaitForEndOfFrame();
            lastGreatedStick.transform.Rotate(0, 0, -2f);
            counter--;
        }
        if (counter == 0)
        {
            StartCoroutine("moveChar");
            
        }
    }

    IEnumerator moveChar()
    {
        player.GetComponent<Animator>().Play("Run");
        Vector3 posEnd = lastGreatedStick.transform.GetChild(1).transform.position;

        posEnd = new Vector3(posEnd.x ,player.transform.position.y, 0);

        //while (Mathf.Abs(posEnd.x - player.transform.position.x) >= 0.2f)
        float dist = Vector2.Distance(posEnd, player.transform.position);
        while (Mathf.Abs(posEnd.x - player.transform.position.x) >= 0.1f)
        {
            yield return new WaitForEndOfFrame();
            if (dist / 3 < Mathf.Abs(posEnd.x - player.transform.position.x))
            {
                player.transform.position = Vector3.Lerp(player.transform.position, posEnd, 0.03f);
                Debug.Log("1");
            }
            else if (dist * 2 / 3 < Mathf.Abs(posEnd.x - player.transform.position.x))
            {
                player.transform.position = Vector3.Lerp(player.transform.position, posEnd, 0.04f);
                Debug.Log("2");
            }
            else
            {
                player.transform.position = Vector3.Lerp(player.transform.position, posEnd, 0.05f);
                Debug.Log("3");
            }
        }
        player.transform.position = posEnd;
        Destroy(lastGreatedStick);
       

        if(nextGameObject.transform.childCount > 0)
            if(nextGameObject.GetComponentInChildren<GroundArea>().playerInsideGroundArea)
            {
                    if (posEnd.x < nextGameObject.transform.position.x)
                        posEnd.x = nextGameObject.transform.position.x;
                    while (Mathf.Abs(posEnd.x - player.transform.position.x) >= 0.1f)
                    {
                        yield return new WaitForEndOfFrame();
                        player.transform.position = Vector3.Lerp(player.transform.position, posEnd, 0.1f);
                    }
                    player.transform.position = posEnd;

                    CreateNextGroundPointAndTranslate();
            }
            else
            {
                player.GetComponent<BoxCollider2D>().isTrigger = true;
                StartCoroutine("deadMethod");
            }
        player.GetComponent<Animator>().Play("Idle");
    }

    public void CreateNextGroundPointAndTranslate()
    {
        
        GameObject g = Instantiate(gourndPrefabs[Random.Range(0,gourndPrefabs.Count -1)]);
        g.active = true;
        g.transform.parent = lastGroundObject.transform.parent;
        g.transform.localScale = new Vector3(1, 1, 1);


        //Debug.Log("Distance " + (1200f - currentGroundObject.GetComponent<RectTransform>().sizeDelta.x - g.GetComponent<RectTransform>().sizeDelta.x / 2).ToString());
        float distance = Random.Range(200 + currentGroundObject.GetComponent<RectTransform>().sizeDelta.x/2 ,1200 - currentGroundObject.GetComponent<RectTransform>().sizeDelta.x - g.GetComponent<RectTransform>().sizeDelta.x/2 -50);
        g.transform.localPosition = new Vector3(currentGroundObject.transform.localPosition.x + distance, currentGroundObject.transform.localPosition.y, 0);

        currentGroundObject = nextGameObject;

        nextGameObject = g;

        StartCoroutine("translateGroundObjects");
        
    }

    IEnumerator translateGroundObjects()
    {
      
        player.transform.parent = currentGroundObject.transform;
        while (currentGroundObject.transform.localPosition.x  >= leftBorder.transform.localPosition.x + currentGroundObject.GetComponent<RectTransform>().sizeDelta.x/2 + 50)
        {
            yield return new WaitForEndOfFrame();
            currentGroundObject.transform.Translate(new Vector3(-0.1f, 0, 0));
            nextGameObject.transform.Translate(new Vector3(-0.1f, 0, 0));
            lastGroundObject.transform.Translate(new Vector3(-0.1f, 0, 0));
        }
        player.transform.parent = currentGroundObject.transform.parent;
        lastGroundObject = currentGroundObject;
        yield return new WaitForSeconds(0.5f);
        scorePoints++;
        score.text = scorePoints.ToString();
        actionsStopped = false;
        actionEnabled = true;
        mousrBtnUp = true;
    }

    IEnumerator deadMethod()
    {
        yield return new WaitForSeconds(1f);
        EndWindow.transform.SetAsLastSibling();
        EndWindow.active = true;
        if (PlayerPrefs.GetInt("Highscore") < scorePoints + 1)
            PlayerPrefs.SetInt("Highscore", scorePoints + 1);

    }

    public void GoBack()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("Gameplay");
    }

}
