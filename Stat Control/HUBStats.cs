 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUBStats : MonoBehaviour
{

    private int statCounter;

    public bool leadStats;
    public bool blueStats;
    public bool greenStats;
    public bool orangeStats;

    [System.Serializable]
    public struct statsGroup
    {
        public int playerLevel;
        public TextMeshProUGUI levelText;
        public int playerEXP;
        public TextMeshProUGUI expText;
        public Slider expSlider;
        public int pointsAvailable;
        public TextMeshProUGUI points;
        public int strength;
        public TextMeshProUGUI str;
        public int electronics;
        public TextMeshProUGUI elec;
        public int agility;
        public TextMeshProUGUI agi;
        public int combat;
        public TextMeshProUGUI cbt;
        public int scanning;
        public TextMeshProUGUI scan;
        public int efficiency;
        public TextMeshProUGUI eff;
        public int heavyHitter;
        public TextMeshProUGUI hit;
        public int retention;
        public TextMeshProUGUI ret;
        public int defender;
        public TextMeshProUGUI def;
        public int objMaster;
        public TextMeshProUGUI objM;
        public int regenerator;
        public TextMeshProUGUI regen;
        public int lucky;
        public TextMeshProUGUI luck;
        public int teamPlayer;
        public TextMeshProUGUI teamP;
        public int informant;
        public TextMeshProUGUI info;
        public TextMeshProUGUI specialization;

        public Perk perk1;
        public Perk perk2;
        public Perk perk3;
        public Perk perk4;
        public Perk perk5;
        public Perk perk6;

    };

    public statsGroup botStats = new statsGroup();

    private TextMeshProUGUI leadName;
    private TextMeshProUGUI blueName;
    private TextMeshProUGUI greenName;
    private TextMeshProUGUI orangeName;

    private HUBTracker hubTracker;
    private Inventory equipInv;
    private InventoryManager invManager;
    private ShowPerks showPerks;
    private bool retrievedStats = false;

    private void Awake()
    {
        hubTracker = GameObject.Find("Persistent Object").GetComponent<HUBTracker>();
        equipInv = GameObject.Find("HUBUI/Loadout Panel/Equipment Grid").GetComponent<Inventory>();
        invManager = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();
        showPerks = GameObject.Find("HUBUI/Loadout Panel/Perk Group").GetComponent<ShowPerks>();

        botStats.points = GameObject.Find("HUBUI/Loadout Panel/Points #").GetComponent<TextMeshProUGUI>();
        botStats.levelText = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Level #").GetComponent<TextMeshProUGUI>();
        botStats.expText = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Exp #").GetComponent<TextMeshProUGUI>();
        botStats.expSlider = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Exp Slider").GetComponent<Slider>();
        botStats.str = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (1)/Strength #").GetComponent<TextMeshProUGUI>();
        botStats.elec = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (1)/Electronics #").GetComponent<TextMeshProUGUI>();
        botStats.agi = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (1)/Agility #").GetComponent<TextMeshProUGUI>();
        botStats.cbt = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (1)/Combat #").GetComponent<TextMeshProUGUI>();
        botStats.scan = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (3)/Scanning #").GetComponent<TextMeshProUGUI>();
        botStats.eff = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (3)/Efficiency #").GetComponent<TextMeshProUGUI>();
        botStats.hit = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (3)/Heavy Hitter #").GetComponent<TextMeshProUGUI>();
        botStats.ret = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (3)/Retention #").GetComponent<TextMeshProUGUI>();
        botStats.def = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (5)/Defender #").GetComponent<TextMeshProUGUI>();
        botStats.objM = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (5)/Objective Master #").GetComponent<TextMeshProUGUI>();
        botStats.regen = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (5)/Regenerator #").GetComponent<TextMeshProUGUI>();
        botStats.luck = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (5)/Lucky #").GetComponent<TextMeshProUGUI>();
        botStats.teamP = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (5)/Team Player #").GetComponent<TextMeshProUGUI>();
        botStats.info = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Vertical Group (5)/Informant #").GetComponent<TextMeshProUGUI>();
        botStats.specialization = GameObject.Find("HUBUI/Loadout Panel/Stats Backing/Spec Button Backing/Specialization").GetComponent<TextMeshProUGUI>();

        leadName = GameObject.Find("HUBUI/Loadout Panel/Horizontal Group Names/Lead Bot Name").GetComponent<TextMeshProUGUI>();
        blueName = GameObject.Find("HUBUI/Loadout Panel/Horizontal Group Names/Blue Bot Name").GetComponent<TextMeshProUGUI>();
        greenName = GameObject.Find("HUBUI/Loadout Panel/Horizontal Group Names/Green Bot Name").GetComponent<TextMeshProUGUI>();
        orangeName = GameObject.Find("HUBUI/Loadout Panel/Horizontal Group Names/Orange Bot Name").GetComponent<TextMeshProUGUI>();

        leadStats = true;
        blueStats = false;
        greenStats = false;
        orangeStats = false;

        botStats.specialization.text = "No Specialization";

        leadName.color = new Color32(0, 200, 30, 255);
    }

    void Start()
    {
        if (leadStats)
            hubTracker.TransferStats(0);
        if (blueStats)
            hubTracker.TransferStats(1);
        if (greenStats)
            hubTracker.TransferStats(2);
        if (orangeStats)
            hubTracker.TransferStats(3);
    }

    void Update()
    {
        botStats.points.text = botStats.pointsAvailable.ToString();
        botStats.levelText.text = botStats.playerLevel.ToString();
        botStats.expText.text = botStats.playerEXP.ToString() + "/100";
        botStats.expSlider.value = botStats.playerEXP;

        botStats.str.text = botStats.strength.ToString();
        botStats.elec.text = botStats.electronics.ToString();
        botStats.agi.text = botStats.agility.ToString();
        botStats.cbt.text = botStats.combat.ToString();
        botStats.scan.text = botStats.scanning.ToString();
        botStats.eff.text = botStats.efficiency.ToString();
        botStats.hit.text = botStats.heavyHitter.ToString();
        botStats.ret.text = botStats.retention.ToString();
        botStats.def.text = botStats.defender.ToString();
        botStats.objM.text = botStats.objMaster.ToString();
        botStats.regen.text = botStats.regenerator.ToString();
        botStats.luck.text = botStats.lucky.ToString();
        botStats.teamP.text = botStats.teamPlayer.ToString();
        botStats.info.text = botStats.informant.ToString();

        if (botStats.pointsAvailable <= 0)
            botStats.pointsAvailable = 0;

        if(!retrievedStats)
        {
            if (leadStats)
                hubTracker.TransferStats(0);
            if (blueStats)
                hubTracker.TransferStats(1);
            if (greenStats)
                hubTracker.TransferStats(2);
            if (orangeStats)
                hubTracker.TransferStats(3);

            retrievedStats = true;
        }

    }

    public void UpdateStats() //called throughout the script when stats need to be updated for the hubTracker
    {
        if (leadStats)
            hubTracker.StatsUpdate(0, botStats);
        if (blueStats)
            hubTracker.StatsUpdate(1, botStats);
        if (greenStats)
            hubTracker.StatsUpdate(2, botStats);
        if (orangeStats)
            hubTracker.StatsUpdate(3, botStats);
    }
    
    public void ChangeBonuses(Slot tempTarget, bool polarity) //called when adding or removing stats from a gear item being applied to a bot
    {
        if(polarity == true)
        {
            botStats.strength += tempTarget.bonusStats[0];
            botStats.electronics += tempTarget.bonusStats[1];
            botStats.agility += tempTarget.bonusStats[2];
            botStats.combat += tempTarget.bonusStats[3];
            botStats.scanning += tempTarget.bonusStats[4];
            botStats.efficiency += tempTarget.bonusStats[5];
            botStats.heavyHitter += tempTarget.bonusStats[6];
            botStats.retention += tempTarget.bonusStats[7];
            botStats.defender += tempTarget.bonusStats[8];
            botStats.objMaster += tempTarget.bonusStats[9];
            botStats.regenerator += tempTarget.bonusStats[10];
            botStats.lucky += tempTarget.bonusStats[11];
            botStats.teamPlayer += tempTarget.bonusStats[12];
            botStats.informant += tempTarget.bonusStats[13];
        }
        else
        {
            botStats.strength -= tempTarget.bonusStats[0];
            botStats.electronics -= tempTarget.bonusStats[1];
            botStats.agility -= tempTarget.bonusStats[2];
            botStats.combat -= tempTarget.bonusStats[3];
            botStats.scanning -= tempTarget.bonusStats[4];
            botStats.efficiency -= tempTarget.bonusStats[5];
            botStats.heavyHitter -= tempTarget.bonusStats[6];
            botStats.retention -= tempTarget.bonusStats[7];
            botStats.defender -= tempTarget.bonusStats[8];
            botStats.objMaster -= tempTarget.bonusStats[9];
            botStats.regenerator -= tempTarget.bonusStats[10];
            botStats.lucky -= tempTarget.bonusStats[11];
            botStats.teamPlayer -= tempTarget.bonusStats[12];
            botStats.informant -= tempTarget.bonusStats[13];
        }

        UpdateStats();
    }

    public void StatsUpdate(HUBTracker.botStruct trackerBotStats) //recieves stats from the tracker and sets them for the display in the loadout screen
    {
        botStats.pointsAvailable = trackerBotStats.points;
        botStats.playerLevel = trackerBotStats.level;
        botStats.playerEXP = trackerBotStats.exp;
        botStats.strength = trackerBotStats.strength;
        botStats.electronics = trackerBotStats.electronics;
        botStats.agility = trackerBotStats.agility;
        botStats.combat = trackerBotStats.combat;
        botStats.scanning = trackerBotStats.scanning;
        botStats.efficiency = trackerBotStats.efficiency;
        botStats.heavyHitter = trackerBotStats.heavyHitter;
        botStats.retention = trackerBotStats.retention;
        botStats.defender = trackerBotStats.defender;
        botStats.objMaster = trackerBotStats.objMaster;
        botStats.regenerator = trackerBotStats.regenerator;
        botStats.lucky = trackerBotStats.lucky;
        botStats.teamPlayer = trackerBotStats.teamPlayer;
        botStats.informant = trackerBotStats.informant;
        botStats.specialization.text = trackerBotStats.spec;

        if (trackerBotStats.perk1 != null) //if there is a perk in this slot from the tracker, add it via showperks
        {
            showPerks.DisplayPerk(trackerBotStats.perk1); //add the perk to showperks
            botStats.perk1 = trackerBotStats.perk1; //set the info from the tracker perk equal to the info for botStats
        }
        else
        {
            botStats.perk1 = null; //if there is no perk, set it to null
        }

        if (trackerBotStats.perk2 != null)
        {
            showPerks.DisplayPerk(trackerBotStats.perk2);
            botStats.perk2 = trackerBotStats.perk2;
        }
        else
        {
            botStats.perk2 = null;
        }

        if (trackerBotStats.perk3 != null)
        {
            showPerks.DisplayPerk(trackerBotStats.perk3);
            botStats.perk3 = trackerBotStats.perk3;
        }
        else
        {
            botStats.perk3 = null;
        }

        if (trackerBotStats.perk4 != null)
        {
            showPerks.DisplayPerk(trackerBotStats.perk4);
            botStats.perk4 = trackerBotStats.perk4;
        }
        else
        {
            botStats.perk4 = null;
        }

        if (trackerBotStats.perk5 != null)
        {
            showPerks.DisplayPerk(trackerBotStats.perk5);
            botStats.perk5 = trackerBotStats.perk5;
        }
        else
        {
            botStats.perk5 = null;
        }

        if (trackerBotStats.perk6 != null)
        {
            showPerks.DisplayPerk(trackerBotStats.perk6);
            botStats.perk6 = trackerBotStats.perk6;
        }
        else
        {
            botStats.perk6 = null;
        }

        showPerks.ClearPerks();
    }

    public void SpecUpdate(string spec)
    {
        botStats.specialization.text = spec;

        UpdateStats();
    }

    public void BotSelect(int botID)
    {
        if (botID == 0)
        {
            leadStats = true;
            blueStats = false;
            greenStats = false;
            orangeStats = false;
            leadName.color = new Color32(0, 200, 30, 255);
            blueName.color = new Color32(0, 134, 20, 255);
            greenName.color = new Color32(0, 134, 20, 255);
            orangeName.color = new Color32(0, 134, 20, 255);
        }
        else if(botID == 1)
        {
            leadStats = false;
            blueStats = true;
            greenStats = false;
            orangeStats = false;
            leadName.color = new Color32(0, 134, 20, 255);
            blueName.color = new Color32(0, 200, 30, 255);
            greenName.color = new Color32(0, 134, 20, 255);
            orangeName.color = new Color32(0, 134, 20, 255);
        }
        else if (botID == 2)
        {
            leadStats = false;
            blueStats = false;
            greenStats = true;
            orangeStats = false;
            leadName.color = new Color32(0, 134, 20, 255);
            blueName.color = new Color32(0, 134, 20, 255);
            greenName.color = new Color32(0, 200, 30, 255);
            orangeName.color = new Color32(0, 134, 20, 255);
        }
        else if (botID == 3)
        {
            leadStats = false;
            blueStats = false;
            greenStats = false;
            orangeStats = true;
            leadName.color = new Color32(0, 134, 20, 255);
            blueName.color = new Color32(0, 134, 20, 255);
            greenName.color = new Color32(0, 134, 20, 255);
            orangeName.color = new Color32(0, 200, 30, 255);
        }

        invManager.UpdateEquipment();
        retrievedStats = false;
    }

    public void statIncrement(GameObject targetStatObject)
    {
        //general use method to replace all below methods
        if(botStats.pointsAvailable >= 1)
        {
            if (targetStatObject.name.Contains("Str"))
               botStats.strength++;

            if (targetStatObject.name.Contains("Elec"))
               botStats.electronics++;

            if (targetStatObject.name.Contains("Agi"))
               botStats.agility++;

            if (targetStatObject.name.Contains("Com"))
               botStats.combat++;

            if (targetStatObject.name.Contains("Scan"))
               botStats.scanning++;

            if (targetStatObject.name.Contains("Eff"))
               botStats.efficiency++;

            if (targetStatObject.name.Contains("Heavy"))
               botStats.heavyHitter++;

            if (targetStatObject.name.Contains("Ret"))
               botStats.retention++;

            if (targetStatObject.name.Contains("Def"))
               botStats.defender++;

            if (targetStatObject.name.Contains("Obj"))
               botStats.objMaster++;

            if (targetStatObject.name.Contains("Regen"))
               botStats.regenerator++;

            if (targetStatObject.name.Contains("Luck"))
               botStats.lucky++;

            if (targetStatObject.name.Contains("Team P"))
               botStats.teamPlayer++;

            if (targetStatObject.name.Contains("Info"))
               botStats.informant++;

            botStats.pointsAvailable--;

            UpdateStats();
        }
    }
}
