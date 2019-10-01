using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveMarker : MonoBehaviour //controls marking of objectives for when they are out of view
{
    private Image indicatorImage;
    private GameObject currentObj;
    private SpawnObjectives spawnObjs;
    private GameManager gameMgr;

    private bool exitSet = false;

    private void Awake()
    {
        spawnObjs = GameObject.Find("EventSystem").GetComponent<SpawnObjectives>();
        gameMgr = GameObject.Find("Persistent Object").GetComponent<GameManager>();
        indicatorImage = GetComponent<Image>();
    }
    private void Start()
    {
        GetObjective();
        Debug.Log("Set objective marker target");
    }

    private void Update()
    {
        if(currentObj != null) //if there is an objective, set the indicator
            ActivateIndicator();

        if (gameMgr.exitOpen == true && exitSet == false)
        {
            OnMissionObjectiveComplete();
            exitSet = true;
        }
    }

    public void OnMissionObjectiveComplete() //when the exit opens, set it as the marker objective
    {
        currentObj = GameObject.Find("Exit Doorway");
    }

    public void ActivateIndicator()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentObj.transform.position); //locates the screen position of the objective relative to the camera

        //when the objective is on screen, remove the marker
        if(screenPos.z>0 && screenPos.x>0 && screenPos.x<Screen.width && screenPos.y >0 && screenPos.y<Screen.height)
        {
            indicatorImage.enabled = false;
        }
        else //when the objective is offscreen, create a marker pointing to it
        {
            if (screenPos.z<0)
            {
                screenPos.z += -1;
            }

            Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2; //sets the "screen center" to the middle of the screen rather than the default bottom left
            Debug.DrawLine(transform.position, screenCenter, Color.red);

            screenPos -= screenCenter;

            float angle = Mathf.Atan2(screenPos.y, screenPos.x);
            angle -= 90 * Mathf.Deg2Rad;

            float cos = Mathf.Cos(angle);
            float sin = -Mathf.Sin(angle);

            screenPos = screenCenter + new Vector3(sin * 150, cos * 150, 0);

            float m = cos / sin;

            Vector3 screenBounds = screenCenter * 0.9f; //creates a boundary so that the indicator does not go to the edge of the screen

            // using the information from cos and sin, set the screenPos based on the boundary, takes into account what side of the screen the objective is on (top, bottom, left, right)
            if(cos>0) //top of screen
            {
                screenPos = new Vector3(screenBounds.y / m, screenBounds.y, 0);
            }
            else //bottom of screen
            {
                screenPos = new Vector3(-screenBounds.y / m, -screenBounds.y, 0);
            }

            if(screenPos.x>screenBounds.x) //left of screen
            {
                screenPos = new Vector3(screenBounds.x, screenBounds.x * m, 0);
            }
            else if(screenPos.x<-screenBounds.x) //right of screen
            {
                screenPos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
            }

            screenPos += screenCenter;

            //set the position and rotation of the indicator and enable the image so the player can see it
            indicatorImage.gameObject.transform.position = screenPos;
            indicatorImage.gameObject.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            indicatorImage.enabled = true;
        }
    }

    public void GetObjective() //when the level is loaded, get the objective from the spawnObjs script and check each instance to see if it is not null
    {
        if (spawnObjs.capAreaInstance != null) //assault mode
        {
            currentObj = spawnObjs.capAreaInstance;
        }
        else if(spawnObjs.terminalInstance != null) //hack mode
        {
            currentObj = spawnObjs.terminalInstance;
        }
        else if (spawnObjs.structureInstance != null) //defend / destroy mode
        {
            currentObj = spawnObjs.structureInstance;
        }
        else if (spawnObjs.allFriendBotInstances.Count > 0) //protect mode
        {
            currentObj = spawnObjs.allFriendBotInstances[0];
        }
        else //if there are no objectives, turn off the indicator
        {
            indicatorImage.enabled = false;
        }
    }
}
