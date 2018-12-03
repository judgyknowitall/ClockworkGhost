using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxDetector : MonoBehaviour {

    [Header(";)")]
    public bool playerInMe = false;

    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other == player.myHitbox) playerInMe = true;
        else playerInMe = false;
    }
}
