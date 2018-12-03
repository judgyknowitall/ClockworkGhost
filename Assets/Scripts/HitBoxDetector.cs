using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxDetector : MonoBehaviour {

    [Header(";)")]
    public bool playerInMe = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player") playerInMe = true;
        else playerInMe = false;
    }
}
