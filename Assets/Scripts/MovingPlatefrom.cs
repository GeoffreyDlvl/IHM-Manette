using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatefrom : MonoBehaviour
{
    [SerializeField]
    private float maxMovingDistance = 5;
    
    [SerializeField]
    private float a;

    [SerializeField]
    private float b;

    private Vector3 initialTransform;

    enum Direction
    {
        FORWARD = 1,
        BACKWARD = -1
    }

    Direction currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        initialTransform = gameObject.transform.position;
        currentDirection = Direction.FORWARD;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOutOfUpperBound())
        {
            currentDirection = Direction.BACKWARD;
        } else if (IsOutOfLowerBound())
        {
            currentDirection = Direction.FORWARD;
        }
        Move();
    }
    
    private void Move()
    {
        transform.position = new Vector3(transform.position.x + ((int)currentDirection * Time.deltaTime), transform.position.y);
    } 

    private Boolean IsOutOfUpperBound()
    {
        return gameObject.transform.position.x >= initialTransform.x + maxMovingDistance;
    }

    private Boolean IsOutOfLowerBound()
    {
        return gameObject.transform.position.x <= initialTransform.x - maxMovingDistance;
    }
}
