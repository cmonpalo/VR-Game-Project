using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoGoHand : MonoBehaviour
{
    public Transform handAnchor; // Left or Right Hand Anchor
    public Transform centerEyeAnchor; // Center eye anchor (head position)
    public float maxReachDistance = 0.5f; // Threshold before virtual hand extends
    public float extensionFactor = 2.0f; // How much farther the virtual hand extends
    public LayerMask interactableLayer; // Set this to the layer for interactable objects

    private Vector3 initialHandPosition;
    private GameObject selectedObject; // The currently selected object

    void Start()
    {
        initialHandPosition = handAnchor.localPosition; // Get initial hand position
    }

    void Update()
    {
        UpdateHandPosition();
        RaycastForObjects();
        HandleInput();
    }

    // Extends hand based on distance from the head
    void UpdateHandPosition()
    {
        float distanceFromBody = Vector3.Distance(centerEyeAnchor.position, handAnchor.position);

        if (distanceFromBody > maxReachDistance)
        {
            // Extend hand virtually
            Vector3 handOffset = handAnchor.localPosition - initialHandPosition;
            handAnchor.position = Vector3.Lerp(handAnchor.position, handAnchor.position + handOffset * extensionFactor, 0.1f);
        }
    }

    // Raycast to detect objects in front of the hand
    void RaycastForObjects()
    {
        RaycastHit hit;
        if (Physics.Raycast(handAnchor.position, handAnchor.forward, out hit, Mathf.Infinity, interactableLayer))
        {
            if (hit.collider != null)
            {
                Debug.Log("Hit object: " + hit.collider.name);
                selectedObject = hit.collider.gameObject;
            }
        }
        else
        {
            selectedObject = null; // Clear the selected object if no hit
        }
    }

    // Handles input for grasping and releasing objects
    void HandleInput()
    {
        // Grasp object when hand trigger is pressed
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            if (selectedObject != null)
            {
                GraspObject(selectedObject);
            }
        }

        // Release object when hand trigger is released
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
        {
            if (selectedObject != null)
            {
                ReleaseObject(selectedObject);
            }
        }
    }

    // Grasp (parent) the object to the hand
    void GraspObject(GameObject obj)
    {
        Debug.Log("Grasping: " + obj.name);
        obj.transform.SetParent(handAnchor); // Parent to the hand
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Disable physics while holding
        }

        // Check if the object has the Wand component and call Grasp
        Wand wand = obj.GetComponent<Wand>();
        if (wand != null)
        {
            wand.Grasp(handAnchor); // Call the Grasp method on the Wand script
        }
    }

    // Release (unparent) the object from the hand
    void ReleaseObject(GameObject obj)
    {
        Debug.Log("Releasing: " + obj.name);
        obj.transform.SetParent(null); // Unparent from the hand
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Re-enable physics
            rb.velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch); // Apply velocity for throw
        }

        // Check if the object has the Wand component and call Release
        Wand wand = obj.GetComponent<Wand>();
        if (wand != null)
        {
            wand.Release(); // Call the Release method on the Wand script
        }
    }
}
