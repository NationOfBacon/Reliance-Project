using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    [System.Serializable]
    public struct perkStats
    {
        public string perkName;
        public string perkDesc;
        public Sprite perkImage;
        public int damageBuff;
        public int healthBuff;
        public int rangeBuff;
        public int accuracyBuff;
        public int selfHealingBuff;
        public int speedBuff;
        public int fireRateBuff;
        public int stationaryDamageBuff;
        public int auraSizeBuff;
        public int throwDistanceBuff;
        public int partyHealingBuff;
    };

    public perkStats stats = new perkStats();

    public void UpdateImage()
    {
        GetComponent<Image>().sprite = stats.perkImage;
    }
}
