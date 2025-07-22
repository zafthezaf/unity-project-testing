using UnityEngine;
using TMPro;

public class SemiAutoPistol : MonoBehaviour
{
    [Header("Ammo Settings")]
    public int magazineSize = 17;
    public int ammoInMagazine;
    public int reserveAmmo = 60;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    [Header("Input Settings")]
    public KeyCode reloadKey = KeyCode.R;

    [Header("Gun Settings")]
    public float damage = 30f;
    public float range = 100f;
    public float fireRate = 5f;

    [Header("References")]
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public AudioSource audioSource;
    public GunRecoil recoilScript;

    [Header("Sound FX")]
    public AudioClip reloadSound;
    public AudioClip[] emptyClickSounds;

    [Header("UI")]
    public TextMeshProUGUI ammoText;

    private float nextTimeToFire = 0f;

    void Start()
    {
        ammoInMagazine = magazineSize;
    }

    void Update()
    {
        if (isReloading) return;

        // Fire input
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            if (ammoInMagazine > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
                ammoInMagazine--;
            }
            else
            {
                Debug.Log("Click! Out of ammo");

                if (audioSource != null && emptyClickSounds != null && emptyClickSounds.Length > 0)
                {
                    int index = Random.Range(0, emptyClickSounds.Length);
                    audioSource.PlayOneShot(emptyClickSounds[index]);
                }
            }
        }

        // Reload input
        if (Input.GetKeyDown(reloadKey) && ammoInMagazine < magazineSize && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
        }

        // Update ammo display if not reloading
        if (ammoText != null && !isReloading)
        {
            ammoText.text = ammoInMagazine + " / " + reserveAmmo;
        }
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // Show "RELOADING..." text
        if (ammoText != null)
        {
            ammoText.text = "RELOADING";
        }

        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime);

        int bulletsNeeded = magazineSize - ammoInMagazine;
        int bulletsToReload = Mathf.Min(bulletsNeeded, reserveAmmo);

        ammoInMagazine += bulletsToReload;
        reserveAmmo -= bulletsToReload;

        isReloading = false;

        // Ammo display will auto-update on next frame
        Debug.Log("Reloaded.");
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (audioSource != null)
            audioSource.Play(); // gunshot sound (assigned in AudioSource)

        if (recoilScript != null)
            recoilScript.Recoil();

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
