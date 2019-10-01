using System.Collections;
using System.Linq; //used to enable access to the distinct method
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnLorePress : MonoBehaviour
{
    private TextMeshProUGUI loreText;
    private GameObject loreBlocker;

    private string loreEntry;

    private List<GameObject> children = new List<GameObject>();


    private void Start()
    {
        loreText = GameObject.Find("HUBUI/Information Panel/Lore Text").GetComponent<TextMeshProUGUI>();

        foreach (Transform child in transform.GetComponentInChildren<Transform>())
        {
            children.Add(child.transform.gameObject); //adds each child of the specific button to the children list
        }

        if (loreBlocker == null)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].name == "Blocker Panel")
                {
                    loreBlocker = children[i];
                }
            }
        }
    }

    private void Update()
    {
        if (loreEntry != null) //finds the lore entry and also controls if the blocker is active or not
        {
            loreBlocker.SetActive(false);
        }
        else
            loreBlocker.SetActive(true);
    }

    public void OnPressed()
    {
        loreText.text = loreEntry; //when the lore button is pressed, this changes the loreText to whatever the description of the item is
    }

    public void TextUpdate(string newLoreText)
    {
        //takes info from lore script and applies to variables
        loreEntry = newLoreText;
    }

}
