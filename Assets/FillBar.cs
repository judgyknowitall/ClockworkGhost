using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class FillBar : MonoBehaviour
{
    public float minEther = 0;
    public float maxEther;

    public int fillBarIndex;

    private Player player;
    private RectTransform rectTransform;

    [SerializeField] private GameObject fill;

    float maxScale;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        maxScale = rectTransform.localScale.x;
    }

    void FixedUpdate()
    {
        var percent = Mathf.InverseLerp(minEther, maxEther, player.ether);
        percent = percent * 3;
        var myFillPercent = Mathf.Clamp(percent- fillBarIndex, 0, 1);
        if (myFillPercent <= 0)
        {
            fill.SetActive(false);
            return;
        }
        rectTransform.localScale = new Vector3(Mathf.Lerp(0, maxScale, myFillPercent), transform.localScale.y, transform.localScale.z);
        fill.SetActive(true);
    }
}
