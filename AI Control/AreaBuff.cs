using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBuff : MonoBehaviour
{
    public GameObject grenadeObject;

    private void Awake()
    {
        StartCoroutine(SelfDestruct());
    }
    private void Start()
    {
        Destroy(grenadeObject);
    }

    private void OnTriggerEnter(Collider other) //when the bot enters the buff area, activate the buff
    {
        var target = other.gameObject;

        if(target.tag == "Target")
        {
            Debug.Log("Sent buff to " + target.gameObject);
            StartCoroutine(target.GetComponent<BotStats>().SupportBuff());
        }
    }

    //private void OnTriggerStay(Collider other) //if the bot stays in the buff area, keep reactivating the buff
    //{
    //    var target = other.gameObject;

    //    if (target.tag == "Target")
    //    {
    //        target.GetComponent<BotStats>().SupportBuff();
    //    }
    //}

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }
}
