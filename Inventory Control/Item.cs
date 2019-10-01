using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [System.Serializable]
    public struct itemStruct
    {
        public GameObject thisObject;
        public int ID;
        public string displayName;
        public string type;
        public string description;
        public Sprite icon;
        public bool pickedUp;
        public int strBonus;
        public int elecBonus;
        public int agiBonus;
        public int combatBonus;
        public int scanBonus;
        public int effBonus;
        public int hitBonus;
        public int retBonus;
        public int ventBonus;
        public int objMBonus;
        public int regenBonus;
        public int luckBonus;
        public int teamPBonus;
        public int infoBonus;
        public int sellPrice;
        public int buyPrice;

        public bool equipped;
        public bool leadOwns;
        public bool blueOwns;
        public bool greenOwns;
        public bool orangeOwns;
    };

    public List<int> bonusStats = new List<int>();

    public List<string> bonusNames = new List<string>() { "STR", "ELEC", "AGI", "CBT", "SCN", "EFF", "HIT", "RET", "VENT", "OBJM", "REGEN", "LUCK", "TEAMP", "INFO" };

    public itemStruct itemStats = new itemStruct();

    private void Awake()
    {
        itemStats.equipped = false;
        itemStats.leadOwns = false;
        itemStats.blueOwns = false;
        itemStats.greenOwns = false;
        itemStats.orangeOwns = false;
        itemStats.thisObject = gameObject;

        bonusStats.Add(itemStats.strBonus);
        bonusStats.Add(itemStats.elecBonus);
        bonusStats.Add(itemStats.agiBonus);
        bonusStats.Add(itemStats.combatBonus);
        bonusStats.Add(itemStats.scanBonus);
        bonusStats.Add(itemStats.effBonus);
        bonusStats.Add(itemStats.hitBonus);
        bonusStats.Add(itemStats.retBonus);
        bonusStats.Add(itemStats.ventBonus);
        bonusStats.Add(itemStats.objMBonus);
        bonusStats.Add(itemStats.regenBonus);
        bonusStats.Add(itemStats.luckBonus);
        bonusStats.Add(itemStats.teamPBonus);
        bonusStats.Add(itemStats.infoBonus);
    }

    private void Update()
    {
        bonusStats[0] = itemStats.strBonus;
        bonusStats[1] = itemStats.elecBonus;
        bonusStats[2] = itemStats.agiBonus;
        bonusStats[3] = itemStats.combatBonus;
        bonusStats[4] = itemStats.scanBonus;
        bonusStats[5] = itemStats.effBonus;
        bonusStats[6] = itemStats.hitBonus;
        bonusStats[7] = itemStats.retBonus;
        bonusStats[8] = itemStats.ventBonus;
        bonusStats[9] = itemStats.objMBonus;
        bonusStats[10] = itemStats.regenBonus;
        bonusStats[11] = itemStats.luckBonus;
        bonusStats[12] = itemStats.teamPBonus;
        bonusStats[13] = itemStats.infoBonus;
    }
}
