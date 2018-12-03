using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : Goal {

	bool playerIsDead = false;
	
	public override bool isAchieved{
		get {
			return playerIsDead;
		}
	}

	public KillPlayer(GoalData data): base(data){
		
		data.player.OnDeath += OnPlayerDeath;

		dependencies.Add(new AttackPlayer(data));
		dependencies.Add(new FindPlayer(data));
	}

	public void OnPlayerDeath(){
		playerIsDead = true;
	}


}
