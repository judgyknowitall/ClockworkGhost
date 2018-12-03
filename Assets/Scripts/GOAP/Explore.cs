using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explore : Goal {
	public override bool isAchieved{
		get{
			return !data.exploredRooms.Contains(data.currentRoom);
		}
	}

	public override Vector2 Do(){
		if (!data.inHallway){
			dependencies.Add(new WalkDownHallway(data));
			dependencies.Add(new MoveToHallway(data));
		}else{
			dependencies.Add(new UpdateRoom(data));
			dependencies.Add(new WalkDownHallway(data));
		}

		data.newDependenciesExist = true;

		return Vector2.zero;
	}

	public Explore(GoalData data) : base(data){
		dependencies.Add(new WalkDownHallway(data));
		dependencies.Add(new MoveToHallway(data));
	}
}
