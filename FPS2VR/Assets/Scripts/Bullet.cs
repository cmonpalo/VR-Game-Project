using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   private void OnCollisionEnter(Collision collision)
   {
    if (collision.gameObject.CompareTag("Target"))
    {
        print("hit" + collision.gameObject.name + " !");
        Destroy(gameObject);
        collision.gameObject.active = false;
        Invoke(nameof(show), 2f);  
    }
   }
    private void show(GameObject objectGO)
    {
        objectGO.active = true;
        
    }
}
