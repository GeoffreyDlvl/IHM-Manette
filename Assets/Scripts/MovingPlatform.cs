using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.childCount == 0)
        {
            GameObject Empty_1x1 = new GameObject();
            Empty_1x1.name = "Empty_1x1";
            Empty_1x1.transform.parent = transform;
            other.gameObject.transform.SetParent(Empty_1x1.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController2D playerController = GetComponentInChildren<PlayerController2D>();
        Transform playerTransform = playerController.transform;
        playerTransform.SetParent(null);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}
