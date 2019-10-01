using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerShoot : MonoBehaviour
{
    public GameObject particle;
    private LineRenderer laserLine;
    private RaycastHit hit;
    private Ray ray;
    GraphicRaycaster canvasCaster;
    EventSystem eventSys;
    PointerEventData pointData;
    private GameManager gameManager;
    private BotStats botStats;
    private HUBTracker hubTracker;
    private DisplayLog displayLog;
    private AudioSource audioS;
    private bool hitCanvas;
    public GameObject marker;
    private Animator animator;

    public Transform barrelEnd; //manually added object to determine end of barrel
    public Transform turretPos; //manually added object to determine turret rotation
    public Transform barrelPos; //manually added object to determine barrel rotation

    public bool shooting = false;
    public float fireRate = 0.5f;
    public float turretRotateSpeed;
    public float barrelMoveSpeed;
    public int critChance;
    public int heavyHitChance;
    public int heavyHitModifier;

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
        gameManager = GameObject.Find("Persistent Object").GetComponent<GameManager>();
        hubTracker = GameObject.Find("Persistent Object").GetComponent<HUBTracker>();
        botStats = GetComponent<BotStats>();
        canvasCaster = GameObject.Find("RunningUI").GetComponent<GraphicRaycaster>();
        eventSys = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        displayLog = GameObject.Find("RunningUI/Text Log/Log Panel/Content").GetComponent<DisplayLog>();
        animator = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        hitCanvas = false;
    }

    void Update()
    {
        FollowMouse();

        if (Input.GetButtonDown("Fire1")) //use a canvas raycast to detect if the item being clicked is a button, if not, shoot
        {
            pointData = new PointerEventData(eventSys);
            pointData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            canvasCaster.Raycast(pointData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<Button>())
                {
                    hitCanvas = true;
                    break;
                }
            }

            if(!hitCanvas)
            {
                Shoot();
            }

            hitCanvas = false;
        }
    }

    void Shoot() //draw a ray out from the player and damage the enemy
    {
        RaycastHit hit;

            //if the ray hits something when firing in a straight line
        if (Physics.Raycast(barrelEnd.position, barrelEnd.forward, out hit) && animator.GetBool("Firing") == false)
        {
            //set the laser line to end whereever the laser hits
            laserLine.SetPosition(0, barrelEnd.position);
            laserLine.SetPosition(1, hit.point);
            animator.SetBool("Firing", true);
            laserLine.enabled = true;
            audioS.PlayOneShot(GetComponent<MultipleAudioClips>().clips[1]);

            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                EnemyAIMachine enemyHit = hit.collider.gameObject.GetComponent<EnemyAIMachine>();
                Health enemyHP = hit.collider.gameObject.GetComponent<Health>();

                int randCritChance = Random.Range(0, 100);
                int randHeavyHitChance = Random.Range(0, 100);

                if(randCritChance <= critChance)
                {
                    int hitDamage = 2;

                    if(randHeavyHitChance <= heavyHitChance)
                        hitDamage += heavyHitModifier;

                    enemyHP.HealthReduce(hitDamage);
                    displayLog.RecieveLog(enemyHit.displayName + " takes " + hitDamage + " Damage, CRITICAL!!!!");
                }
                else
                {
                    int hitDamage = 1;

                    if (randHeavyHitChance <= heavyHitChance)
                        hitDamage += heavyHitModifier;

                    enemyHP.HealthReduce(hitDamage);
                    displayLog.RecieveLog(enemyHit.displayName + " takes " + hitDamage + " Damage");
                }

                hit.collider.gameObject.GetComponent<AudioSource>().PlayOneShot(hit.collider.gameObject.GetComponent<MultipleAudioClips>().clips[0]);

                enemyHit.enemyObject = gameObject;
                enemyHit.canSeeEnemy = true;

                if (enemyHP.health <= 0)
                {
                    gameManager.OnKill(enemyHit.gameObject);
                    displayLog.RecieveLog(enemyHit.displayName + " was destroyed", Color.red);

                    botStats.KillExpIncrease(hit.collider.gameObject);

                    hit.collider.gameObject.SetActive(false);
                }
            }
            else if (hit.collider.gameObject.CompareTag("EnemyStructure"))
            {
                StructureInfo enemyHit = hit.collider.gameObject.GetComponent<StructureInfo>();
                Health enemyHP = hit.collider.gameObject.GetComponent<Health>();

                int randCritChance = Random.Range(0, 100);
                int randHeavyHitChance = Random.Range(0, 100);

                if (randCritChance <= critChance)
                {
                    int hitDamage = 2;

                    if (randHeavyHitChance <= heavyHitChance)
                        hitDamage += heavyHitModifier;

                    enemyHP.HealthReduce(hitDamage);
                    displayLog.RecieveLog(enemyHit.displayName + " takes " + hitDamage + " Damage, CRITICAL!!!!");
                }
                else
                {
                    int hitDamage = 1;

                    if (randHeavyHitChance <= heavyHitChance)
                        hitDamage += heavyHitModifier;

                    enemyHP.HealthReduce(hitDamage);
                    displayLog.RecieveLog(enemyHit.displayName + " takes " + hitDamage + " Damage");
                }

                hit.collider.gameObject.GetComponent<AudioSource>().PlayOneShot(hit.collider.gameObject.GetComponent<MultipleAudioClips>().clips[0]);

                if (enemyHP.health <= 0)
                {
                    gameManager.OnKill(enemyHit.gameObject);
                    displayLog.RecieveLog(enemyHit.displayName + " was destroyed", Color.red);

                    botStats.KillExpIncrease(hit.collider.gameObject);

                    hit.collider.gameObject.SetActive(false);
                }
            }
        }
    }

    private void StopFiring()
    {
        animator.SetBool("Firing", false);
        laserLine.enabled = false;
    }

    private void FollowMouse() //called from update to have the turret and barrel of the bot follow the mouse
    {
        //create a var to hold the raycast hit data
        RaycastHit hit;

        //create a ray from the screen to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        //if the ray hits something, do this
        if (Physics.Raycast(ray, out hit))
        {
            //create a vector3 var to hold the location of the hit
            Vector3 hitPos = hit.point;

            //create a quaternion to store the rotation for the bot to the mouse pos
            Quaternion hitRot = Quaternion.LookRotation(hitPos - transform.position);

            //create a quaternion to store the rotation for the turret to the mouse pos
            Quaternion turretRot = Quaternion.LookRotation(hitPos - turretPos.transform.position);

            //Have the turret rotate towards the hit point using the hitRot var, use delta time to smooth travel
            turretPos.transform.rotation = Quaternion.RotateTowards(turretPos.transform.rotation, hitRot, Time.deltaTime * turretRotateSpeed);

            //set the euler angles of the turret so that the turret will only move on the y axis
            turretPos.transform.localEulerAngles = new Vector3(0, turretPos.transform.localEulerAngles.y, 0);

            //Have the barrel rotate move up or down to point at the hit point, smoothed, using the turrets transform as the height of the barrel from the hit point
            barrelPos.transform.rotation = Quaternion.RotateTowards(barrelPos.transform.rotation, turretRot, Time.deltaTime * barrelMoveSpeed);

            //set the euler angles of the barrel so that it will only rotate on the y axis
            barrelPos.transform.localEulerAngles = new Vector3(0, barrelPos.transform.localEulerAngles.y, 0);
        }
    }
}
