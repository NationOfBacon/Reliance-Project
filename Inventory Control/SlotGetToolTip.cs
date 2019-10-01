using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotGetToolTip : MonoBehaviour
{
    private MouseToolTip[] mouseToolTips;
    private MouseToolTip toolTip;
    private Slot thisSlot;

    private void Awake()
    {
        mouseToolTips = Resources.FindObjectsOfTypeAll<MouseToolTip>();
        toolTip = mouseToolTips[0];
        thisSlot = transform.parent.GetComponent<Slot>();
    }

    public void ShowToolTipInfo()
    {
        toolTip.ShowSlotInfo(thisSlot);
    }

    public void HideToolTipInfo()
    {
        toolTip.HideToolTip();
    }
}

