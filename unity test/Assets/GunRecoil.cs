using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [Header("Position Recoil")]
    public float kickbackDistance = 0.05f;
    public float returnSpeed = 20f;

    [Header("Rotation Recoil")]
    public float rotationAmount = 5f;
    public float rotationReturnSpeed = 25f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 recoilVelocity;
    private float currentRecoilRotation;
    private float recoilRotationVelocity;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Smoothly return position
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, initialPosition, ref recoilVelocity, 1f / returnSpeed);

        // Smoothly return rotation
        currentRecoilRotation = Mathf.SmoothDamp(currentRecoilRotation, 0f, ref recoilRotationVelocity, 1f / rotationReturnSpeed);
        transform.localRotation = initialRotation * Quaternion.Euler(-currentRecoilRotation, 0f, 0f);
    }

    public void Recoil()
    {
        // Add backward kick
        transform.localPosition -= transform.forward * kickbackDistance;

        // Add upward tilt
        currentRecoilRotation += rotationAmount;
    }
}
