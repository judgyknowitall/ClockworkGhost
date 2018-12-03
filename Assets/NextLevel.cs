﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NextLevel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player") return;
        FindObjectOfType<LevelManager>().NextLevel();
        FindObjectOfType<Player>().transform.position = Vector2.zero;
    }
}
