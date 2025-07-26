using UnityEngine;

public class GunPhysicsFollower : MonoBehaviour
{
    public Transform followTarget; // Drag in "Gun Holder" here
    public float positionSpring = 500f;
    public float positionDamping = 20f;
    public float rotationSpring = 100f;
    public float rotationDamping = 10f;

    private ConfigurableJoint joint;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        SetupJoint();
    }

    void SetupJoint()
    {
        JointDrive posDrive = new JointDrive
        {
            positionSpring = positionSpring,
            positionDamper = positionDamping,
            maximumForce = Mathf.Infinity
        };

        JointDrive rotDrive = new JointDrive
        {
            positionSpring = rotationSpring,
            positionDamper = rotationDamping,
            maximumForce = Mathf.Infinity
        };

        joint.xDrive = posDrive;
        joint.yDrive = posDrive;
        joint.zDrive = posDrive;

        joint.angularXDrive = rotDrive;
        joint.angularYZDrive = rotDrive;
    }

    void FixedUpdate()
    {
        if (followTarget == null) return;

        joint.targetPosition = transform.InverseTransformPoint(followTarget.position);
        joint.targetRotation = Quaternion.Inverse(Quaternion.Inverse(transform.rotation) * followTarget.rotation);
    }
}
