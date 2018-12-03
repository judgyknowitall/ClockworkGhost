using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Goal{
	abstract public bool isAchieved{ get; }

	abstract public Vector2 Do();

	public List<Goal> dependencies;

	public GoalData data;
}