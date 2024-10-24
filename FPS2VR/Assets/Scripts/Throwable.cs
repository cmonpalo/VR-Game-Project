using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{

    public Transform handAnchor;
    private bool isHeld = false;

    public bool armed = false;
    [SerializeField] GameObject Explosion;

    public float bulletPrefabLifeTime = 1f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grasp(Transform handAnchor)
    {
        this.handAnchor = handAnchor;
        isHeld = true;
    }

    public void Release()
    {
        isHeld = false;
        handAnchor = null;
        armed = true;
    }

    private void OnTriggerEnter(Collider collision)
   {
    

        if (armed == true) {
        GameObject kaboom = Instantiate(Explosion);
        Transform explosionTransform = kaboom.GetComponent<Transform>();
        explosionTransform.position = transform.position;
        Destroy(gameObject);
        Destroy(kaboom, 2.0f);
        
        }
   }

    
}
