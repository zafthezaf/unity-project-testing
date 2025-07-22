using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    public float kickbackDistance = 0.05f;
    public float rotationAmount = 5f;

    public float returnSpeed = 10f;
    public float rotationReturnSpeed = 20f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 recoilVelocity;
    private float currentRecoilRotation;
    private float rotationVelocity;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Smoothly move back to original position
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, initialPosition, ref recoilVelocity, 1f / returnSpeed);

        // Smoothly rotate back
        currentRecoilRotation = Mathf.SmoothDamp(currentRecoilRotation, 0f, ref rotationVelocity, 1f / rotationReturnSpeed);
        transform.localRotation = initialRotation * Quaternion.Euler(currentRecoilRotation, 0f, 0f);
    }

    public void Recoil()
    {
        // Apply backward movement
        transform.localPosition -= transform.forward * kickbackDistance;

        // Apply upward rotation
        currentRecoilRotation += rotationAmount;
    }
}
