using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    private InventoryManager invManager;
    private DisplayLog logD;
    private GetFinishInfo finishI;

    private void Awake()
    {
        finishI = GameObject.Find("FinishUI").GetComponent<GetFinishInfo>();
    }

    private void Start()
    {
        invManager = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();
        logD = GameObject.Find("RunningUI/Text Log/Log Panel/Content").GetComponent<DisplayLog>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            GameObject itemPickedUp = collision.gameObject;
            logD.RecieveLog(itemPickedUp.GetComponent<Item>().itemStats.displayName + " has been picked up", Color.yellow);
            finishI.itemNames.Add(itemPickedUp.GetComponent<Item>().itemStats.displayName);
            itemPickedUp.transform.parent = invManager.gameObject.transform;
            GetComponent<AudioSource>().PlayOneShot(GetComponent<MultipleAudioClips>().clips[2]);

            if (itemPickedUp.GetComponent<Item>().itemStats.type == "Lore")
            {
                invManager.loreObjects.Add(itemPickedUp);
                invManager.loreObjects = invManager.loreObjects.Distinct().ToList();
            }
            else
            {
                invManager.allPickedUpObjects.Add(itemPickedUp);
                invManager.OnItemMove(itemPickedUp.gameObject, true);
            }

            itemPickedUp.SetActive(false);
        }
    }
}
