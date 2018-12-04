using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSensor : MonoBehaviour {

    public GameObject floor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(FindObjectOfType<Player>().gameObject);
            floor.SetActive(true);
            floor.GetComponent<Ender>().End();
        }
	}
}
