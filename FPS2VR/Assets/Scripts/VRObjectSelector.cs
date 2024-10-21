using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRObjectSelector : MonoBehaviour
{
    public Transform rayOrigin;
    public float rayDistance = 10f;
    public Material highlightMaterial;
    private Material originalMaterial;
    private GameObject selectedObject;

    void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Selectable"))
            {
                // If a new object is hit, reset the previous one
                if (selectedObject != hitObject)
                {
                    ResetHighlight();
                    selectedObject = hitObject;

                    // Save the original material and change to highlight material
                    Renderer renderer = hitObject.GetComponent<Renderer>();
                    originalMaterial = renderer.material;
                    renderer.material = highlightMaterial;
                }
            }
        }
        else
        {
            // If no object is hit, reset the highlight
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
}
