using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpecSelect : MonoBehaviour
{
    private int currentLevel;
    private TextMeshProUGUI specText;
    private string specTitle;
    private GameObject specChoicePanel;
    private GameObject specSkillTreePanel;
    private GameObject skillPanelChild;
    private int specRequirement = 5;
    private bool specSet;
    public Sprite tempImage;

    private List<GameObject> perkBlocks = new List<GameObject>();

    private HUBStats hubStats;

    // Start is called before the first frame update
    void Start()
    {
        specChoicePanel = GameObject.Find("HUBUI/Loadout Panel/Spec Panel");
        specSkillTreePanel = GameObject.Find("HUBUI/Loadout Panel/Skill Tree Panel");
        skillPanelChild = GameObject.Find("HUBUI/Loadout Panel/Skill Tree Panel/Spec Title");
        hubStats = GameObject.Find("HUBUI/Loadout Panel/Stats Backing").GetComponent<HUBStats>();
        specText = GetComponent<TextMeshProUGUI>();
        specText.text = "No Specialization";
        specSet = false;

        foreach(Transform block in skillPanelChild.GetComponentsInChildren<Transform>())
        {
            if(block.name.Contains("Block"))
            {
                perkBlocks.Add(block.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentLevel = int.Parse(GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Level #").GetComponent<TextMeshProUGUI>().text);

        if (currentLevel < specRequirement)
        {
            specSet = false;
        }

        if(hubStats.botStats.specialization.text == "Medic" || hubStats.botStats.specialization.text == "Heavy" || hubStats.botStats.specialization.text == "Support" || hubStats.botStats.specialization.text == "Sniper")
        {
            specSet = true;
        }

        if(hubStats.botStats.specialization.text == "No Specialization")
        {
            specSet = false;
        }
    }

    public void CheckSpecState()
    {
        if(currentLevel >= specRequirement && specSet == false)
        {
            specChoicePanel.SetActive(true);
        }

        if(specSet == true)
        {
            specSkillTreePanel.SetActive(true);
        }
    }

    private void ShowSpecs() //once a bot has selected a spec, this method will run to display the perks specific to that spec
    {
        if(specTitle == "Medic")
        {
            //get a reference to the perk script on each block and update the info to fit the spec
            Perk perk1 = perkBlocks[0].GetComponent<Perk>();
            perk1.stats.perkName = "Fast Heal";
            perk1.stats.perkDesc = "Enhances medics ability to heal at higher rates.";
            perk1.stats.perkImage = tempImage;
            perk1.stats.partyHealingBuff = 2;

            Perk perk2 = perkBlocks[1].GetComponent<Perk>();
            perk2.stats.perkName = "Self Sustaining";
            perk2.stats.perkDesc = "Allows the medic to heal passively.";
            perk2.stats.perkImage = tempImage;
            perk2.stats.selfHealingBuff = 2;

        }

        if (specTitle == "Support")
        {
            Perk perk1 = perkBlocks[0].GetComponent<Perk>();
            perk1.stats.perkName = "Ranged Assistance";
            perk1.stats.perkDesc = "Allows for ranged AOE grenade blasts that buff allies.";
            perk1.stats.perkImage = tempImage;

            Perk perk2 = perkBlocks[1].GetComponent<Perk>();
            perk2.stats.perkName = "Enhanced Aim";
            perk2.stats.perkDesc = "When partnered with another bot, increase the groups firing accuracy.";
            perk2.stats.perkImage = tempImage;
            perk2.stats.accuracyBuff = 2;
        }

        if (specTitle == "Heavy")
        {
            Perk perk1 = perkBlocks[0].GetComponent<Perk>();
            perk1.stats.perkName = "Suppression";
            perk1.stats.perkDesc = "Increases rate of fire for 5 seconds and provides an effect that holds enemies in place";
            perk1.stats.perkImage = tempImage;
            perk1.stats.fireRateBuff = 2;

            Perk perk2 = perkBlocks[1].GetComponent<Perk>();
            perk2.stats.perkName = "Armor Up";
            perk2.stats.perkDesc = "Increases health by 30% and slows movement by 50%. Lasts 20 seconds.";
            perk2.stats.perkImage = tempImage;
            perk2.stats.healthBuff = 2;
            perk2.stats.speedBuff = -2;
        }

        if (specTitle == "Sniper")
        {
            Perk perk1 = perkBlocks[0].GetComponent<Perk>();
            perk1.stats.perkName = "Lock-Down";
            perk1.stats.perkDesc = "Removes ability to move while increasing damage output and range. Rate of fire is slower while in this mode.";
            perk1.stats.perkImage = tempImage;
            perk1.stats.damageBuff = 2;
            perk1.stats.rangeBuff = 2;
            perk1.stats.fireRateBuff = -2;

            Perk perk2 = perkBlocks[1].GetComponent<Perk>();
            perk2.stats.perkName = "Team Targeting";
            perk2.stats.perkDesc = "When an ally is being attacked by enemies that are in sight of the sniper they can engage at any range";
            perk2.stats.perkImage = tempImage;
        }
    }

    public void SetMedicSpec()
    {
        specTitle = "Medic";
        hubStats.SpecUpdate(specTitle);
        specSet = true;
        ShowSpecs();

        for(int i = 0; i < perkBlocks.Count; i++)
        {
            perkBlocks[i].GetComponent<Perk>().UpdateImage();
        }
    }

    public void SetSupportSpec()
    {
        specTitle = "Support";
        hubStats.SpecUpdate(specTitle);
        specSet = true;
        ShowSpecs();

        for (int i = 0; i < perkBlocks.Count; i++)
        {
            perkBlocks[i].GetComponent<Perk>().UpdateImage();
        }
    }

    public void SetHeavySpec()
    {
        specTitle = "Heavy";
        hubStats.SpecUpdate(specTitle);
        specSet = true;
        ShowSpecs();

        for (int i = 0; i < perkBlocks.Count; i++)
        {
            perkBlocks[i].GetComponent<Perk>().UpdateImage();
        }
    }

    public void SetSniperSpec()
    {
        specTitle = "Sniper";
        hubStats.SpecUpdate(specTitle);
        specSet = true;
        ShowSpecs();

        for (int i = 0; i < perkBlocks.Count; i++)
        {
            perkBlocks[i].GetComponent<Perk>().UpdateImage();
        }
    }
}
