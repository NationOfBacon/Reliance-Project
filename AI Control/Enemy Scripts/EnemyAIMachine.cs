using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIMachineTools;
using UnityEngine.AI;

public class EnemyAIMachine : MonoBehaviour
{
    [HideInInspector]
    public EnemyBotStateMachine botMachine;
    [HideInInspector]
    public GameManager gameMgr;
    [HideInInspector]
    public NavMeshAgent thisAgent;
    [HideInInspector]
    public DisplayLog logD;
    [HideInInspector]
    public LineRenderer laserLine;
    [HideInInspector]
    public Health health;
    [HideInInspector]
    public AudioSource audioS;

    public string displayName;
    public int searchRadius;
    public int retreatRadius;
    public int reinforcementRadius;
    public int attackDistance;
    public int followDistance;
    public int hitChance;
    public int critChance;
    public int turretRotateSpeed;
    public int barrelMoveSpeed;
    public int bodyRotateSpeed;
    public Transform lineOrigin;
    public Transform barrelEnd;
    public Transform turretPos;
    public Transform barrelPos;
    public Vector3 contactPoint;
    public GameObject partnerBot;
    public GameObject partnerBot1;
    public GameObject objective;
    public GameObject enemyObject;
    public bool disabled = false;
    public bool canSeeEnemy = false;
    public bool onTarget = false;
    public bool leader = false;
    public bool defender = false;
    public bool turretBot = false;
    public bool stateSet = false;
    public bool removingEnemy = false;
    public bool insideNormalSearch = false;
    public bool insideDefend = false;
    public bool insideTurret = false;
    public bool insidePartner = false;
    public List<GameObject> movingBotsOnly = new List<GameObject>();
    public List<GameObject> additionalEnemies = new List<GameObject>();
    public List<GameObject> nearbyFriendlies = new List<GameObject>();
    public List<GameObject> friendlyBots;

    private void Awake()
    {
        transform.parent = null;
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            friendlyBots.Add(bot);
        }
        friendlyBots.Remove(this.gameObject);

        botMachine = GetComponent<EnemyBotStateMachine>();

