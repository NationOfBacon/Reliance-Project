using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPerks : MonoBehaviour
{
    private HUBStats hubStats;

    private List<GameObject> children = new List<GameObject>();

    private void Awake()
    {
        hubStats = GameObject.Find("HUBUI/Loadout Panel/Stats Backing").GetComponent<HUBStats>();

        foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            var imageObject = child.gameObject.GetComponent<Image>();
            children.Add(child.gameObject);

            if (imageObject.sprite != null)
            {
                child.gameObject.SetActive(true);
            }
            else
                child.gameObject.SetActive(false);
        }
    }

    public void DisplayPerk(Perk targetPerk) //used in the hubStats updatestats method to display current perks of the selected bot
    {
        foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            var imageObject = child.gameObject.GetComponent<Image>();

            if (child.gameObject.GetComponent<Perk>().stats.perkName == targetPerk.stats.perkName) //if this perk has already been added, do nothing
            {
                break;
            }
            else if (imageObject.sprite == null) //if the image component does not have a sprite, transfer all stats from the targetperk
            {
                var currentPerk = child.gameObject.GetComponent<Perk>(); //get a reference to the perk on the current child object

                currentPerk.stats = targetPerk.stats; //transfer stats
                imageObject.sprite = currentPerk.stats.perkImage; //set the image

                child.gameObject.SetActive(true);
            }
        }
    }

    public void AddPerk(Perk targetPerk) //used for buttons on the perk trees so that users can add perks to a bot
    {
        foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            var imageObject = child.gameObject.GetComponent<Image>();

            if (child.gameObject.GetComponent<Perk>().stats.perkName == targetPerk.stats.perkName) //if this perk has already been added, do nothing
            {
                break;
            }
            else if (imageObject.sprite == null) //if the image component does not have a sprite, transfer all stats from the targetperk
            {
                var currentPerk = child.gameObject.GetComponent<Perk>(); //get a reference to the perk on the current child object

                currentPerk.stats = targetPerk.stats; //transfer stats
                imageObject.sprite = currentPerk.stats.perkImage; //set the image

                child.gameObject.SetActive(true);

                if (hubStats.botStats.perk1 == null) //if the hubstats perk is not there
                {
                    var perkObject = Instantiate(child.gameObject as GameObject, hubStats.transform); //create a copy of the gameobject that the perk is located on
                    perkObject.SetActive(false);
                    hubStats.botStats.perk1 = perkObject.GetComponent<Perk>(); //set the botStats perk reference equal to the instantiated gameobjects perk script

                    break;
                }
                else
                {
                    //hubStats perk is there
                    Debug.Log("hubStats perk1 is not null");
                }



                if (hubStats.botStats.perk2 == null) //if the hubstats perk is not there
                {
                    var perkObject = Instantiate(child.gameObject as GameObject, hubStats.transform); //create a copy of the gameobject that the perk is located on
                    perkObject.SetActive(false);
                    hubStats.botStats.perk2 = perkObject.GetComponent<Perk>(); //set the botStats perk reference equal to the instantiated gameobjects perk script

                    break;
                }
                else
                {
                    //hubStats perk is there
                    Debug.Log("hubStats perk2 is not null");
                }


                if (hubStats.botStats.perk3 == null) //if the hubstats perk is not there
                {
                    var perkObject = Instantiate(child.gameObject as GameObject, hubStats.transform); //create a copy of the gameobject that the perk is located on
                    perkObject.SetActive(false);
                    hubStats.botStats.perk3 = perkObject.GetComponent<Perk>(); //set the botStats perk reference equal to the instantiated gameobjects perk script

                    break;
                }
                else
                {
                    //hubStats perk is there
                    Debug.Log("hubStats perk3 is not null");
                }

                if (hubStats.botStats.perk4 == null) //if the hubstats perk is not there
                {
                    var perkObject = Instantiate(child.gameObject as GameObject, hubStats.transform); //create a copy of the gameobject that the perk is located on
                    perkObject.SetActive(false);
                    hubStats.botStats.perk4 = perkObject.GetComponent<Perk>(); //set the botStats perk reference equal to the instantiated gameobjects perk script

                    break;
                }
                else
                {
                    //hubStats perk is there
                    Debug.Log("hubStats perk4 is not null");
                }

                if (hubStats.botStats.perk5 == null) //if the hubstats perk is not there
                {
                    var perkObject = Instantiate(child.gameObject as GameObject, hubStats.transform); //create a copy of the gameobject that the perk is located on
                    perkObject.SetActive(false);
                    hubStats.botStats.perk5 = perkObject.GetComponent<Perk>(); //set the botStats perk reference equal to the instantiated gameobjects perk script

                    break;
                }
                else
                {
                    //hubStats perk is there
                    Debug.Log("hubStats perk5 is not null");
                }

                if (hubStats.botStats.perk6 == null) //if the hubstats perk is not there
                {
                    var perkObject = Instantiate(child.gameObject as GameObject, hubStats.transform); //create a copy of the gameobject that the perk is located on
                    perkObject.SetActive(false);
                    hubStats.botStats.perk6 = perkObject.GetComponent<Perk>(); //set the botStats perk reference equal to the instantiated gameobjects perk script

                    break;
                }
                else
                {
                    //hubStats perk is there
                    Debug.Log("hubStats perk6 is not null");
                }

                break;
            }
        }
        hubStats.UpdateStats(); //called to send the perks to the hubTracker
    }

    public void ClearPerks() //called from hubstats when stats are updated. Clears perks if the hub does not have a perk in the slot
    {
        if (hubStats.botStats.perk1 == null)
        {
            children[0].GetComponent<Perk>().stats.perkName = null;
            children[0].GetComponent<Perk>().stats.perkDesc = null;
            children[0].GetComponent<Perk>().stats.perkImage = null;
            children[0].GetComponent<Perk>().stats.damageBuff = 0;
            children[0].GetComponent<Perk>().stats.healthBuff = 0;
            children[0].GetComponent<Perk>().stats.rangeBuff = 0;
            children[0].GetComponent<Perk>().stats.accuracyBuff = 0;
            children[0].GetComponent<Perk>().stats.selfHealingBuff = 0;
            children[0].GetComponent<Perk>().stats.speedBuff = 0;
            children[0].GetComponent<Perk>().stats.fireRateBuff = 0;
            children[0].GetComponent<Perk>().stats.stationaryDamageBuff = 0;
            children[0].GetComponent<Perk>().stats.auraSizeBuff = 0;
            children[0].GetComponent<Perk>().stats.throwDistanceBuff = 0;
            children[0].GetComponent<Perk>().stats.partyHealingBuff = 0;

            children[0].GetComponent<Image>().sprite = null;
        }

        if (hubStats.botStats.perk2 == null)
        {
            children[1].GetComponent<Perk>().stats.perkName = null;
            children[1].GetComponent<Perk>().stats.perkDesc = null;
            children[1].GetComponent<Perk>().stats.perkImage = null;
            children[1].GetComponent<Perk>().stats.damageBuff = 0;
            children[1].GetComponent<Perk>().stats.healthBuff = 0;
            children[1].GetComponent<Perk>().stats.rangeBuff = 0;
            children[1].GetComponent<Perk>().stats.accuracyBuff = 0;
            children[1].GetComponent<Perk>().stats.selfHealingBuff = 0;
            children[1].GetComponent<Perk>().stats.speedBuff = 0;
            children[1].GetComponent<Perk>().stats.fireRateBuff = 0;
            children[1].GetComponent<Perk>().stats.stationaryDamageBuff = 0;
            children[1].GetComponent<Perk>().stats.auraSizeBuff = 0;
            children[1].GetComponent<Perk>().stats.throwDistanceBuff = 0;
            children[1].GetComponent<Perk>().stats.partyHealingBuff = 0;
                     
            children[1].GetComponent<Image>().sprite = null;
        }

        if (hubStats.botStats.perk3 == null)
        {
            children[2].GetComponent<Perk>().stats.perkName = null;
            children[2].GetComponent<Perk>().stats.perkDesc = null;
            children[2].GetComponent<Perk>().stats.perkImage = null;
            children[2].GetComponent<Perk>().stats.damageBuff = 0;
            children[2].GetComponent<Perk>().stats.healthBuff = 0;
            children[2].GetComponent<Perk>().stats.rangeBuff = 0;
            children[2].GetComponent<Perk>().stats.accuracyBuff = 0;
            children[2].GetComponent<Perk>().stats.selfHealingBuff = 0;
            children[2].GetComponent<Perk>().stats.speedBuff = 0;
            children[2].GetComponent<Perk>().stats.fireRateBuff = 0;
            children[2].GetComponent<Perk>().stats.stationaryDamageBuff = 0;
            children[2].GetComponent<Perk>().stats.auraSizeBuff = 0;
            children[2].GetComponent<Perk>().stats.throwDistanceBuff = 0;
            children[2].GetComponent<Perk>().stats.partyHealingBuff = 0;
                     
            children[2].GetComponent<Image>().sprite = null;
        }

        if (hubStats.botStats.perk4 == null)
        {
            children[3].GetComponent<Perk>().stats.perkName = null;
            children[3].GetComponent<Perk>().stats.perkDesc = null;
            children[3].GetComponent<Perk>().stats.perkImage = null;
            children[3].GetComponent<Perk>().stats.damageBuff = 0;
            children[3].GetComponent<Perk>().stats.healthBuff = 0;
            children[3].GetComponent<Perk>().stats.rangeBuff = 0;
            children[3].GetComponent<Perk>().stats.accuracyBuff = 0;
            children[3].GetComponent<Perk>().stats.selfHealingBuff = 0;
            children[3].GetComponent<Perk>().stats.speedBuff = 0;
            children[3].GetComponent<Perk>().stats.fireRateBuff = 0;
            children[3].GetComponent<Perk>().stats.stationaryDamageBuff = 0;
            children[3].GetComponent<Perk>().stats.auraSizeBuff = 0;
            children[3].GetComponent<Perk>().stats.throwDistanceBuff = 0;
            children[3].GetComponent<Perk>().stats.partyHealingBuff = 0;
                     
            children[3].GetComponent<Image>().sprite = null;
        }

        if (hubStats.botStats.perk5 == null)
        {
            children[4].GetComponent<Perk>().stats.perkName = null;
            children[4].GetComponent<Perk>().stats.perkDesc = null;
            children[4].GetComponent<Perk>().stats.perkImage = null;
            children[4].GetComponent<Perk>().stats.damageBuff = 0;
            children[4].GetComponent<Perk>().stats.healthBuff = 0;
            children[4].GetComponent<Perk>().stats.rangeBuff = 0;
            children[4].GetComponent<Perk>().stats.accuracyBuff = 0;
            children[4].GetComponent<Perk>().stats.selfHealingBuff = 0;
            children[4].GetComponent<Perk>().stats.speedBuff = 0;
            children[4].GetComponent<Perk>().stats.fireRateBuff = 0;
            children[4].GetComponent<Perk>().stats.stationaryDamageBuff = 0;
            children[4].GetComponent<Perk>().stats.auraSizeBuff = 0;
            children[4].GetComponent<Perk>().stats.throwDistanceBuff = 0;
            children[4].GetComponent<Perk>().stats.partyHealingBuff = 0;
                     
            children[4].GetComponent<Image>().sprite = null;
        }

        if (hubStats.botStats.perk6 == null)
        {
            children[5].GetComponent<Perk>().stats.perkName = null;
            children[5].GetComponent<Perk>().stats.perkDesc = null;
            children[5].GetComponent<Perk>().stats.perkImage = null;
            children[5].GetComponent<Perk>().stats.damageBuff = 0;
            children[5].GetComponent<Perk>().stats.healthBuff = 0;
            children[5].GetComponent<Perk>().stats.rangeBuff = 0;
            children[5].GetComponent<Perk>().stats.accuracyBuff = 0;
            children[5].GetComponent<Perk>().stats.selfHealingBuff = 0;
            children[5].GetComponent<Perk>().stats.speedBuff = 0;
            children[5].GetComponent<Perk>().stats.fireRateBuff = 0;
            children[5].GetComponent<Perk>().stats.stationaryDamageBuff = 0;
            children[5].GetComponent<Perk>().stats.auraSizeBuff = 0;
            children[5].GetComponent<Perk>().stats.throwDistanceBuff = 0;
            children[5].GetComponent<Perk>().stats.partyHealingBuff = 0;
                     
            children[5].GetComponent<Image>().sprite = null;
        }
    }
}