using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowState : MonoBehaviour
{
    private TextMeshProUGUI stateText;
    private AIMachine botTarget;

    void Start()
    {
        stateText = GetComponent<TextMeshProUGUI>();

        //if the text objects name contains the color of the bot, set the text equal to the state of that bot
        if(stateText.gameObject.name.Contains("Blue"))
            botTarget = GameObject.Find("AISphere Blue").GetComponent<AIMachine>();
        if (stateText.gameObject.name.Contains("Green"))
            botTarget = GameObject.Find("AISphere Green").GetComponent<AIMachine>();
        if (stateText.gameObject.name.Contains("Orange"))
            botTarget = GameObject.Find("AISphere Orange").GetComponent<AIMachine>();
    }

    void Update()
    {
        stateText.text = botTarget.botMachine.currentState.ToString();

        for(int i = 1; i < stateText.text.Length; i++) //remove text after the second capital letter in the name of the state
        {
            char targetChar = stateText.text[i];

            if (char.IsUpper(targetChar))
                stateText.text = stateText.text.Remove(i);
        }

        if (!botTarget.gameObject.activeSelf) //if the bot is dead set the state text to nothing
            stateText.text = "";
    }
}
