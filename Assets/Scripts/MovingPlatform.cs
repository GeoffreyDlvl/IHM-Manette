using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField]
    float speed = 5f;

    [SerializeField]
    List<Transform> waypoints;
    int waypointIndex = 0;

    void Update()
    {
        Move();   
    }

    private void Move()
    {
        if (waypointIndex < waypoints.Count)
        {
            var targetPosition = waypoints[waypointIndex].position;
            var movementThisFrame = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            print(Vector2.Distance(transform.position, targetPosition));
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                waypointIndex++;
            }
        }
        else
        {
            waypointIndex = 0;
        }
    }
}
