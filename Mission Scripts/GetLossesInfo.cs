using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetLossesInfo : MonoBehaviour
{
    private float leadBotLosses;
    private float blueBotLosses;
    private float greenBotLosses;
    private float orangeBotLosses;
    private float totalLosses;
    public TextMeshProUGUI leadText;
    public TextMeshProUGUI blueText;
    public TextMeshProUGUI greenText;
    public TextMeshProUGUI orangeText;
    public TextMeshProUGUI totalLossText;
    private Slider leadSlider;
    private Slider blueSlider;
    private Slider greenSlider;
    private Slider orangeSlider;

    private InventoryManager invMgr;

    private void Awake()
    {
        leadSlider = GameObject.Find("RunningUI/Horizontal Group Health/My Health/HP Slider").GetComponent<Slider>();
        blueSlider = GameObject.Find("RunningUI/Horizontal Group Health/Blue HP").GetComponent<Slider>();
        greenSlider = GameObject.Find("RunningUI/Horizontal Group Health/Green HP").GetComponent<Slider>();
        orangeSlider = GameObject.Find("RunningUI/Horizontal Group Health/Orange HP").GetComponent<Slider>();
        invMgr = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();
    }
    private void Start()
    {
        leadBotLosses = leadSlider.value - leadSlider.maxValue;
        blueBotLosses = blueSlider.value - blueSlider.maxValue;
        greenBotLosses = greenSlider.value - greenSlider.maxValue;
        orangeBotLosses = orangeSlider.value - orangeSlider.maxValue;
        totalLosses = leadBotLosses + blueBotLosses + greenBotLosses + orangeBotLosses;

        leadText.text = leadBotLosses.ToString();
        if(leadBotLosses < 0)
            leadText.color = Color.red;
        blueText.text = blueBotLosses.ToString();
        if (blueBotLosses < 0)
            blueText.color = Color.red;
        greenText.text = greenBotLosses.ToString();
        if (greenBotLosses < 0)
            greenText.color = Color.red;
        orangeText.text = orangeBotLosses.ToString();
        if (orangeBotLosses < 0)
            orangeText.color = Color.red;
        totalLossText.text = totalLosses.ToString();
        if (totalLosses < 0)
            totalLossText.color = Color.red;

        invMgr.currencyLost = totalLosses;
    }
}
