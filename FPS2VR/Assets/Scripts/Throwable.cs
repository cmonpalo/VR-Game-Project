using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{

    public Transform handAnchor;
    private bool isHeld = false;
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
    }


}
