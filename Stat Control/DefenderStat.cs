using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderStat : MonoBehaviour //placed on bots to control functionality of the defender stat
{
    private BotStats bStats;
    private Health hp;
    private DisplayLog dLog;

    private int startingProcChance;
    public int procChance;
    public bool statEnabled = false;

    private void Awake()
    {
        bStats = GetComponent<BotStats>();
        hp = GetComponent<Health>();
        dLog = GameObject.Find("RunningUI/Text Log/Log Panel/Content").GetComponent<DisplayLog>();
    }
    private void Start()
    {
        startingProcChance = procChance;
    }

    public bool OnTakeHitReturn() //called by health reduce methods in the health script to see if the player should recieve damage or not
    {
        bool activated = false;

        int randProcChance = Random.Range(0, 100);

        if (randProcChance <= procChance)
        {
            if(hp.shield != hp.maxShield)
            {
                activated = true;

                if (GetComponent<PlayerShoot>())
                    dLog.RecieveLog("Defender stat absorbed damage for Lead Bot");
                else
                    dLog.RecieveLog("Defender stat absorbed damage for " + GetComponent<AIMachine>().displayName);
            }
        }

        return activated;
    }

    public void UpdateProcChance(int chanceMod)
    {
        procChance = startingProcChance + chanceMod;

        if (chanceMod >= 1)
        {
            statEnabled = true;
            hp.SetShieldActive();
        }
    }

}
