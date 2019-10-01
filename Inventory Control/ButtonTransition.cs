using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonTransition : MonoBehaviour
{
    public Color mouseOverColor;
    public Color mouseExitColor;
    private GameObject backingObject;
    private TextMeshProUGUI buttonText;

    private void Awake()
    {
        backingObject = gameObject;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void MouseOver()
    {
        backingObject.GetComponent<Image>().color = mouseOverColor;
        buttonText.color = mouseExitColor;
    }

    public void MouseExit()
    {
        backingObject.GetComponent<Image>().color = mouseExitColor;
        buttonText.color = mouseOverColor;
    }
}
