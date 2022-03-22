using UnityEngine;
using System.Collections;

public class BlobShadow : MonoBehaviour
{
    private LayerMask shadowCastLayer;
    public Transform targetTransform;
    public Vector3 offset = new Vector3(0, 0.1f, 0);

    private Transform cachedTransform;
    private Renderer shadowRenderer;
    //public float defaultScale = 1f;
    //public float divisionScale = 10f;

    public float defaultScale = 1.25f;
    public float divisionScale = 0.25f;


    void Start()
    {
        cachedTransform = this.transform;
        shadowRenderer = GetComponentInChildren<Renderer>();
        transform.rotation = Quaternion.identity;
        
        shadowCastLayer = LayerMask.NameToLayer("Default");
    }

    void LateUpdate()
    {
        if (targetTransform == null) return;

        // cast a ray to the floor to work out where the shadow should be
        RaycastHit hitInfo;
        Vector3 targetPosition = targetTransform.position;
        if (Physics.Raycast(targetPosition, Vector3.down, out hitInfo, Mathf.Infinity, 1 << shadowCastLayer))
        {
            cachedTransform.position = hitInfo.point + offset;
            float scale = defaultScale - Mathf.Abs(hitInfo.point.y - targetPosition.y) * divisionScale;
            if (scale < 0f) scale = 0f;

            cachedTransform.localScale = new Vector3(scale, 0, scale);
            shadowRenderer.enabled = true;
        }
        else
        {
            shadowRenderer.enabled = false;
        }
    }
}
