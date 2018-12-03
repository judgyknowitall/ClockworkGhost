using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalData{
	public Transform self;
	public Player player;

	public bool playerInRoom;
	public LevelManager.Node currentRoom;
	
	public HashSet<LevelManager.Node> exploredRooms = new HashSet<LevelManager.Node>();

	public bool newDependenciesExist;
	public bool inHallway;
	public LevelManager.GraphDirections hallWayDirection;
	public LevelManager levelManager;

}
