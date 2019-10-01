using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformantStat : MonoBehaviour
{
    public int radius;
    private int modifiedRadius;
    List<GameObject> bots = new List<GameObject>();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetRadius(int radiusIncrease)
    {
        gameObject.SetActive(true);
        modifiedRadius = radius + radiusIncrease;
        GetComponent<SphereCollider>().radius = modifiedRadius;
    }

    private void Update()
    {
        if(bots.Count > 0)
        {
            for (int i = 0; i < bots.Count; i++)
            {
                if (!bots[i].activeSelf)
                {
                    bots.RemoveAt(i);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var colName = other.gameObject.name;

        if(colName == "EnemyBot(Clone)" || colName == "Turret Base(Clone)" || colName == "Enemy Structure(Clone)")
        {
            bots.Add(other.gameObject);
            other.gameObject.GetComponent<Health>().enemyHealthCanvas.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var colName = other.gameObject.name;

        if (colName == "EnemyBot(Clone)" || colName == "Turret Base(Clone)" || colName == "Enemy Structure(Clone)")
        {
            GameObject exitingBot = other.gameObject;

            for(int i = 0; i < bots.Count; i++)
            {
                if(exitingBot == bots[i])
                {
                    other.gameObject.GetComponent<Health>().enemyHealthCanvas.enabled = false;
                    bots.RemoveAt(i);
                }
            }
        }
    }
}
