using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField]
    public Vector2 direction;

    void Start()
    {

    }

    void FixedUpdate()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionVector = mousePosition - transform.position;
        Vector2 directionVectorNorm = direction.normalized;
        direction = directionVector;


        var quat = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
        var quatVec = quat.eulerAngles;
        quatVec = quatVec.Multiply(Vector3.forward);

        transform.rotation = Quaternion.Euler(quatVec);
    }
}
