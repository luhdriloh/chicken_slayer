using UnityEngine;

public class CameraFunctions : MonoBehaviour
{
    public static CameraFunctions _cameraFunctions;

    private void Start()
    {
        if (_cameraFunctions == null)
        {
            _cameraFunctions = this;
        }
        else
        {
            Destroy(this);
        }

    }

    public void AddRecoilInDirection(Vector3 fireDirection)
    {
        //_recoilScript.AddRecoilInDirection(fireDirection);
    }
}
