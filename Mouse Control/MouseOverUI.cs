using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MouseTools;

public class MouseOverUI : MonoBehaviour
{
    private CustomCursor mouseController;

    private void Awake()
    {

    }

    private void Start()
    {
        mouseController = GameObject.Find("Mouse Base").GetComponent<CustomCursor>();
    }

    public void SetOverUI()
    {
        mouseController.overUI = true;
    }

    public void SetExitUI()
    {
        mouseController.overUI = false;
    }
}
