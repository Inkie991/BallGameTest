using UnityEngine;

public class PathPreview : MonoBehaviour
{
    [SerializeField] private Transform _sprite;
    [SerializeField] private Transform _source;
    [SerializeField] private Transform _target;
    [SerializeField] private float _width;
    [Space(10f)] 
    [SerializeField] private float _xOffset;
    [SerializeField] private float _zOffset;
    private float _pathScaleFactor;

    private void Awake()
    {
        _pathScaleFactor = _width / 2;
    }
    
    private void Update() 
    {
        
        if (Managers.Gameplay.GameOver) return;
        
        if (_source != null && _target != null && _sprite != null)
        {

            var _sourcePosition = _source.position;
            var _sourcePosWithOffset = new Vector3(_sourcePosition.x + _xOffset, _sourcePosition.y, _sourcePosition.z + _zOffset);
            _sourcePosition = _sourcePosWithOffset;
            
            var center = (_sourcePosition + _target.position) / 2f;
            center.y = 0.025f;
            _sprite.position = center;

            var direction = _target.position - _sourcePosition;
            direction.y = 0f;

            _sprite.localScale = new Vector3(direction.magnitude, _width, 1);

            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            _sprite.rotation = Quaternion.Euler(-90f, -angle, 0f);
        }
    }

    public void PathScaler(float value)
    {
        _width -= value * _pathScaleFactor;
        _xOffset -= value * (_xOffset / 2);
        _zOffset -= value * (_zOffset / 2);
    }
}

