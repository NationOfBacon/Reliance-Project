using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MouseTools;

public class MouseReload : MonoBehaviour
{
    private CustomCursor mouseController;


    private void Start()
    {
        mouseController = GameObject.Find("Mouse Base").GetComponent<CustomCursor>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseController.animControl.GetBool("Reloading") == false)
        {
            mouseController.SetAnimBool("Reloading", true);
        }
    }
}
