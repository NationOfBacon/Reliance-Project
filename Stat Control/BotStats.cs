using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class BotStats : MonoBehaviour
{
    private int botPoints;
    private int botLevel;
    private int botExp;
    private int strength; //modifies the max health of the bot
    private int electronics; //modifies how fast the bot captures objectives (terminals and capture areas)
    private int agility; //modifies the speed of the bot
    private int combat; //modifies the accuracy of each shot and crit chance
    private int scanning; //modifies the size of the bots search radius
    private int efficiency; //improves bots turret turn speed and fire rate
    private int heavyHitter; //increases chances that the bot will deal higher normal and critical damage
    private int retention; //increases the amount of exp gained from all sources
    private int defender; //Gives the bot a shield point each time the stat is proc'd by an enemy shooting at it
    private int objMaster; //grants the bot a boost to luck, combat and efficiency when inside the cap zone of a terminal or capture area
    private int regenerator; //gives the bot the ability to regen health
    private int lucky; //reduces the enemies chance of hitting the bot
    private int teamPlayer; //applies buff, increasing CBT, REGEN and ELEC to nearby units, each point increases the range the buff is applied
    private int informant; //when a bot gets near to an enemy, its health is displayed over its head. Each point increases the range that health bars will appear
    private string specalization; //role that the bot has chosen

    public TeamPlayerStat tpStatObject;
    public InformantStat infoStatObject;

    private Perk perk1; //special perk that modifies multiple different aspects about the bot
    private Perk perk2;
    private Perk perk3;
    private Perk perk4;
    private Perk perk5;
    private Perk perk6;

    private Health health;
    private NavMeshAgent agent;
    private AIMachine aiMachine;
    private EnemyAIMachine enemyAI;
    private HUBTracker hubTracker;

    //stats that will be used to store the base values of each value to be buffed
    float agentSpeed;
    float agentAcc;
    float agentAngSpd;
    int hitChance;
    int searchRadius;
    int turretRotateSpd;
    int barrelMoveSpd;
    float fireRate;
    int heavyHitChance;
    int heavyHitMod;

    float playerForwardSpeed;
    float playerReverseSpeed;
    float playerTurnSpeed;
    int critChance;

    bool firstStatsSet = false;
    bool playerFirstStatsSet = false;


    void Start()
    {
        hubTracker = GameObject.Find("Persistent Object").GetComponent<HUBTracker>();
        health = GetComponent<Health>();

        if (!GetComponent<PlayerMove>())
        {
            SetStatBonuses();
            SetPerkBonuses();
        }
        else
        {
            SetPlayerStatBonuses();
        }

        Debug.Log("bot stats start finished");
    }

    public void SetStatBonuses()
    {
        aiMachine = GetComponent<AIMachine>();
        agent = GetComponent<NavMeshAgent>();

        if (!firstStatsSet)
            health.HealthUpdate(strength, true);
        else
            health.HealthUpdate(strength, false);

        if (firstStatsSet == false)
        {
            agentSpeed = agent.speed;
            agentAcc = agent.acceleration;
            agentAngSpd = agent.angularSpeed;
            hitChance = aiMachine.hitChance;
            searchRadius = aiMachine.searchRadius;
            turretRotateSpd = aiMachine.turretRotateSpeed;
            barrelMoveSpd = aiMachine.barrelMoveSpeed;
            fireRate = aiMachine.fireRate;
            heavyHitChance = aiMachine.heavyHitChance;
            heavyHitMod = aiMachine.heavyHitModifier;
        }

        agent.speed = agility + agentSpeed;
        agent.acceleration = agility + agentAcc;
        agent.angularSpeed = agility + agentAngSpd;

        aiMachine.hitChance = combat + hitChance;
        aiMachine.critChance = combat + critChance;

        aiMachine.searchRadius = scanning + searchRadius;

        aiMachine.turretRotateSpeed = (efficiency * 2) + turretRotateSpd;
        aiMachine.barrelMoveSpeed = (efficiency * 2) + barrelMoveSpd;
        aiMachine.fireRate = fireRate - (efficiency / 50); //may not work correctly

        aiMachine.heavyHitChance = heavyHitter + heavyHitChance;

        int hHMod = 0;
        for (int i = 0; i < heavyHitter; i += 3)
        {
            hHMod += 1;
        }
        aiMachine.heavyHitModifier = hHMod + heavyHitMod;

        firstStatsSet = true;

        GetComponent<DefenderStat>().UpdateProcChance(defender);

        if (regenerator >= 1)
            health.autoHeal = true;

        if (teamPlayer >= 1)
            tpStatObject.EnableStat(teamPlayer);

        if (informant >= 1)
            infoStatObject.SetRadius(informant);
    }

    public void SetPlayerStatBonuses()
    {
        if (!playerFirstStatsSet)
            health.HealthUpdate(strength, true);
        else
            health.HealthUpdate(strength, false);

        PlayerMove playerMove = GetComponent<PlayerMove>();
        PlayerShoot playerShoot = GetComponent<PlayerShoot>();

        if (playerFirstStatsSet == false)
        {
            playerForwardSpeed = playerMove.forwardSpeed;
            playerReverseSpeed = playerMove.reverseSpeed;
            playerTurnSpeed = playerMove.turnSpeed;
            critChance = playerShoot.critChance;
            turretRotateSpd = Mathf.RoundToInt(playerShoot.turretRotateSpeed);
            barrelMoveSpd = Mathf.RoundToInt(playerShoot.barrelMoveSpeed);
            fireRate = playerShoot.fireRate;
            heavyHitChance = playerShoot.heavyHitChance;
            heavyHitMod = playerShoot.heavyHitModifier;
        }

        //agility
        playerMove.forwardSpeed = agility + playerForwardSpeed;
        playerMove.reverseSpeed = agility + playerReverseSpeed;
        playerMove.turnSpeed = agility + playerTurnSpeed;

        //combat
        playerShoot.critChance = combat + critChance;

        //scanning (gives player ability to shoot out a pulse that shows enemies behind walls, each point increases range of pulse and duration that enemies stay visible)

        //efficiency

        //heavy hitter
        playerShoot.heavyHitChance = heavyHitter + heavyHitChance;

        int hHMod = 0;
        for(int i = 0; i < heavyHitter; i += 3)
        {
            hHMod += 1;
        }
        playerShoot.heavyHitModifier = hHMod + heavyHitMod;

        playerFirstStatsSet = true;

        //defender
        GetComponent<DefenderStat>().UpdateProcChance(defender);
        //regenerator
        if (regenerator >= 1)
            health.autoHeal = true;
        else
            health.autoHeal = false;
        //team player
        if (teamPlayer >= 1)
            tpStatObject.EnableStat(teamPlayer);

        if(informant >= 1)
            infoStatObject.SetRadius(informant);

    }

    public void SetPerkBonuses()
    {
        if (perk1 != null) //if there is a perk in the slot
        {
            if (specalization == "Medic") //if the bots specialization is medic
            {
                if (perk1.stats.perkName == "Fast Heal") //if the perk is called fast heal
                {
                    if (GetComponent<PlayerMove>())
                    {

                    }
                    else
                    {
                        //heals nearby friendly units at higher rates than base
                        aiMachine.healRate -= perk1.stats.partyHealingBuff;

                        if (aiMachine.healRate < 1)
                        {
                            aiMachine.healRate = 1;
                        }
                    }
                }

                if (perk1.stats.perkName == "Self Sustaining")
                {
                    if (GetComponent<PlayerMove>())
                    {
                        health.autoHeal = true;
                    }
                    else
                    {
                        //enables passive healing
                        health.autoHeal = true;
                    }

                }
            }
        }
        if (perk2 != null) //if there is a perk in the slot
        {
            if (specalization == "Medic") //if the bots specialization is medic
            {
                if (perk2.stats.perkName == "Fast Heal") //if the perk is called fast heal
                {
                    if (GetComponent<PlayerMove>())
                    {

                    }
                    else
                    {
                        //heals nearby friendly units at higher rates than base
                        aiMachine.healRate -= perk2.stats.partyHealingBuff;

                        if (aiMachine.healRate < 1)
                        {
                            aiMachine.healRate = 1;
                        }
                    }

                }

                if (perk2.stats.perkName == "Self Sustaining")
                {
                    if (GetComponent<PlayerMove>())
                    {
                        health.autoHeal = true;
                    }
                    else
                    {
                        //enables passive healing
                        health.autoHeal = true;
                    }

                }
            }
        }
    }

    public void StatUpdate(HUBTracker.botStruct botStats) //recieves bot stats from the hub tracker
    {
        botPoints = botStats.points;
        botLevel = botStats.level;
        botExp = botStats.exp;
        strength = botStats.strength;
        electronics = botStats.electronics;
        agility = botStats.agility;
        combat = botStats.combat;
        scanning = botStats.scanning;
        efficiency = botStats.efficiency;
        heavyHitter = botStats.heavyHitter;
        retention = botStats.retention;
        defender = botStats.defender;
        objMaster = botStats.objMaster;
        regenerator = botStats.regenerator;
        lucky = botStats.lucky;
        teamPlayer = botStats.teamPlayer;
        informant = botStats.informant;
        specalization = botStats.spec;
        perk1 = botStats.perk1;
        perk2 = botStats.perk2;
        perk3 = botStats.perk3;
        perk4 = botStats.perk4;
        perk5 = botStats.perk5;
        perk6 = botStats.perk6;
    }

    public void KillExpIncrease(GameObject targetRef) //called on kill to distribute exp
    {
        if(targetRef.GetComponent<StructureInfo>()) //if the target killed was a structure
        {
            if (this.gameObject.name == "AISphere Blue")
                hubTracker.blueBot.exp += (60 + retention);
            else if (this.gameObject.name == "AISphere Green")
                hubTracker.greenBot.exp += (60 + retention);
            else if (this.gameObject.name == "AISphere Orange")
                hubTracker.orangeBot.exp += (60 + retention);
            else if (this.gameObject.name == "Player Sphere")
                hubTracker.leadBot.exp += (60 + retention);
        }
        else if(!targetRef.GetComponent<EnemyAIMachine>().turretBot) //if the target killed was not a turret
        {
            if (this.gameObject.name == "AISphere Blue")
                hubTracker.blueBot.exp += (20 + retention);
            else if (this.gameObject.name == "AISphere Green")
                hubTracker.greenBot.exp += (20 + retention);
            else if (this.gameObject.name == "AISphere Orange")
                hubTracker.orangeBot.exp += (20 + retention);
            else if (this.gameObject.name == "Player Sphere")
                hubTracker.leadBot.exp += (20 + retention);
        }
        else //if the target killed was a turret
        {
            if (this.gameObject.name == "AISphere Blue")
                hubTracker.blueBot.exp += (40 + retention);
            else if (this.gameObject.name == "AISphere Green")
                hubTracker.greenBot.exp += (40 + retention);
            else if (this.gameObject.name == "AISphere Orange")
                hubTracker.orangeBot.exp += (40 + retention);
            else if (this.gameObject.name == "Player Sphere")
                hubTracker.leadBot.exp += (40 + retention);
        }
    }

    public IEnumerator SupportBuff() //called from the areabuff script to buff the bot when it enters a buff area
    {
        aiMachine.supportBuff = true;
        agent.speed = 40;
        agent.acceleration = 40;
        agent.angularSpeed = 40;
        Debug.Log("applied buff");
        yield return new WaitForSeconds(9.0f);
        agent.speed = 30;
        agent.acceleration = 30;
        agent.angularSpeed = 30;
        Debug.Log("removed buff");
        aiMachine.supportBuff = false;
    }

    public void CapSpeedIncrease(CaptureArea capArea) //called from the capture area to boost cap rate
    {
        capArea.capSpeed += electronics;
    }

    public void CapSpeedReduce(CaptureArea capArea)
    {
        capArea.capSpeed -= electronics;
    }

    public void CapStatAdd()
    {
        int luck = 0, cbt = 0, eff = 0;

        for(int i = 0; i < objMaster; i += 5)
        {
            luck += 5;
            cbt += 5;
            eff += 5;
        }

        lucky += luck;
        combat += cbt;
        efficiency += eff;

        if (GetComponent<PlayerShoot>())
            SetPlayerStatBonuses();
        else
            SetStatBonuses();

    }

    public void CapStatRemove()
    {
        int luck = 0, cbt = 0, eff = 0;

        for (int i = 0; i < objMaster; i += 5)
        {
            luck += 5;
            cbt += 5;
            eff += 5;
        }

        lucky -= luck;
        combat -= cbt;
        efficiency -= eff;

        if (GetComponent<PlayerShoot>())
            SetPlayerStatBonuses();
        else
            SetStatBonuses();
    }

    public bool SendObjmValue()
    {
        bool isActive = false;
        if (objMaster >= 1)
            isActive = true;

        return isActive;
    }

    public int SendRegenValue()
    {
        return regenerator;
    }

    public int ModifyEnemyHitChance(int enemyHitChance)
    {
        return enemyHitChance -= lucky;
    }

    public void GiveTeamPlayerBuff()
    {
        int cbt = 0, regen = 0, elec = 0;

        for(int i = 0; i < teamPlayer; i += 5)
        {
            cbt += 5;
            regen += 5;
            elec += 5;
        }

        combat += cbt;
        regenerator += regen;
        electronics += elec;

        if (GetComponent<PlayerShoot>())
            SetPlayerStatBonuses();
        else
            SetStatBonuses();
    }

    public void RemoveTeamPlayerBuff()
    {
        int cbt = 0, regen = 0, elec = 0;

        for(int i = 0; i < teamPlayer; i += 5)
        {
            cbt += 5;
            regen += 5;
            elec += 5;
        }

        combat -= cbt;
        regenerator -= regen;
        electronics -= elec;

        if (GetComponent<PlayerShoot>())
            SetPlayerStatBonuses();
        else
            SetStatBonuses();
    }
}
