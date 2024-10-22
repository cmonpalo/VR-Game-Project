using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWand : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    
    public GameObject wandObject;

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
            print(wandObject);
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

         rb.AddForce(transform.up.normalized * bulletVelocity, ForceMode.Impulse);


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
