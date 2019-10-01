using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lore : MonoBehaviour
{
    private GameObject loreObject;
    private int loreID;
    private int type;
    private string itemDesc;
    private Sprite icon;

    private GameObject worldOptions;
    private GameObject factionOptions;
    private GameObject locationOptions;
    private GameObject botOptions;
    private List<GameObject> allLoreTabs = new List<GameObject>();

    private OnLorePress lorePress;

    private void Awake()
    {
        worldOptions = GameObject.Find("HUBUI/Information Panel/World Options");
        factionOptions = GameObject.Find("HUBUI/Information Panel/Faction Options");
        locationOptions = GameObject.Find("HUBUI/Information Panel/Location Options");
        botOptions = GameObject.Find("HUBUI/Information Panel/Bot Options");
    }

    void Start()
    {

        //Adds the children objects of each "Options" object to the allLoreTabs list
        foreach (Transform child in worldOptions.transform.GetComponentInChildren<Transform>())
        {
            allLoreTabs.Add(child.transform.gameObject);
        }

        foreach (Transform child in factionOptions.transform.GetComponentInChildren<Transform>())
        {
            allLoreTabs.Add(child.transform.gameObject);
        }

        foreach (Transform child in locationOptions.transform.GetComponentInChildren<Transform>())
        {
            allLoreTabs.Add(child.transform.gameObject);
        }

        foreach (Transform child in botOptions.transform.GetComponentInChildren<Transform>())
        {
            allLoreTabs.Add(child.transform.gameObject);
        }

        factionOptions.SetActive(false);
        locationOptions.SetActive(false);
        botOptions.SetActive(false);
    }

    public void ItemRetrieve(Item.itemStruct targetItem) // check item that is recieved from the inventory manager to see if its name matches any of the lore tabs in the loreObjects list
    {

        //retrieves the information for any item sent from the inventory manager
        for(int i = 0; i < allLoreTabs.Count; i++)
        {
            if(allLoreTabs[i].name == targetItem.thisObject.name) //if the items name matches any of the lore entry names, create a copy and then parent it to that lore entry
            {
                var loreCopy = Instantiate(targetItem.thisObject as GameObject, allLoreTabs[i].transform);
                loreCopy.SetActive(true);
                allLoreTabs[i].GetComponent<OnLorePress>().TextUpdate(loreCopy.GetComponent<Item>().itemStats.description);

                break;
            }
        }
    }
}
