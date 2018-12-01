using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public List<LevelDescriptor> levels;

	IEnumerator<LevelDescriptor> levelEnumerator;

	// Use this for initialization
	void Start () {
		levelEnumerator = levels.GetEnumerator();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[System.Serializable]
	public struct LevelDescriptor{
		uint minDifficulty;
		uint maxDifficulty;
		uint length;
		uint complexity;
		Lore lore;
		List<Spawner> spawners; 
	}
}
