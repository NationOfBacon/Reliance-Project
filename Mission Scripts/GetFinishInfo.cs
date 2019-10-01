using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetFinishInfo : MonoBehaviour //attached to the finish UI in the level scene, fills out the stats on the finish screen
{
    public TextMeshProUGUI CreditCount;
    public TextMeshProUGUI datumCount;
    public GameObject itemsGrid;
    public GameObject itemText;

    public List<string> itemNames = new List<string>();
    private InventoryManager manager;

    private void Start()
    {
        manager = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();

        CreditCount.text = manager.tempRewards[0].ToString();
        datumCount.text = manager.tempRewards[1].ToString();

        for(int i = 0; i < itemNames.Count; i++)
        {
            GameObject tempItemText = Instantiate(itemText as GameObject, itemsGrid.transform);

            tempItemText.GetComponent<TextMeshProUGUI>().text = itemNames[i];
        }
    }

}
