using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureArea : MonoBehaviour
{
    public bool captured;
    public int capSpeed = 1;
    private float captureRate;
    public float roundedRate;
    public bool botsCalled = false;
    private Animator anim;
    private AudioSource audioS;

    private GameManager gameMgr;

    private void Awake()
    {
        gameMgr = GameObject.Find("Persistent Object").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;

        captureRate = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(captureRate >= 100)
        {
            captured = true;
            captureRate = 100;
            roundedRate = 100;
        }

        if(captureRate > 0 && botsCalled == false)
        {
            SendEnemies();
            botsCalled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            bool objmActive = other.gameObject.GetComponent<BotStats>().SendObjmValue();

            if(objmActive)
            {
                other.gameObject.GetComponent<BotStats>().CapStatAdd();
            }

            other.gameObject.GetComponent<BotStats>().CapSpeedIncrease(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Target")
        {
            captureRate += capSpeed * Time.deltaTime;
            anim.SetBool("Capturing", true);
            audioS.Play();
            roundedRate = Mathf.Round(captureRate);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            audioS.Stop();
            anim.SetBool("Capturing", false);

            bool objmActive = other.gameObject.GetComponent<BotStats>().SendObjmValue();

            if (objmActive)
            {
                other.gameObject.GetComponent<BotStats>().CapStatRemove();
            }

            other.gameObject.GetComponent<BotStats>().CapSpeedReduce(this);
        }
    }

    public void SendEnemies() //when the capture rate starts to go up, this will be called to send enemies to the objective
    {
        for(int i = 0; i < gameMgr.allEnemies.Count; i++)
        {
            if(!gameMgr.allEnemies[i].GetComponent<EnemyAIMachine>().defender)
                gameMgr.allEnemies[i].GetComponent<EnemyAIMachine>().MoveToObjective();
        }
    }
}
