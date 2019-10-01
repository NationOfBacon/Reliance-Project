using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class ItemPool : MonoBehaviour //this script will hold all different objects that are available to be purchased or picked up in missions.
{
    public List<GameObject> armoryObjects = new List<GameObject>();
    public List<GameObject> junkObjects = new List<GameObject>();
    public List<GameObject> weaponsObjects = new List<GameObject>();
    public List<GameObject> specialObjects = new List<GameObject>();
    public GameObject itemPrefab;
    private GameObject armoryPanel;
    private GameObject junkPanel;
    private GameObject weaponsPanel;
    private GameObject specialPanel;

    private void Awake()
    {
        armoryPanel = GameObject.Find("HUBUI/Inventory Panel/Shop Panels/Armory Shop");
        junkPanel = GameObject.Find("HUBUI/Inventory Panel/Shop Panels/Junk Shop");
        weaponsPanel = GameObject.Find("HUBUI/Inventory Panel/Shop Panels/Weapons Shop");
        specialPanel = GameObject.Find("HUBUI/Inventory Panel/Shop Panels/Specialty Shop");
    }

    void Start()
    {
        GenerateShopLists();
        Debug.Log("Creating shop contents");
    }

    public void GenerateShopLists()
    {
        for(int y = 0; y < 4; y++) //for each shop
        {
            int itemNumber = Random.Range(0, 3);
            GameObject targetPanel = null;
            List<GameObject> targetList = new List<GameObject>();

            //use the y value to determine what shop is being setup for the current loop
            if (y == 0)
            {
                targetPanel = armoryPanel;
                targetList = armoryObjects;
            }
            else if (y == 1)
            {
                targetPanel = junkPanel;
                targetList = junkObjects;
            }
            else if (y == 2)
            {
                targetPanel = weaponsPanel;
                targetList = weaponsObjects;
            }
            else if (y == 3)
            {
                targetPanel = specialPanel;
                targetList = specialObjects;
            }

            List<GameObject> tempItemObjects = new List<GameObject>();
            List<Item> tempItems = new List<Item>();

            if (itemNumber == 0)
                Debug.Log("item number was 0 for the " + targetPanel.name + " so no items were spawned");

            for (int i = 0; i < itemNumber; i++) //use a random number to decide how many items should appear
            {
                int randItem = Random.Range(0, targetList.Count); //use a random variable to decide what item is being spawned from the list

                var currentPanel = Instantiate(itemPrefab as GameObject, targetPanel.transform, false); //use the target panel variable to give the transform for the current shop
                currentPanel.GetComponent<ItemBuy>().attachedItem = Instantiate(targetList[randItem] as GameObject, targetPanel.transform, false);

                foreach (Transform tempChild in targetPanel.GetComponentsInChildren<Transform>()) //get the children objects under the panel
                {
                    if (tempChild.name.Contains("Panel"))
                    {
                        tempItemObjects.Add(tempChild.gameObject);
                        tempItemObjects = tempItemObjects.Distinct().ToList();
                    }


                    if (tempChild.GetComponent<Item>())
                    {
                        tempItems.Add(tempChild.GetComponent<Item>());
                        tempItems = tempItems.Distinct().ToList();
                    }

                }

                string tempName = "";

                foreach (Transform childVar in tempItemObjects[i].GetComponentsInChildren<Transform>()) //for each child of the current panel prefab, set each text field
                {

                    if (childVar.name == "Item Name Text") // set the name of the prefab to the itemname field of the item
                    {
                        childVar.GetComponent<TextMeshProUGUI>().text = targetList[randItem].GetComponent<Item>().itemStats.displayName;

                        tempName = childVar.GetComponent<TextMeshProUGUI>().text;

                        if (childVar.GetComponent<TextMeshProUGUI>().text.Contains("Super"))
                        {
                            childVar.GetComponent<TextMeshProUGUI>().color = Color.yellow;
                        }

                        if (childVar.GetComponent<TextMeshProUGUI>().text.Contains("Altered"))
                        {
                            childVar.GetComponent<TextMeshProUGUI>().color = new Color(0, 229, 202, 255);
                        }
                    }

                    if(childVar.name == "Item Image")
                    {
                        childVar.GetComponent<Image>().sprite = targetList[randItem].GetComponent<Item>().itemStats.icon;
                    }

                    if (childVar.name == "Item Cost") //set the cost of the prefab using the random number
                    {
                        childVar.GetComponent<TextMeshProUGUI>().text = targetList[randItem].GetComponent<Item>().itemStats.buyPrice.ToString();
                    }

                    if(childVar.name == "Item Currency Type")
                    {
                        if(tempName.Contains("Altered"))
                        {
                            childVar.GetComponent<TextMeshProUGUI>().text = "Datum";
                        }
                        else
                        {
                            childVar.GetComponent<TextMeshProUGUI>().text = "Credits";
                        }
                    }
                }

                tempName = "";
            }
        }
    }
}
