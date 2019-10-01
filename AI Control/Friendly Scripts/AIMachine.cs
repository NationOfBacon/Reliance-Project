using AIMachineTools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class AIMachine : MonoBehaviour
{
    [HideInInspector]
    public DisplayLog logD;
    [HideInInspector]
    public GameManager gameManager;
    [HideInInspector]
    public BotStats botStats;
    [HideInInspector]
    public Health enemyHealth;
    [HideInInspector]
    public NavMeshAgent thisAgent;
    [HideInInspector]
    public BotStateMachine botMachine;
    [HideInInspector]
    public LineRenderer laserLine;
    [HideInInspector]
    public AudioSource audioS;

    public string displayName;
    public int followDistance;
    public int healDistance;
    public int searchRadius;
    public int attackDistance;
    public int retreatRadius;
    public int reinforcementRadius;
    public int hitChance;
    public int critChance;
    public int heavyHitChance;
    public int heavyHitModifier;
    public float fireRate;
    public float healRate;
    public float markerDistance;
    public float moveToDistance;
    public int turretRotateSpeed;
    public int barrelMoveSpeed;
    public int bodyRotateSpeed;
    public Transform lineOrigin;
    public Transform barrelEnd;
    public Transform turretPos;
    public Transform barrelPos;
    public Vector3 contactPoint;
    public Vector3 targetWaypoint;
    public Vector3 startingPos;
    public GameObject marker;
    public GameObject grenadePrefab;
    public GameObject areaHealPrefab;
    public GameObject enemyObject;
    public GameObject barrelHitTarget;
    public GameObject playerObject;
    public GameObject followTarget;
    public GameObject markerCopy;
    public bool canSeeEnemy;
    public bool onTarget = false;
    public bool supportBuff = false;
    public bool removingEnemy = false;
    public bool disabled = false;
    public List<GameObject> friendlyBots;
    public List<GameObject> additionalEnemies;
    public List<GameObject> nearbyFriendlies;
    public List<GameObject> objectives;

    void Start()
    {
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Target"))
        {
            if(bot.GetComponent<Health>() && bot.GetComponent<AIMachine>())
                friendlyBots.Add(bot);
        }
        friendlyBots.Remove(this.gameObject);
        friendlyBots.Remove(GameObject.Find("PlayerSphere"));

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Cap Area")) //if there are any capture areas in the level, add them to the list
        {
            objectives.Add(obj);
        }

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Terminal")) //if there are any terminals in the level, add them to the list
        {
            objectives.Add(obj);
        }

        botMachine = GetComponent<BotStateMachine>();
        thisAgent = GetComponent<NavMeshAgent>();
        botStats = GetComponent<BotStats>();
        gameManager = GameObject.Find("Persistent Object").GetComponent<GameManager>();
        logD = GameObject.Find("RunningUI/Text Log/Log Panel/Content").GetComponent<DisplayLog>();
        playerObject = GameObject.Find("PlayerSphere");
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
        audioS = GetComponent<AudioSource>();
        startingPos = transform.position;

        if (gameObject.name.Contains("Blue"))
            displayName = "Blue Bot";
        if (gameObject.name.Contains("Green"))
            displayName = "Green Bot";
        if (gameObject.name.Contains("Orange"))
            displayName = "Orange Bot";
    }

    void Update()
    {
        if (enemyObject != null) //if the enemy object is still set as a target despite being set to inactive, remove it here
            if (enemyObject.activeSelf == false)
            {
                enemyObject = null;
                Debug.Log(displayName + ": Enemy object is not active, removing.");
            }


        DrawSearchRays();

        DrawBarrelRay();

        KeyCommands();
    }

    private void KeyCommands()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //sets the bot into a idle state
        {
            botMachine.ChangeState(IdleState.instance);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) //sets the bot into a follow state
        {
            botMachine.ChangeState(FollowState.instance);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //sets the bot into a search state
        {
            botMachine.ChangeState(SearchState.instance);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) //sets the bot into a sentry state
        {
            botMachine.ChangeState(SentryState.instance);
        }

        if (Input.GetButtonDown("AI Move To Command")) //sets the bot into a moveto state
        {
            botMachine.ChangeState(MoveToState.instance);
        }
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
                else if(hit.collider.gameObject.CompareTag("Target"))
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

        for(int i = 0; i < additionalEnemies.Count; i++)
        {
            if (Vector3.Distance(transform.position, additionalEnemies[i].transform.position) > searchRadius || additionalEnemies[i].activeSelf == false)
                additionalEnemies.RemoveAt(i);
        }

        for(int i = 0; i < nearbyFriendlies.Count; i++)
        {
            if (Vector3.Distance(transform.position, nearbyFriendlies[i].transform.position) > searchRadius || nearbyFriendlies[i].activeSelf == false)
                nearbyFriendlies.RemoveAt(i);
        }
    }

    public void DrawBarrelRay()
    {
        RaycastHit hit;

        if (Physics.Raycast(barrelEnd.transform.position, barrelEnd.forward, out hit, attackDistance + 10f))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.DrawRay(barrelEnd.transform.position, barrelEnd.forward * hit.distance, Color.red);
                barrelHitTarget = hit.collider.gameObject;
                contactPoint = hit.point;
                onTarget = true;
            }
            else if (hit.collider.gameObject.CompareTag("Target"))
            {
                Debug.DrawRay(barrelEnd.transform.position, barrelEnd.forward * hit.distance, Color.blue);
                barrelHitTarget = hit.collider.gameObject;
                contactPoint = hit.point;
                onTarget = true;
            }
            else if (hit.collider.gameObject.CompareTag("EnemyStructure"))
            {
                Debug.DrawRay(barrelEnd.transform.position, barrelEnd.forward * hit.distance, Color.green);
                barrelHitTarget = hit.collider.gameObject;
                contactPoint = hit.point;
                onTarget = true;
            }
            else
            {
                Debug.DrawRay(barrelEnd.transform.position, barrelEnd.forward * hit.distance, Color.yellow);
                barrelHitTarget = null;
                onTarget = false;
            }
        }
        else
        {
            Debug.DrawRay(barrelEnd.transform.position, barrelEnd.forward * attackDistance);
            barrelHitTarget = null;
            onTarget = false;
        }
    }

    public void SetTarget() //after enemies are found this method will decide which enemy is the current target
    {
        float minDistance = float.MaxValue;
        GameObject targetBot = null;

        if (additionalEnemies.Count > 0) //if the list is not empty
        {
            for(int i = 0; i < additionalEnemies.Count; i++) //check if any of the enemies in the list are not active before setting the target
            {
                if (additionalEnemies[i].activeSelf == false)
                    additionalEnemies.Remove(additionalEnemies[i]);
            }

            if (additionalEnemies.Count == 1) //if theres only one enemy 
            {
                enemyObject = additionalEnemies[0];
                enemyHealth = enemyObject.GetComponent<Health>();
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
                enemyHealth = enemyObject.GetComponent<Health>();
            }
        }
    }

    public void MarkLocation() //gets the location for use with the moveto state
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            markerCopy = Instantiate(marker, hit.point, Quaternion.identity);
            marker.transform.position = hit.point;
        }
    }

    public void RemoveMarker() //get rid of the marker that was created
    {
        if(markerCopy != null)
            Destroy(markerCopy);
    }
}
