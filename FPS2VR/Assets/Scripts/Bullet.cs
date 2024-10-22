using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
   {
    if (collision.gameObject.CompareTag("Target"))
    {
        Debug.Log("hit" + collision.gameObject.name + " !");
        Destroy(gameObject);
        collision.gameObject.GetComponent<AudioSource>().Play();
    }
   }
}
