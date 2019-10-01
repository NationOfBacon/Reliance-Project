using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    public List<GameObject> itemsList = new List<GameObject>(); //holds all gear items that can be spawned into a level
    public List<GameObject> loreItemsList = new List<GameObject>(); //holds all lore items that can be spawned into a level
    private List<GameObject> itemSpawnPoints = new List<GameObject>(); //holds all spawn points in the level

    private void Awake()
    {
        foreach(GameObject spawn in GameObject.FindGameObjectsWithTag("ItemSpawn"))
        {
            itemSpawnPoints.Add(spawn);
        }

        int randGear = Random.Range(0, 6); //random var used to determine how many items will spawn in
        int i = 0;

        while (i < randGear)
        {
            int randPos = Random.Range(0, itemSpawnPoints.Count); //random var used to chose a spawn out of the spawns list
            int randSelect = Random.Range(0, 1); //random var used to determine if the item spawned will be a gear or lore item (0 is gear, 1 is lore)

            if (!itemSpawnPoints[randPos].GetComponentInChildren<Item>()) //check if the spawn point already has an item as a child of it
            {
                if (randSelect == 1)
                {
                    int randItem = Random.Range(0, loreItemsList.Count); //random var used to choose a lore item out of the lore list

                    Instantiate(loreItemsList[randItem], itemSpawnPoints[randPos].transform);
                }
                else
                {
                    int randItem = Random.Range(0, itemsList.Count); //random var used to choose a lore item out of the gear list

                    Instantiate(itemsList[randItem], itemSpawnPoints[randPos].transform);
                }

                i++;
            }
            else
            {
                Debug.Log("The item spawn selected already has something there");
            }
        }

        Debug.Log("Item Spawning Complete");
    }
}
