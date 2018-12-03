using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveToHallway : Goal {

	private Vector2 targetPosition;
	private MoveToLocation achievedPositionChecker;

	public override bool isAchieved{
		get {
			return achievedPositionChecker.isAchieved;
		}
	}

	/*public override Vector2 Do(){
		data.newDependenciesExist = true;
		return base.Do();
	}*/

	public MoveToHallway(GoalData data): base(data){
		LevelManager.GraphDirections direction = LevelManager.GraphDirections.UP;

		for (var i = 0; i < data.currentRoom.room.walls.GetLength(0); i++){
			for (var j = 0; j < data.currentRoom.room.walls.GetLength(1); j++){
				if (data.currentRoom.room.walls[i,j] == null){
					if (j == 0) {
						var prevTile = data.currentRoom.room.walls[i,j+1];
						direction = (LevelManager.GraphDirections)i;
						LevelManager.GraphDirections side = direction.orthoganal()[0];
						targetPosition = (Vector2)prevTile.transform.position + side.ToVector2() + direction.opposite().ToVector2();
						goto End;
					}
					else{
						var prevTile = data.currentRoom.room.walls[i,j-1];
						direction = (LevelManager.GraphDirections)i;
						LevelManager.GraphDirections side = direction.orthoganal()[1];
						targetPosition = (Vector2)prevTile.transform.position + side.ToVector2() + direction.opposite().ToVector2();
						goto End;
					}
				}
			}
		}

		End:
			data.hallWayDirection = direction;
			achievedPositionChecker = new MoveToLocation(data, targetPosition);
			dependencies.Add(achievedPositionChecker);
			data.newDependenciesExist = true;
	}
}
