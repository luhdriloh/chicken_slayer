using UnityEngine;

public class LaserPointerDone : MonoBehaviour
{
    public float _laserLength;
    public LayerMask _border;
    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Vector3 endPosition = (transform.right * _laserLength) + transform.position;

        RaycastHit2D[] hits = new RaycastHit2D[1];

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Enemy"));
        filter.useTriggers = true;

        // the colliders hit will be stored in hits
        int collidersHit = Physics2D.Raycast(transform.position, transform.right, filter, hits);
        if (collidersHit > 0)
        {
            _lineRenderer.SetPositions(new Vector3[] { transform.position, hits[0].point });
        }
        else
        {
            _lineRenderer.SetPositions(new Vector3[] { transform.position, endPosition });
        }
    }
}
