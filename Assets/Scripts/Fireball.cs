using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private ParticleSystem _splashVFX;
    private Rigidbody _body;
    private Transform _doorPos;
    private Vector3 _collisionPoint;
    private bool _isColliding;

    // Start is called before the first frame update
    void Start()
    {
        _splashVFX.Stop();
        _isColliding = false;
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isColliding) transform.position = _collisionPoint;
    }

    public void Shoot(Transform target)
    {
        float duration = Vector3.Distance(transform.position, target.position) / 5;
        if (_body != null) _body.DOMove(target.position, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        _body.DOKill();
        _isColliding = true;
        _collisionPoint = transform.position;
        _splashVFX.Play();
        ObstacleInfection();
    }

    private void ObstacleInfection()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 
            transform.localScale.x / 3 * 5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Obstacle>() != null) hitCollider.GetComponent<Obstacle>().Infection();
        }

        PathIsClearEvent evt = new() { FireballPos = transform.position };
        EventManager.Broadcast(evt);
        
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 3 * 7.5f);
    }
}
