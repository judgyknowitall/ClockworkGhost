using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class NextLevel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player") return;
        var b = FindObjectOfType<LevelManager>().NextLevel();
        if (!b) { SceneManager.LoadScene("FinalLevel"); return; }
        FindObjectOfType<Player>().transform.position = Vector2.zero;
    }
}
