using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionGetToolTip : MonoBehaviour
{
    private MouseToolTip[] mouseToolTips;
    private MouseToolTip toolTip;

    private void Awake()
    {
        mouseToolTips = Resources.FindObjectsOfTypeAll<MouseToolTip>();
        toolTip = mouseToolTips[0];
    }

    public void ShowInfo(string info)
    {
        toolTip.ShowToolTip(info);
    }

    public void HideToolTipInfo()
    {
        toolTip.HideToolTip();
    }
}
