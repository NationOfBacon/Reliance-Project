using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUBTracker : MonoBehaviour
{
    [System.Serializable]
    public struct botStruct
    {
        public BotStats bot;
        public int points;
        public int level;
        public int exp;
        public int strength;
        public int electronics;
        public int agility;
        public int combat;
        public int scanning;
        public int efficiency;
        public int heavyHitter;
        public int retention;
        public int defender;
        public int objMaster;
        public int regenerator;
        public int lucky;
        public int teamPlayer;
        public int informant;
        public string spec;

        public Perk perk1;
        public Perk perk2;
        public Perk perk3;
        public Perk perk4;
        public Perk perk5;
        public Perk perk6;
    };

    public botStruct leadBot = new botStruct();
    public botStruct blueBot = new botStruct();
    public botStruct greenBot = new botStruct();
    public botStruct orangeBot = new botStruct();

    public List<botStruct> allBots = new List<botStruct>();

    private HUBStats hubStats;
    private InventoryManager invMgr;

    private void Awake()
    {
        invMgr = GetComponent<InventoryManager>();
    }

    void Start()
    {
        leadBot.level = 1;
        blueBot.level = 1;
        greenBot.level = 1;
        orangeBot.level = 1;
        leadBot.points = 10;
        blueBot.points = 10;
        greenBot.points = 10;
        orangeBot.points = 10;

        leadBot.spec = "No Specialization";
        blueBot.spec = "No Specialization";
        greenBot.spec = "No Specialization";
        orangeBot.spec = "No Specialization";
        allBots.Add(leadBot);
        allBots.Add(blueBot);
        allBots.Add(greenBot);
        allBots.Add(orangeBot);
    }

    // Update is called once per frame
    void Update()
    {
        if (leadBot.exp > 99)
        {
            leadBot.level += 1;
            leadBot.points += 2;
            leadBot.exp -= 100;
        }

        if (blueBot.exp > 99)
        {
            blueBot.level += 1;
            blueBot.points += 2;
            blueBot.exp -= 100;
        }

        if (greenBot.exp > 99)
        {
            greenBot.level += 1;
            greenBot.points += 2;
            greenBot.exp -= 100;
        }

        if (orangeBot.exp > 99)
        {
            orangeBot.level += 1;
            orangeBot.points += 2;
            orangeBot.exp -= 100;
        }

        allBots[0] = leadBot;
        allBots[1] = blueBot;
        allBots[2] = greenBot;
        allBots[3] = orangeBot;
    }

    public void LoadDataFromFile(string fileName)
    {
        PlayerData data = SaveSystem.LoadData(fileName);

        var currentBot = new botStruct();
        var currentData = new PlayerData.botData();

        for(int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                currentBot = leadBot;
                currentData = data.leadBot;
            }
            else if (i == 1)
            {
                currentBot = blueBot;
                currentData = data.blueBot;
            }
            else if (i == 2)
            {
                currentBot = greenBot;
                currentData = data.greenBot;
            }
            else if (i == 3)
            {
                currentBot = orangeBot;
                currentData = data.orangeBot;
            }

            currentBot.points = currentData.points;
            currentBot.level = currentData.level;
            currentBot.exp = currentData.exp;
            currentBot.strength = currentData.strength;
            currentBot.electronics = currentData.electronics;
            currentBot.agility = currentData.agility;
            currentBot.combat = currentData.combat;
            currentBot.scanning = currentData.scanning;
            currentBot.efficiency = currentData.efficiency;
            currentBot.heavyHitter = currentData.heavyHitter;
            currentBot.retention = currentData.retention;
            currentBot.defender = currentData.defender;
            currentBot.objMaster = currentData.objMaster;
            currentBot.regenerator = currentData.regenerator;
            currentBot.lucky = currentData.lucky;
            currentBot.teamPlayer = currentData.teamPlayer;
            currentBot.informant = currentData.informant;
            currentBot.spec = currentData.spec;


            if (i == 0)
                leadBot = currentBot;
            else if (i == 1)
                blueBot = currentBot;
            else if (i == 2)
                greenBot = currentBot;
            else if (i == 3)
                orangeBot = currentBot;
        }
    }

    public void ClearAllSaveData()
    {
        var currentBot = new botStruct();

        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
                currentBot = leadBot;
            else if (i == 1)
                currentBot = blueBot;
            else if (i == 2)
                currentBot = greenBot;
            else if (i == 3)
                currentBot = orangeBot;

            currentBot.points = 10;
            currentBot.level = 1;
            currentBot.exp = 0;
            currentBot.strength = 0;
            currentBot.electronics = 0;
            currentBot.agility = 0;
            currentBot.combat = 0;
            currentBot.scanning = 0;
            currentBot.efficiency = 0;
            currentBot.heavyHitter = 0;
            currentBot.retention = 0;
            currentBot.defender = 0;
            currentBot.objMaster = 0;
            currentBot.regenerator = 0;
            currentBot.lucky = 0;
            currentBot.teamPlayer = 0;
            currentBot.informant = 0;
            currentBot.spec = "No Specialization";


            if (i == 0)
                leadBot = currentBot;
            else if (i == 1)
                blueBot = currentBot;
            else if (i == 2)
                greenBot = currentBot;
            else if (i == 3)
                orangeBot = currentBot;
        }
    }

    public void OnLevelLoad(Scene scene) //called from the game manager to send stats to the bots when a level is loaded
    {
        if (scene.name == "HUB scene")
        {
            hubStats = GameObject.Find("HUBUI/Loadout Panel/Stats Backing").GetComponent<HUBStats>();
        }

        if (scene.name != "HUB scene" && scene.name != "Main Menu")
        {
            leadBot.bot = GameObject.Find("PlayerSphere").GetComponent<BotStats>();
            blueBot.bot = GameObject.Find("AISphere Blue").GetComponent<BotStats>();
            greenBot.bot = GameObject.Find("AISphere Green").GetComponent<BotStats>();
            orangeBot.bot = GameObject.Find("AISphere Orange").GetComponent<BotStats>();

            leadBot.bot.StatUpdate(leadBot);
            blueBot.bot.StatUpdate(blueBot);
            greenBot.bot.StatUpdate(greenBot);
            orangeBot.bot.StatUpdate(orangeBot);

            hubStats = null;
        }
    }

    public void TransferStats(int botID)
    {
        if(botID == 0)
            hubStats.StatsUpdate(leadBot);
        else if(botID == 1)
            hubStats.StatsUpdate(blueBot);
        else if(botID == 2)
            hubStats.StatsUpdate(greenBot);
        else if(botID == 3)
            hubStats.StatsUpdate(orangeBot);
    }

    public void StatsUpdate(int botID, HUBStats.statsGroup botStats) //when called, using the integer value to identify what bot is being acted on and update the stats for that bot
    {
        botStruct tempStruct = new botStruct();

        //Before setting the stats for the targeted bot struct, set the new temporary structure equal to the one that is being changed
        if (botID == 0)
            tempStruct = leadBot;
        else if (botID == 1)
            tempStruct = blueBot;
        else if (botID == 2)
            tempStruct = greenBot;
        else if (botID == 3)
            tempStruct = orangeBot;

        tempStruct.points = botStats.pointsAvailable;
        tempStruct.level = botStats.playerLevel;
        tempStruct.exp = botStats.playerEXP;
        tempStruct.strength = botStats.strength;
        tempStruct.electronics = botStats.electronics;
        tempStruct.agility = botStats.agility;
        tempStruct.combat = botStats.combat;
        tempStruct.scanning = botStats.scanning;
        tempStruct.efficiency = botStats.efficiency;
        tempStruct.heavyHitter = botStats.heavyHitter;
        tempStruct.retention = botStats.retention;
        tempStruct.defender = botStats.defender;
        tempStruct.objMaster = botStats.objMaster;
        tempStruct.regenerator = botStats.regenerator;
        tempStruct.lucky = botStats.lucky;
        tempStruct.teamPlayer = botStats.teamPlayer;
        tempStruct.informant = botStats.informant;
        tempStruct.spec = botStats.specialization.text.ToString();
        tempStruct.perk1 = botStats.perk1;
        tempStruct.perk2 = botStats.perk2;
        tempStruct.perk3 = botStats.perk3;
        tempStruct.perk4 = botStats.perk4;
        tempStruct.perk5 = botStats.perk5;
        tempStruct.perk6 = botStats.perk6;
        if (tempStruct.perk1 != null)
            tempStruct.perk1.gameObject.transform.SetParent(gameObject.transform);
        if (tempStruct.perk2 != null)
            tempStruct.perk2.gameObject.transform.SetParent(gameObject.transform);
        if (tempStruct.perk3 != null)
            tempStruct.perk3.gameObject.transform.SetParent(gameObject.transform);
        if (tempStruct.perk4 != null)
            tempStruct.perk4.gameObject.transform.SetParent(gameObject.transform);
        if (tempStruct.perk5 != null)
            tempStruct.perk5.gameObject.transform.SetParent(gameObject.transform);
        if (tempStruct.perk6 != null)
            tempStruct.perk6.gameObject.transform.SetParent(gameObject.transform);

        //after setting the stats for the temporary bot struct, update the actual bot struct by setting it equal to this temporary one
        if (botID == 0)
            leadBot = tempStruct;
        else if (botID == 1)
            blueBot = tempStruct;
        else if (botID == 2)
            greenBot = tempStruct;
        else if (botID == 3)
            orangeBot = tempStruct;
    }
}
