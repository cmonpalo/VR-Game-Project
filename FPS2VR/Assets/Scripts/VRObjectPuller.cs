using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRObjectPuller : MonoBehaviour
{
  
    public Transform rayOrigin;
    public float rayDistance = 10f;
    public Material highlightMaterial;
    private Material originalMaterial;
    private GameObject selectedObject;
    public Transform handTransform; // Target hand where object should move to
    public float pullSpeed = 5f;

    void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        // Raycast to detect selectable objects
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Selectable"))
            {
                if (selectedObject != hitObject)
                {
                    ResetHighlight();
                    selectedObject = hitObject;

                    // Highlight the selected object
                    Renderer renderer = hitObject.GetComponent<Renderer>();
                    originalMaterial = renderer.material;
                    renderer.material = highlightMaterial;
                }
            }

            // Check for input to pull the object to the hand
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            {
                StartCoroutine(PullObjectToHand(hitObject));
            }
        }
        else
        {
            ResetHighlight();
        }
    }

    void ResetHighlight()
    {
        if (selectedObject != null)
        {
            Renderer renderer = selectedObject.GetComponent<Renderer>();
            renderer.material = originalMaterial;
            selectedObject = null;
        }
    }

    IEnumerator PullObjectToHand(GameObject obj)
    {
        while (Vector3.Distance(obj.transform.position, handTransform.position) > 0.1f)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, handTransform.position, pullSpeed * Time.deltaTime);
            yield return null;
        }

        // Once it's close enough, snap it to the hand's position
        obj.transform.position = handTransform.position;
    }
}