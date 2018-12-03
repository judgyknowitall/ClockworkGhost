using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NextLevel : MonoBehaviour
{
    [SerializeField] private LevelManager lm;

    void OnTriggerEnter2D(Collider2D col)
    {
        lm.NextLevel();
    }
}
