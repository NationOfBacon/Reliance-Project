using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamage : MonoBehaviour
{
    public GameObject floatingDamagePrefab;
    private List<GameObject> floatingDamageObjects = new List<GameObject>();
    private GameObject currentObject;
    public Transform startMarker;
    public Transform endMarker;

    public void SpawnDamageNumber(int damageAmt)
    {
        if(floatingDamageObjects.Count < 10)
        {
            currentObject = (Instantiate(floatingDamagePrefab as GameObject, transform));
            floatingDamageObjects.Add(currentObject);
        }
        else
        {
            for(int i = 0; i < floatingDamageObjects.Count; i++)
            {
                if (!floatingDamageObjects[i].activeSelf)
                {
                    currentObject = floatingDamageObjects[i];
                    currentObject.SetActive(true);
                    currentObject.transform.position = transform.position;
                    currentObject.GetComponent<MoveDamageValue>().ResetPosition();
                    break;
                }
            }
        }

        currentObject.GetComponent<MoveDamageValue>().SetMarkers(startMarker, endMarker);

        currentObject.GetComponent<TextMeshProUGUI>().text = damageAmt.ToString();
        if(damageAmt == 1)
            currentObject.GetComponent<TextMeshProUGUI>().color = Color.red;
        else if (damageAmt == 2)
            currentObject.GetComponent<TextMeshProUGUI>().color = Color.blue;
        else if (damageAmt > 2)
            currentObject.GetComponent<TextMeshProUGUI>().color = Color.yellow;
    }
}
