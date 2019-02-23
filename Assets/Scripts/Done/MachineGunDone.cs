using UnityEngine;

public class MachineGunDone : Shooter
{
    public AudioClip _bulletSoundFX;

    private AudioSource _audioSource;
    private float startYScale;
    private ParticleSystem _muzzleFlash;
    private RecoilScriptDone _recoilScript;

    protected override void Start()
    {
        base.Start();
        startYScale = transform.transform.localScale.y;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _bulletSoundFX;

        _muzzleFlash = GetComponentInChildren<ParticleSystem>();
        _recoilScript = GetComponent<RecoilScriptDone>();
    }

    private void Update()
    {
        // get angle that you are pointing towards
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        RotateWeapon(angle);

        _currentTimeBetweenShotFired += Time.deltaTime;

        // fire
        if (Input.GetMouseButtonDown(0) && _currentTimeBetweenShotFired >= _tapFireDelay || Input.GetMouseButton(0) && _currentTimeBetweenShotFired >= _fireDelay)
        {
            _audioSource.PlayOneShot(_bulletSoundFX);
            FireWeapon(angle);
            MuzzleFlash();
            CameraFunctions._cameraFunctions.AddRecoilInDirection(transform.right);
            _recoilScript.AddRecoil();
        }
    }

    private void RotateWeapon(float angle)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        // switch orientation of weapon depending on where you are pointing
        transform.transform.localScale = transform.right.x <= 0
            ? (Vector3)new Vector2(transform.transform.localScale.x, -startYScale)
            : (Vector3)new Vector2(transform.transform.localScale.x, startYScale);
    }

    private void MuzzleFlash()
    {
        _muzzleFlash.Emit(30);
    }
}
