using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneezeWand : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public float maxSpread = 8; //For sneezeWand
    public enum WeaponModel
    {
        BigWand,
        Wand
    }

    public WeaponModel thisWeaponModel;

    private bool isHeld = false;  // Check if the weapon is currently being held
    public Transform handAnchor;  // Reference to the VR hand anchor (set during grasp)

    // Update is called once per frame
    void Update()
    {
        // Only allow shooting if the BigWand is being held
        if (isHeld && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            FireWeapon();
        }
    }

    // Call this when the object is grasped (Go-Go hand technique)
    public void Grasp(Transform handAnchor)
    {
        this.handAnchor = handAnchor;
        isHeld = true;
    }

    // Call this when the object is released
    public void Release()
    {
        isHeld = false;
        handAnchor = null;
    }

    private void FireWeapon()
    {
        if (bulletPrefab == null || handAnchor == null) return;

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        // Shoot the bullet in the forward direction of the hand anchor
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
           
            // Vector3 dir = transform.forward + new Vector3(Random.Range(-8, 8), Random.Range(-8,8), Random.Range(-8, 8));
                rb.AddForce(transform.up.normalized * 10 + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5)), ForceMode.Impulse);    
            GetComponent<AudioSource>().Play();   
        }

        // Destroy the bullet after some time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
