using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData //takes data and converts it for placement in a save file
{
    [System.Serializable]
    public struct botData
    {
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

        public int perk1;
        public int perk2;
        public int perk3;
        public int perk4;
        public int perk5;
        public int perk6;
    };

    public botData leadBot = new botData();
    public botData blueBot = new botData();
    public botData greenBot = new botData();
    public botData orangeBot = new botData();

    public PlayerData (HUBTracker tracker)
    {

        for(int i = 0; i < 4; i++)
        {
            var currentStruct = new HUBTracker.botStruct();
            var currentBot = new botData();

            if (i == 0)
            {
                currentStruct = tracker.leadBot;
                currentBot = leadBot;
            }
            else if (i == 1)
            {
                currentStruct = tracker.blueBot;
                currentBot = blueBot;
            }
            else if (i == 2)
            {
                currentStruct = tracker.greenBot;
                currentBot = greenBot;
            }
            else if (i == 3)
            {
                currentStruct = tracker.orangeBot;
                currentBot = orangeBot;
            }

            currentBot.points = currentStruct.points;
            currentBot.level = currentStruct.level;
            currentBot.exp = currentStruct.exp;
            currentBot.strength = currentStruct.strength;
            currentBot.electronics = currentStruct.electronics;
            currentBot.agility = currentStruct.agility;
            currentBot.combat = currentStruct.combat;
            currentBot.scanning = currentStruct.scanning;
            currentBot.efficiency = currentStruct.efficiency;
            currentBot.heavyHitter = currentStruct.heavyHitter;
            currentBot.retention = currentStruct.retention;
            currentBot.defender = currentStruct.defender;
            currentBot.objMaster = currentStruct.objMaster;
            currentBot.regenerator = currentStruct.regenerator;
            currentBot.lucky = currentStruct.lucky;
            currentBot.teamPlayer = currentStruct.teamPlayer;
            currentBot.informant = currentStruct.informant;
            currentBot.spec = currentStruct.spec;

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
}
