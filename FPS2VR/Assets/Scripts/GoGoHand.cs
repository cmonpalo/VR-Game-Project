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

    public GameObject currentWandObject;
    private Vector3 initialHandPosition;
    private GameObject selectedObject; // The currently selected object
    private GameObject highlightedObject; // The currently highlighted object

    // Store the original color for reverting after deselecting
    private Color originalColor;

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
                // Highlight object when pointing, but don't highlight the selected object
                if (hit.collider.gameObject != selectedObject)
                {
                    ClearHighlight(); // Clear previous highlight if a new object is detected
                    HighlightObject(hit.collider.gameObject); // Highlight the new object
                }
                selectedObject = hit.collider.gameObject;
            }
        }
        else
        {
            ClearHighlight(); // Clear highlight when no object is detected
            selectedObject = null;
        }
    }

    // Adds a highlight effect to the object
    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Save original color if this object hasn't been highlighted yet
            if (highlightedObject == null || highlightedObject != obj)
            {
                originalColor = renderer.material.color;
            }

            renderer.material.color = Color.yellow; // Change color to highlight
            highlightedObject = obj;
        }
    }

    // Clears the highlight effect from the object
    void ClearHighlight()
    {
        if (highlightedObject != null)
        {
            Renderer renderer = highlightedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColor; // Reset color to original
            }
            highlightedObject = null;
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
            if (currentWandObject != null)
            {
                ReleaseObject(currentWandObject);
            }
        }
    }

    // Grasp (parent) the object to the hand
    void GraspObject(GameObject obj)
    {
        ClearHighlight(); // Clear the highlight once object is selected
        currentWandObject = null;
        Debug.Log("Grasping: " + obj.name);
        obj.transform.SetParent(handAnchor); // Parent to the hand
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            currentWandObject = obj;
            rb.isKinematic = true; // Disable physics while holding
        }

        // Check if the object has the Wand component and call Grasp
        BigWand wand = obj.GetComponent<BigWand>();
        if (wand != null)
        {
            obj.GetComponent<BigWand>().wandObject = obj;
            wand.Grasp(handAnchor); // Call the Grasp method on the Wand script
        }
        StandardWand wand1 = obj.GetComponent<StandardWand>();
        if (wand1 != null)
        {
            currentWandObject = obj;
            wand1.Grasp(handAnchor); // Call the Grasp method on the Wand script
        }
        SneezeWand wand2 = obj.GetComponent<SneezeWand>();
        if (wand2 != null)
        {
            currentWandObject = obj;
            wand2.Grasp(handAnchor); // Call the Grasp method on the Wand script
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
        if (obj.CompareTag("Throwable")) {
            rb.velocity = centerEyeAnchor.rotation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        }

        // Check if the object has the Wand component and call Release
        BigWand wand = obj.GetComponent<BigWand>();
        if (wand != null)
        {
            wand.Release(); // Call the Release method on the Wand script
        }
        StandardWand wand1 = obj.GetComponent<StandardWand>();
        if (wand1 != null)
        {
            wand1.Release(); // Call the Release method on the Wand script
        }
        SneezeWand wand2 = obj.GetComponent<SneezeWand>();
        if (wand2 != null)
        {
            wand2.Release(); // Call the Release method on the Wand script
        }

        // Reset color when released
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = originalColor;
        }

        currentWandObject = null; // Clear current object
    }
}