using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Killed : MonoBehaviour
{
    TextMeshProUGUI pro;

    void Start ()
    {
        pro = GetComponent<TextMeshProUGUI>();
        var player = FindObjectOfType<Player>();

        player.OnDeath += YOUDIED;
    }

    private void YOUDIED()
    {
        pro.enabled=true;
    }
}
