using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FindPlayer : Goal{
	public float lookDistance = 10;

	override public bool isAchieved{
		get {
			var tmpDir = data.player.transform.position - data.self.position;
			var maybeHit = Physics2D.Raycast(data.self.position, tmpDir, data.lookDistance, 1 << 8);
			if (maybeHit.collider != null){
				if (maybeHit.collider.gameObject.tag == "Player") return true;
			}

			return false;
		}
	}

	public FindPlayer(GoalData data){
		this.data = data;
		dependencies = new List();
	}
}
