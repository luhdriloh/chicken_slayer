using UnityEngine;

public class LittleDevil : MonoBehaviour
{
    public float _speed;
    public float yMin;
    public float yMax;
    public float xDistance;

    private Rigidbody2D _rigidbody;
    private float _health = 6;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = new Vector2(_speed, 0f);
    }

    public void SubtractHealth(int amount)
    {
        _health -= amount;

        if (_health <= 0)
        {
            _health = 6;
            transform.position = new Vector3(xDistance, Random.Range(yMin, yMax), -1);
        }
    }
}
