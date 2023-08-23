using UnityEngine;

public class CameraFollow : MonoBehaviour

{
    [SerializeField] private Transform _target;
    private float _smoothSpeed = 0.125f;
    private float _yPos;
    private float _xOffset;
    private float _zOffset;

        void Awake()
    {
        _yPos = transform.position.y;
        _xOffset = transform.position.x - _target.position.x;
        _zOffset = transform.position.z - _target.position.z;
    }

    void FixedUpdate()
    {
        float tempX = _target.position.x + _xOffset;
        float tempZ = _target.position.z + _zOffset;
        Vector3 desiredPosition = new Vector3(tempX, _yPos, tempZ);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;
    }
}