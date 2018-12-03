using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Goal{
	abstract public bool isAchieved{ get; }

	virtual public Vector2 Do(){
		return Vector2.zero;
	}

	public List<Goal> dependencies;

	public GoalData data;

	public Goal(GoalData data){
		this.data = data;
		dependencies = new List<Goal>();
	}
}