        if (!turretBot)
        {
            thisAgent = GetComponent<NavMeshAgent>();
            displayName = "Enemy Bot";
        }
        else
        {
            thisAgent = null;
            displayName = "Enemy Turret";
        }

        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
        logD = GameObject.Find("RunningUI/Text Log/Log Panel/Content").GetComponent<DisplayLog>();
        gameMgr = GameObject.Find("Persistent Object").GetComponent<GameManager>();
        audioS = GetComponent<AudioSource>();
    }

    private void Start()
    {
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (bot.GetComponent<Health>())
            {
                if (bot.GetComponent<EnemyAIMachine>().turretBot == false)
                    movingBotsOnly.Add(bot);
            }

        }
        movingBotsOnly.Remove(this.gameObject);

        if (gameMgr.missionName == "Hack")
        {
            objective = GameObject.FindGameObjectWithTag("Terminal");
        }
        else if (gameMgr.missionName == "Defend")
        {
            objective = GameObject.FindGameObjectWithTag("DefendStructure");
        }
        else if (gameMgr.missionName == "Assault")
        {
            objective = GameObject.FindGameObjectWithTag("Cap Area");
        }
        else if (gameMgr.missionName == "Destroy")
        {
            objective = GameObject.FindGameObjectWithTag("EnemyStructure");
        }
        else if (gameMgr.missionName == "Protect")
        {
            objective = gameMgr.allyBots[0];
        }
    }

    void Update()
    {
        if (enemyObject != null) //if the enemy object is still set as a target despite being set to inactive, remove it here
            if (enemyObject.activeSelf == false)
                enemyObject = null;

        DrawSearchRays();

        SetMissionState();

        DrawBarrelRay();
    }

    public void DrawSearchRays() //draw rays out to detect things in the world
    {
        Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
        Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;

        for (var i = 0; i < 72; i++)
        {
            if (Physics.Raycast(lineOrigin.transform.position, direction, out hit, searchRadius))
            {
                if (hit.collider.gameObject.CompareTag("Target"))
                {
                    Debug.DrawRay(lineOrigin.transform.position, direction * hit.distance, Color.red);
                    canSeeEnemy = true;
                    additionalEnemies.Add(hit.collider.gameObject);
                    additionalEnemies = additionalEnemies.Distinct().ToList();

                    SetTarget();

                    break;
                }
                else if (hit.collider.gameObject.CompareTag("DefendStructure"))
                {
                    Debug.DrawRay(lineOrigin.transform.position, direction * hit.distance, Color.green);
                    canSeeEnemy = true;
                    additionalEnemies.Add(hit.collider.gameObject);
                    additionalEnemies = additionalEnemies.Distinct().ToList();

                    SetTarget();

                    break;
                }
                else if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Debug.DrawRay(lineOrigin.transform.position, direction * hit.distance, Color.blue);
                    nearbyFriendlies.Add(hit.collider.gameObject);
                    nearbyFriendlies = nearbyFriendlies.Distinct().ToList();
                }
                else
                {
                    Debug.DrawRay(lineOrigin.transform.position, direction * hit.distance, Color.yellow);
                    canSeeEnemy = false;
                }
            }
            else
            {
                Debug.DrawRay(lineOrigin.transform.position, direction * searchRadius, Color.white);
                canSeeEnemy = false;
            }
            direction = stepAngle * direction;
        }

        for (int i = 0; i < additionalEnemies.Count; i++)
        {
            if (Vector3.Distance(transform.position, additionalEnemies[i].transform.position) > searchRadius || additionalEnemies[i].activeSelf == false)
                additionalEnemies.RemoveAt(i);
        }

        for (int i = 0; i < nearbyFriendlies.Count; i++)
        {
            if (Vector3.Distance(transform.position, nearbyFriendlies[i].transform.position) > searchRadius || nearbyFriendlies[i].activeSelf == false)
                nearbyFriendlies.RemoveAt(i);
        }
    }

    public void DrawBarrelRay() //draw the barrel ray to determine if the barrel is on target
    {
        RaycastHit hit;

        if (Physics.Raycast(barrelEnd.transform.position, barrelEnd.forward, out hit, attackDistance + 10))
        {
            if (hit.collider.gameObject.CompareTag("Target") || hit.collider.gameObject.CompareTag("DefendStructure"))
            {
                Debug.DrawRay(barrelEnd.transform.position, barrelEnd.forward * hit.distance, Color.red);
                contactPoint = hit.point;
                onTarget = true;
            }
            else
            {
                Debug.DrawRay(barrelEnd.transform.position, barrelEnd.forward * hit.distance, Color.yellow);
                onTarget = false;
            }
        }
        else
        {
            Debug.DrawRay(barrelEnd.transform.position, barrelEnd.forward * attackDistance);
            onTarget = false;
        }
    }

    public void SetMissionState() //when the bot is spawned in, have it decide what mode to enter
    {
        if (!stateSet)
        {
            if (gameMgr.missionName == "Defend" || gameMgr.missionName == "Assault" || gameMgr.missionName == "Protect" || gameMgr.missionName == "Destroy" || gameMgr.missionName == "Hack")
            {
                if (defender)
                {
                    botMachine.ChangeState(EnemyMoveToState.instance);
                    insideDefend = true;
                }
                else
                {
                    SetSearch();
                }
            }
            else if (gameMgr.missionName == "Kill")
            {
                SetSearch();
            }

            stateSet = true;
        }
    }

    public void SetTarget() //after enemies are found this method will decide which enemy is the current target
    {
        float minDistance = float.MaxValue;
        GameObject targetBot = null;

        if (additionalEnemies.Count > 0) //if the list is not empty
        {
            for (int i = 0; i < additionalEnemies.Count; i++) //check if any of the enemies in the list are not active before setting the target
            {
                if (additionalEnemies[i].activeSelf == false)
                    additionalEnemies.Remove(additionalEnemies[i]);
            }

            if (additionalEnemies.Count == 1) //if theres only one enemy 
            {
                enemyObject = additionalEnemies[0];
                health = enemyObject.GetComponent<Health>();
            }
            else if(additionalEnemies.Count > 1) //if there is more than 1 enemy
            {
                for(int i = 0; i < additionalEnemies.Count; i++)// for each enemy in the list get the distance between this bot and the enemy and then decide if that distance is the shortest one
                {
                    float thisDistance = Vector3.Distance(transform.position, additionalEnemies[i].transform.position);

                    if(thisDistance < minDistance)
                    {
                        minDistance = thisDistance;
                        targetBot = additionalEnemies[i];
                    }
                }
                //after the loop is complete set the enemyobject to the target that is closest
                enemyObject = targetBot;
                health = enemyObject.GetComponent<Health>();
            }
        }
    }

    public void SetSearch() //called to set the normal search mode when the bot is not performing special actions
    {
        if (movingBotsOnly.Count >= 9)
        {
            if (!turretBot && leader == true)
            {
                botMachine.ChangeState(EnemySearchState.instance);
                insideNormalSearch = true;
            }

            if (!turretBot && leader == false)
            {
                botMachine.ChangeState(EnemyPartnerState.instance);
                insidePartner = true;
            }

            if (turretBot)
            {
                botMachine.ChangeState(EnemyTurretSearchState.instance);
                insideTurret = true;
            }
        }

        if (movingBotsOnly.Count < 9)
        {
            if (!turretBot)
            {
                botMachine.ChangeState(EnemySearchState.instance);
                insideNormalSearch = true;
            }

            if (turretBot)
            {
                botMachine.ChangeState(EnemyTurretSearchState.instance);
                insideTurret = true;
            }
        }
    }

    public void MoveToObjective() //used from other scripts to call enemy bots to an objective
    {
        insideDefend = true;
        insideTurret = false;
        insideNormalSearch = false;
        insidePartner = false;
        botMachine.ChangeState(EnemyMoveToState.instance);
    }
}
