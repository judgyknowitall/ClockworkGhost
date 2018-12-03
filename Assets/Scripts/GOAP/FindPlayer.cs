using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FindPlayer : Goal{
	public float lookDistance = 10;

	override public bool isAchieved{
		get {
			foreach (var dep in dependencies) {
				if (!dep.isAchieved) return false;
			}

			return true;
		}
	}

	public  FindPlayer(GoalData data): base(data){

		dependencies.Add(new Explore(data));
		//dependencies.Add(new CheckForPlayer(data));
	}
}
