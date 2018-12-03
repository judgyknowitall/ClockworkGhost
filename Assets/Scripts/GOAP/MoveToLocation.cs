using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocation : Goal {

	Vector2 targetPosition;

	public override bool isAchieved{
		get{
			return Vector2.Distance((Vector2)data.self.position, targetPosition) < 1f / 16f;
		}
	}

	public override Vector2 Do(){
		var direction = (targetPosition - (Vector2)data.self.position).normalized;
		return direction;
	}

	public MoveToLocation(GoalData data, Vector2 tp): base(data){
		targetPosition = tp;
	}



}	
