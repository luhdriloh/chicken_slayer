using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    public float _laserBeamLength;
    private LineRenderer _linerenderer;

    private void Start()
    {
        _linerenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Vector3 endPosition;
        RaycastHit2D[] hits = new RaycastHit2D[1];

        // create a filter for the raycast
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Enemy"));
        filter.useTriggers = true;

        // the colliders hit will be stored in hits
        int collidersHit = Physics2D.Raycast(transform.position, transform.right, filter, hits);
        if (collidersHit > 0)
        {
            endPosition = hits[0].point;
        }
        else
        {
            endPosition = transform.position + (transform.right * _laserBeamLength);
        }

        _linerenderer.SetPositions(new Vector3[] { transform.position, endPosition });
    }
}
