using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MouseTools;

public class MouseCommand : MonoBehaviour //used to select bots for commands and to order bots around
{
    private CustomCursor cursor;

    //private bool setSearch = false;

    private void Awake()
    {
        cursor = GetComponent<CustomCursor>();
    }

    private void Update()
    {
        if(cursor.botSelected && cursor.overEnemy && Input.GetKeyDown(KeyCode.Mouse1))
        {
            SetToAttack();
        }

        if(cursor.botSelected && cursor.overBot && cursor.leadBot && Input.GetKeyDown(KeyCode.Mouse1))
        {
            SetToFollowLeader();
        }

        //if(cursor.botSelected && cursor.overBot && Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    if (cursor.blueBot && cursor.selectedBot.displayName.Contains("Blue"))
        //        SetToSearchState();
        //    else if (cursor.greenBot && cursor.selectedBot.displayName.Contains("Green"))
        //        SetToSearchState();
        //    else if (cursor.orangeBot && cursor.selectedBot.displayName.Contains("Orange"))
        //        SetToSearchState();
        //}
    }

    public void SetToAttack()
    {
        cursor.selectedBot.enemyObject = cursor.targetEnemy;
        cursor.selectedBot.canSeeEnemy = true;
        cursor.selectedBot.botMachine.ChangeState(ChaseState.instance);
        Debug.Log("Gave attack order to: " + cursor.selectedBot.displayName);

        cursor.ResetValues();
    }

    public void SetToFollowLeader()
    {
        cursor.selectedBot.botMachine.ChangeState(FollowState.instance);
        Debug.Log("Gave follow order to: " + cursor.selectedBot.displayName);

        cursor.ResetValues();
    }

    //public void SetToSearchState()
    //{
    //    cursor.selectedBot.botMachine.ChangeState(SearchState.instance);
    //    Debug.Log("Gave follow order to: " + cursor.selectedBot.displayName);

    //    cursor.ResetValues();
    //}
}
