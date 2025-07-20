using UnityEngine;

public class CrosshairPosition : MonoBehaviour
{
   void LateUpdate() {
    Vector3 pos = transform.localPosition;
    transform.localPosition = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), pos.z);
}
 
}
