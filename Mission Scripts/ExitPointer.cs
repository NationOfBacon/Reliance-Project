using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPointer : MonoBehaviour
{
    private GameObject playerObject;
    private GameObject exitDoor;
    private Vector3 lookVector;
    private float xPos;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("PlayerSphere");
        exitDoor = GameObject.Find("Exit Doorway");
        xPos = transform.rotation.x;
    }

    // Update is called once per frame
    void Update()
    {
        //lookVector = playerObject.transform.position - exitDoor.transform.position;
        xPos = exitDoor.transform.rotation.y - playerObject.transform.rotation.y;

        //transform.Rotate(0, xPos, 0);

        //if(transform.rotation.y > xPos)
        //{
        //    transform.Rotate(0, 0, 0);
        //}
    }
}
