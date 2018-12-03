using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killed : MonoBehaviour
{
    TextMeshProUGUI pro;
    float dead = Mathf.Infinity;

    void Start ()
    {
        pro = GetComponent<TextMeshProUGUI>();
        var player = FindObjectOfType<Player>();

        player.OnDeath += YOUDIED;
    }

    void Update()
    {
        if (Time.time > dead)
            SceneManager.LoadScene("Menu");
    }

    private void YOUDIED()
    {

        pro.enabled=true;
        dead = Time.time + 10f;
    }
}
