using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

public class RaycastSelector : MonoBehaviour
{
    public Camera playerCamera;       // Reference to the player's camera
    public float rayDistance = 10f;   // Max distance for raycasting
    public Material highlightMaterial;
    private Material originalMaterial;
    private GameObject selectedObject;

    public float pullSpeed = 2f;      // Speed to pull object towards the camera

    void Update()
    {
        // Cast a ray from the camera's position through the mouse cursor
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform raycast and check if it hits an object
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            // If the object has the "Selectable" tag, proceed
            if (hitObject.CompareTag("Selectable"))
            {
                // If a new object is selected, reset the previous one
                if (selectedObject != hitObject)
                {
                    ResetHighlight();
                    selectedObject = hitObject;

                    // Apply the highlight material to the object
                    Renderer renderer = hitObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        originalMaterial = renderer.material;
                        renderer.material = highlightMaterial;
                    }
                }

                // When the left mouse button is clicked, start pulling the object
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Mouse clicked, pulling object towards the camera");
                    StartCoroutine(PullObjectToCamera(hitObject));
                }
            }
        }
        else
        {
            ResetHighlight();
        }
    }

    void ResetHighlight()
    {
        // Restore the original material if an object was highlighted
        if (selectedObject != null)
        {
            Renderer renderer = selectedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterial;
            }
            selectedObject = null;
        }
    }

    IEnumerator PullObjectToCamera(GameObject obj)
    {
        Debug.Log("Starting to pull object");

        // Ensure we have a valid target
        if (obj == null)
        {
            Debug.LogError("Object is null!");
            yield break;
        }

        // Pull object towards the camera's position over time
        while (Vector3.Distance(obj.transform.position, playerCamera.transform.position) > 0.5f)
        {
            // Log the current distance to monitor pulling progress
            float distance = Vector3.Distance(obj.transform.position, playerCamera.transform.position);
            Debug.Log("Distance to camera: " + distance);

            // Smoothly move the object towards the camera's position
            obj.transform.position = Vector3.Lerp(obj.transform.position, playerCamera.transform.position, pullSpeed * Time.deltaTime);

            yield return null;
        }

        Debug.Log("Object reached camera");
    }
}