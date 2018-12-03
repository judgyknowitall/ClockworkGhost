using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Black : MonoBehaviour
{
    public void Start()
    {
        var color = new Color();
        ColorUtility.TryParseHtmlString("#323138", out color);
        GetComponent<Camera>().backgroundColor = color;
    }
}
