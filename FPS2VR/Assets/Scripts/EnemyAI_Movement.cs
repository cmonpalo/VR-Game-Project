using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Movement : MonoBehaviour
{
    public Transform pointA;  // First patrol point
    public Transform pointB;  // Second patrol point
    public float speed = 3f;  // Movement speed
    private Transform currentTarget;  // The current target point
    public float stopDistance = 0.1f;  // Distance to consider as "reached"

    private void Start()
    {
        currentTarget = pointA;  // Start by moving towards point A
    }

    private void Update()
    {
        // Move towards the current target (either point A or point B)
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Check if the enemy has reached the current target point
        if (Vector3.Distance(transform.position, currentTarget.position) < stopDistance)
        {
            // Switch to the other point
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }
    }
}