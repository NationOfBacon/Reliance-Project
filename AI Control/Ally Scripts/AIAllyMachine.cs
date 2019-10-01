using AllyAIMachineTools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class AIAllyMachine : MonoBehaviour
{
    [HideInInspector]
    public GameManager gameManager;
    [HideInInspector]
    public DisplayLog logD;
    [HideInInspector]
    public AllyBotStateMachine botMachine;
    [HideInInspector]
    public NavMeshAgent thisAgent;
    [HideInInspector]
    public LineRenderer laserLine;
    [HideInInspector]
    public AudioSource audioS;
    [HideInInspector]
    public Health enemyHealth;

    public string displayName = "Ally Bot";
    public int attackDistance;
    public int hitChance;
    public int searchRadius;
    public int stopDistance;
    public int turretRotateSpeed;
    public int barrelMoveSpeed;
    public int bodyRotateSpeed;
    public int retreatRadius;
    public int reinforcementRadius;
    public GameObject enemyObject;
    public GameObject playerObject;
    public Transform lineOrigin;
    public Transform barrelEnd;
    public Transform turretPos;
    public Transform barrelPos;
    public Vector3 contactPoint;
    public Vector3 regroupPos;
    public bool canSeeEnemy;
    public bool disabled;
    public bool onTarget = false;
    public bool removingEnemy = false;
    public bool supportBuff = false;
    public List<GameObject> friendlyBots;
    public List<GameObject> additionalEnemies = new List<GameObject>();
    public List<GameObject> nearbyFriendlies = new List<GameObject>();

    private void Start()
    {
        transform.parent = null;
        botMachine = GetComponent<AllyBotStateMachine>();
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Target"))
        {
            if (bot.GetComponent<Health>() && bot.GetComponent<AIAllyMachine>())
                friendlyBots.Add(bot);
        }
        friendlyBots.Remove(this.gameObject);
        friendlyBots.Remove(GameObject.Find("PlayerSphere"));

        thisAgent = GetComponent<NavMeshAgent>();
        gameManager = GameObject.Find("Persistent Object").GetComponent<GameManager>();
        logD = GameObject.Find("RunningUI/Text Log/Log Panel/Content").GetComponent<DisplayLog>();
        playerObject = GameObject.Find("PlayerSphere");
        laserLine = GameObject.Find("Line Render").GetComponent<LineRenderer>();
        audioS = GetComponent<AudioSource>();
        regroupPos = GameObject.Find("AllyBotRegroupPoint").transform.position;
    }

    void Update()
    {
        DrawSearchRays();

        DrawBarrelRay();
    }

    private void DrawSearchRays()
    {
        Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
        Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;

        for (var i = 0; i < 72; i++) //creates the draw rays that the bot uses to detect enemies
        {
            if (Physics.Raycast(lineOrigin.transform.position, direction, out hit, searchRadius))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Debug.DrawRay(lineOrigin.transform.position, direction * hit.distance, Color.red);
                    canSeeEnemy = true;
                    additionalEnemies.Add(hit.collider.gameObject);
                    additionalEnemies = additionalEnemies.Distinct().ToList();

                    SetTarget();

                    break;
                }
                else if (hit.collider.gameObject.CompareTag("EnemyStructure"))
                {
                    Debug.DrawRay(lineOrigin.transform.position, direction * hit.distance, Color.green);
                    canSeeEnemy = true;
                    additionalEnemies.Add(hit.collider.gameObject);
                    additionalEnemies = additionalEnemies.Distinct().ToList();

                    SetTarget();

                    break;
                }
                else if (hit.collider.gameObject.CompareTag("Target"))
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
    }

    public void SetTarget() //after enemies are found this method will decide which enemy is the current target
    {
        float minDistance = float.MaxValue;
        GameObject targetBot = null;

        if (additionalEnemies.Count > 0) //if the list is not empty
        {
            if (additionalEnemies.Count == 1) //if theres only one enemy 
            {
                enemyObject = additionalEnemies[0];
                enemyHealth = enemyObject.GetComponent<Health>();
            }
            else if (additionalEnemies.Count > 1) //if there is more than 1 enemy
            {
                for (int i = 0; i < additionalEnemies.Count; i++)// for each enemy in the list get the distance between this bot and the enemy and then decide if that distance is the shortest one
                {
                    float thisDistance = Vector3.Distance(transform.position, additionalEnemies[i].transform.position);

                    if (thisDistance < minDistance)
                    {
                        minDistance = thisDistance;
                        targetBot = additionalEnemies[i];
                    }
                }
                //after the loop is complete set the enemyobject to the target that is closest
                enemyObject = targetBot;
                enemyHealth = enemyObject.GetComponent<Health>();
            }
        }
    }

    public void DrawBarrelRay()
    {
        RaycastHit hit;

        if (Physics.Raycast(barrelEnd.transform.position, barrelEnd.forward, out hit, attackDistance + 10))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.DrawRay(barrelEnd.transform.position, barrelEnd.forward * hit.distance, Color.red);
                contactPoint = hit.point;
                onTarget = true;
            }
            else if (hit.collider.gameObject.CompareTag("Target"))
            {
                Debug.DrawRay(barrelEnd.transform.position, barrelEnd.forward * hit.distance, Color.blue);
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
}
