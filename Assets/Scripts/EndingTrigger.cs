using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    [SerializeField] public GameObject tut;

    void OnTriggerEnter2D()
    {
        tut.SetActive(true);
    }
}
