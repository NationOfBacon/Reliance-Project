using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayLog : MonoBehaviour
{
    public GameObject textPrefab;
    private List<GameObject> listCount = new List<GameObject>();
    Color32 orange = new Color32(255, 134, 51, 255);

    private void Update()
    {
        if(listCount.Count > 20)
        {
            for (int i = 0; i < listCount.Count; i++)
            {
                if(listCount[i] != null)
                {
                    Destroy(listCount[i]);
                    listCount.Remove(listCount[i]);
                    break;
                }
            }
        }
    }


    public void RecieveLog(string displayText, GameObject sentFromObject) //used to display things that bots are saying or just to display text
    {
        var objectCopy = Instantiate(textPrefab as GameObject, transform);
        objectCopy.GetComponent<TextMeshProUGUI>().text = displayText;

        if (sentFromObject.name.Contains("Blue"))
            objectCopy.GetComponent<TextMeshProUGUI>().color = Color.blue ;
        if (sentFromObject.name.Contains("Green"))
            objectCopy.GetComponent<TextMeshProUGUI>().color = Color.green;
        if (sentFromObject.name.Contains("Orange"))
            objectCopy.GetComponent<TextMeshProUGUI>().color = orange;

        listCount.Add(objectCopy); //list controls how many items are able to be displayed in the log at a time
    }

    public void RecieveLog(string displayText)
    {
        var objectCopy = Instantiate(textPrefab as GameObject, transform);
        objectCopy.GetComponent<TextMeshProUGUI>().text = displayText;

        listCount.Add(objectCopy);
    }

    public void RecieveLog(string displayText, Color textColor)
    {
        var objectCopy = Instantiate(textPrefab as GameObject, transform);
        objectCopy.GetComponent<TextMeshProUGUI>().text = displayText;
        objectCopy.GetComponent<TextMeshProUGUI>().color = textColor;

        listCount.Add(objectCopy);
    }
}
