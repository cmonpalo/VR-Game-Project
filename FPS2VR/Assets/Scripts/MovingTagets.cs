using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTagets : MonoBehaviour
{
    [SerializeField]
    Transform[] waypoints;
    int currentwaypoint = 0;
    Rigidbody rigidb;
    [SerializeField]
    float moveSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        rigidb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
       if(Vector3.Distance(transform.position, waypoints[currentwaypoint].position)<.25f) 
       {
        currentwaypoint +=1;
        currentwaypoint = currentwaypoint % waypoints.Length;
       }
       Vector3 _dir =(waypoints[currentwaypoint].position - transform.position).normalized;
       rigidb.MovePosition(transform.position+ _dir * moveSpeed * Time.deltaTime);
    }
}
