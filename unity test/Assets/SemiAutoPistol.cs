using UnityEngine;

public class SemiAutoPistol : MonoBehaviour
{
    [Header("Gun Settings")]
    public float damage = 30f;
    public float range = 100f;
    public float fireRate = 5f; // Max 5 shots per second

    [Header("References")]
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            var target = hit.transform.GetComponent<EnemyHealth>();
            if (target != null)
            {
                Vector3 hitDirection = hit.transform.position - fpsCam.transform.position;
                float impactForce = 1.2f;
                target.TakeDamage(damage, hitDirection, impactForce);
            }

            if (impactEffect != null)
            {
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }
    }
}
