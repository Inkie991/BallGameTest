using System;
using System.Collections.Generic;
using UnityEngine;

public class PositionGeneratorNew : MonoBehaviour {

    [SerializeField] private float _xMin;
    [SerializeField] private float _xMax;

    [Space]

    [SerializeField] private float _zMin;
    [SerializeField] private float _zMax;

    [Space]

    [SerializeField] private float _prefabRadius;
    [SerializeField] private Transform _blockedZoneCenter;
    [SerializeField] private float _blockedZoneRadius;

    [Space]

    [SerializeField] private int _maxFailIterations = 1000;

    private List<Vector3> _positions;

    private void Start() {
        _positions = new List<Vector3>();
        GenerateSpawnPositions();
    }

    public void GenerateSpawnPositions(Action<List<Vector3>> callback = null) {
        var list = new List<Vector3>();
        Vector3 newPosition = Vector3.zero;

        while (_maxFailIterations > 0) {
            newPosition.x = UnityEngine.Random.Range(_xMin, _xMax);
            newPosition.z = UnityEngine.Random.Range(_zMin, _zMax);
            newPosition.y = 1f;

            if (IsPositionValid(newPosition, list))
                list.Add(newPosition);
            else
                _maxFailIterations--;
        }

        _positions = list;

        callback?.Invoke(list);
    }

    private bool IsPositionValid(Vector3 newPosition, List<Vector3> existingPositions) {
        if (Vector3.Distance(newPosition, _blockedZoneCenter.position) < _blockedZoneRadius + _prefabRadius)
            return false;

        float prefabDiameter = 2 * _prefabRadius;
        foreach (var existingPosition in existingPositions)
            if (Vector3.Distance(newPosition, existingPosition) < prefabDiameter)
                return false;

        return true;
    }

    private void OnDrawGizmos() {
        if (_blockedZoneRadius > 0) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Vector3.zero, _blockedZoneRadius);
        }

        if (_positions == null || _positions.Count == 0 || _prefabRadius <= 0)
            return;

        Gizmos.color = Color.blue;
        foreach (Vector3 position in _positions)
            Gizmos.DrawSphere(position, _prefabRadius);
    }
}