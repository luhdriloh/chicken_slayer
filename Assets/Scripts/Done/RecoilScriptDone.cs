using UnityEngine;

public class RecoilScriptDone : MonoBehaviour
{
    public float _recoilStartSpeed;
    public float _recoilAcceleration;
    public float _maxRecoilDistance;

    private Vector3 _objectBlowbackSpeed;
    private Vector3 _oldOffsetDistance;

    private bool _recoil;
    private bool _headingBackToStartPosition;

    private void Start()
    {
        _recoil = false;
        _headingBackToStartPosition = false;

        _oldOffsetDistance = Vector3.zero;
    }

    private void Update()
    {
        UpdateRecoil();
    }

    public void AddRecoil()
    {
        _recoil = true;
        _headingBackToStartPosition = false;
        _objectBlowbackSpeed = transform.right.normalized * _recoilStartSpeed;
    }
    
    private void UpdateRecoil()
    {
        if (_recoil == false)
        {
            return;
        }

        // calculate current speed given our acceleration and opposite in direction of our offset
        // basically we calculate the new velocity given acceleration, then we calculate the new position
        // given the new velocity
        _objectBlowbackSpeed += (-_oldOffsetDistance.normalized * _recoilAcceleration * Time.deltaTime);
        Vector3 newOffsetDistance = _oldOffsetDistance + _objectBlowbackSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position - _oldOffsetDistance;

        // if the acceleration is not strong enough to pull it back all the way to the max distance then set return values
        if (newOffsetDistance.magnitude > _maxRecoilDistance)
        {
            newOffsetDistance = newOffsetDistance.normalized * _maxRecoilDistance;

            _objectBlowbackSpeed = Vector3.zero;
            _headingBackToStartPosition = true;
        }
        else if (_headingBackToStartPosition == true && newOffsetDistance.magnitude > _oldOffsetDistance.magnitude)
        {
            transform.position -= _oldOffsetDistance;
            _oldOffsetDistance = Vector3.zero;

            // we are done no more recoil
            _recoil = false;
            _headingBackToStartPosition = false;
            return;
        }

        transform.position = newPosition + newOffsetDistance;
        _oldOffsetDistance = newOffsetDistance;
    }
}
