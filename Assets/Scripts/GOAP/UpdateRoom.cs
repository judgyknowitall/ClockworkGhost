using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRoom : Goal {

	bool isDone = false;

    public UpdateRoom(GoalData data) : base(data)
    {
    }

    public override bool isAchieved{
		get{
			return isDone;
		}
	}

	public override Vector2 Do(){
		data.exploredRooms.Add(data.currentRoom);
		switch(data.hallWayDirection){
			case LevelManager.GraphDirections.UP:
				data.currentRoom = data.currentRoom.up;
				break;
			case LevelManager.GraphDirections.DOWN:
				data.currentRoom = data.currentRoom.down;
				break;
			case LevelManager.GraphDirections.LEFT:
				data.currentRoom = data.currentRoom.left;
				break;
			case LevelManager.GraphDirections.RIGHT:
				data.currentRoom = data.currentRoom.right;
				break;
		}
		isDone = true;
		return Vector2.zero;
	}
}
