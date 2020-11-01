using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatefrom : MonoBehaviour
{
    [SerializeField, Tooltip("Maximum moving range according x axis,  Ex: 5, plateform will fo from -5 to 5")]
    private float maxMovingRange = 5;

    [SerializeField, Tooltip("Maximum speed in u/s.")]
    private float speed = 1;

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
        transform.position = new Vector3(transform.position.x + ((int)currentDirection * speed *Time.deltaTime), transform.position.y);
    } 

    private Boolean IsOutOfUpperBound()
    {
        return gameObject.transform.position.x >= initialTransform.x + speed;
    }

    private Boolean IsOutOfLowerBound()
    {
        return gameObject.transform.position.x <= initialTransform.x - speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.transform.parent = null;
    }
}
