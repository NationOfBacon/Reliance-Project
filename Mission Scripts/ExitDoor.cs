using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private GameManager gameManager;
    private UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Persistent Object").GetComponent<GameManager>();
        uiManager = GameObject.Find("Persistent Object").GetComponent<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Target") && gameManager.exitOpen == true)
        {
            uiManager.FinishScreen();
            gameManager.missionComplete = true;
        }
    }
}
