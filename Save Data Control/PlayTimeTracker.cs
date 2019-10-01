using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayTimeTracker
{
    public static float GetPlayTime(float savedTime)
    {
        float playTime = 0;

        playTime = Time.realtimeSinceStartup;

        return playTime;
    }
}
