using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

    [SerializeField] private PositionGeneratorNew _generator;

    [SerializeField] private GameObject _prefab;

    private void Start() 
    {
        _generator.GenerateSpawnPositions(OnPositionsGenerated);
    }

    private void OnPositionsGenerated(List<Vector3> positions)
    {
        int counter = 0;
        foreach (var position in positions)
        {
            var obstacle = Instantiate(_prefab, position, Quaternion.identity, transform);
            obstacle.name = "Obstacle " + counter;
            counter++;
        }
    }
}