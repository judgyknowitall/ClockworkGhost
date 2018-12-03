using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : Goal
{
    public AttackPlayer(GoalData data) : base(data)
    {
    }

    public override bool isAchieved {get{ return true; }}

    public override Vector2 Do()
    {
        return Vector2.zero;
    }
}
