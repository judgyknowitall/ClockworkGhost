using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : Goal
{
    public AttackPlayer(GoalData data) : base(data)
    {
    }

    public override bool isAchieved {get{ throw new System.NotImplementedException(); }}

    public override Vector2 Do()
    {
        throw new System.NotImplementedException();
    }
}
