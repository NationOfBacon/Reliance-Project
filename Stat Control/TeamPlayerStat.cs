using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamPlayerStat : MonoBehaviour //goes onto each bot and controls the behaivor of the team player stat
{
    private BotStats bStats;
    public int radius;
    private int modifiedRadius;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void EnableStat(int tpStat)
    {
        gameObject.SetActive(true);

        modifiedRadius = radius + tpStat;
        GetComponent<SphereCollider>().radius = modifiedRadius;
    }

    public void DisableStat()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            bStats = other.gameObject.GetComponent<BotStats>();
            bStats.GiveTeamPlayerBuff();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            bStats = other.gameObject.GetComponent<BotStats>();
            bStats.RemoveTeamPlayerBuff();
        }
    }
}
