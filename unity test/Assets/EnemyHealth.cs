using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Vector3 pendingImpactDirection;
    private float pendingImpactForce;

    public float maxHealth = 100f;
    private float currentHealth;

    private Rigidbody rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();

        // Lock rotation so the enemy stands upright
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void TakeDamage(float amount, Vector3 hitDirection, float impactForce)
{
    currentHealth -= amount;
    Debug.Log($"{gameObject.name} took {amount} damage");

    if (currentHealth <= 0f)
    {
        // Store impact to apply *after* unlocking ragdoll
        pendingImpactDirection = hitDirection;
        pendingImpactForce = impactForce;

        Die();
    }
    else
    {
        // Apply knockback for normal hits
        rb.AddForce(hitDirection.normalized * impactForce, ForceMode.Impulse);
    }
}


    void Die()
{
    Debug.Log($"{gameObject.name} died!");

    // Unlock ragdoll-style fall
    rb.constraints = RigidbodyConstraints.None;

    // Apply final impact AFTER going limp
    rb.AddForce(pendingImpactDirection.normalized * pendingImpactForce, ForceMode.Impulse);

    // Optional: add spin for style
    rb.AddTorque(Random.insideUnitSphere * 1.5f, ForceMode.Impulse);

    enabled = false;
}

}

