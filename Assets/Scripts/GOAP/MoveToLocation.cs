using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocation : Goal {

	Vector2 targetPosition;

	public override bool isAchieved{
		get{
			Debug.Log("[IN GETTER]@" + data.self.position + " going to: " + targetPosition + "");
			Debug.Log(Vector2.Distance((Vector2)data.self.position, targetPosition) <= 1f / 16f);
			return Vector2.Distance((Vector2)data.self.position, targetPosition) <= 1f / 16f;
		}
	}

	public override Vector2 Do(){
		Debug.Log("[IN DO]@" + data.self.position + " going to: " + targetPosition);
		var direction = (targetPosition - (Vector2)data.self.position).normalized;
		return direction;
	}

	public MoveToLocation(GoalData data, Vector2 tp): base(data){
		Debug.Log("[IN CTOR]@" + data.self.position + " going to: " + targetPosition);
		targetPosition = tp;
	}



}	
