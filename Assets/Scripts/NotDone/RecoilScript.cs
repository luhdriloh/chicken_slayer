using UnityEngine;

public class RecoilScript : MonoBehaviour
{
    public float _maximumOffsetDistance;
    public float _recoilAcceleration;
    public float _weaponRecoilStartSpeed;

    private bool _recoilInEffect;
    private bool _weaponHeadedBackToStartPosition;

    private Vector3 _offsetPosition;
    private Vector3 _recoilSpeed;

    public void AddRecoil()
    {
        _recoilInEffect = true;
        _weaponHeadedBackToStartPosition = false;
        _recoilSpeed = transform.right * _weaponRecoilStartSpeed;
    }

    public void AddRecoil(Vector3 firedirection)
    {
        _recoilInEffect = true;
        _weaponHeadedBackToStartPosition = false;
        _recoilSpeed = firedirection * _weaponRecoilStartSpeed;
    }

    private void Start()
    {
        _recoilSpeed = Vector3.zero;
        _offsetPosition = Vector3.zero;

        _recoilInEffect = false;
        _weaponHeadedBackToStartPosition = false;
    }

    private void Update()
    {
        UpdateRecoil();
    }

    private void UpdateRecoil()
    {
        if (_recoilInEffect == false)
        {
            return;
        }

        // set up speed and then position variables
        _recoilSpeed += (-_offsetPosition.normalized) * _recoilAcceleration * Time.deltaTime;
        Vector3 newOffsetPosition = _offsetPosition + _recoilSpeed * Time.deltaTime;
        Vector3 newTransformPosition = transform.position - _offsetPosition;

        if (newOffsetPosition.magnitude > _maximumOffsetDistance)
        {
            _recoilSpeed = Vector3.zero;
            _weaponHeadedBackToStartPosition = true;
            newOffsetPosition = _offsetPosition.normalized * _maximumOffsetDistance;
        }
        else if (_weaponHeadedBackToStartPosition == true && newOffsetPosition.magnitude > _offsetPosition.magnitude)
        {
            transform.position -= _offsetPosition;
            _offsetPosition = Vector3.zero;

            // set up our boolean
            _recoilInEffect = false;
            _weaponHeadedBackToStartPosition = false;
            return;
        }

        transform.position = newTransformPosition + newOffsetPosition;
        _offsetPosition = newOffsetPosition;
    }
}



























