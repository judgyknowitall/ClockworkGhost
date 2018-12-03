using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkDownHallway : Goal {
	MoveToLocation achievementTracker;

	public override bool isAchieved{
		get{
			return achievementTracker.isAchieved;
		}
	}

	public WalkDownHallway(GoalData data) : base(data){
		var tp = (Vector2)data.self.position + data.hallWayDirection.ToVector2() * (data.levelManager.separation / 2);
		achievementTracker = new MoveToLocation(data, tp);
		dependencies.Add(achievementTracker);
	}
}
