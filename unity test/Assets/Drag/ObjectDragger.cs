using UnityEngine;

public class ObjectDragger : MonoBehaviour
{
    public float maxDistance = 5f;
    public float dragSpeed = 10f;
    public LayerMask dragLayerMask;

    private Camera cam;
    private Rigidbody draggedRb;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TryStartDrag();

        if (Input.GetKeyUp(KeyCode.E))
            StopDrag();
    }

    void FixedUpdate()
    {
        if (draggedRb != null)
        {
            Vector3 targetPos = cam.transform.position + cam.transform.forward * maxDistance;
            Vector3 direction = (targetPos - draggedRb.position);
            draggedRb.linearVelocity = direction * dragSpeed;
        }
    }

void TryStartDrag()
{
    Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, dragLayerMask))
    {
        if (hit.collider.GetComponent<Draggable>() != null)
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb != null)
            {
                draggedRb = rb;
                draggedRb.useGravity = false;
            }
        }
    }
}


    void StopDrag()
    {
        if (draggedRb != null)
        {
            draggedRb.linearVelocity = Vector3.zero;
            draggedRb.useGravity = true;
            draggedRb = null;
        }
    }
}
