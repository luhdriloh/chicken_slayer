using UnityEngine;

public class MachineGun : Shooter
{
    public RecoilScript _cameraRecoilScript;
    public AudioClip _weaponFiringClip;
    public ParticleSystem _muzzleflash;

    private float startYScale;
    private RecoilScript _recoilScript;
    private AudioSource _audioSource;

    protected override void Start()
    {
        base.Start();
        startYScale = transform.transform.localScale.y;
        _recoilScript = GetComponent<RecoilScript>();
        _audioSource = GetComponent<AudioSource>();
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
            _audioSource.PlayOneShot(_weaponFiringClip);
            _cameraRecoilScript.AddRecoil(transform.right);
            _recoilScript.AddRecoil();
            EmitMuzzleFlash();
            FireWeapon(angle);
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

    private void EmitMuzzleFlash()
    {
        _muzzleflash.Emit(30);
    }
}
