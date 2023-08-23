using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Material _obstacleMaterial;
    [SerializeField] private Material _infectedMaterial;
    [SerializeField] private ParticleSystem _explosionVFX;
    private MeshRenderer _renderer;
    
    private void Start()
    {
        _explosionVFX.Stop();
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = _obstacleMaterial;
    }

    public void Infection()
    {
        _renderer.material = _infectedMaterial;
        StartCoroutine(WaitingForDeath());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) EventManager.Broadcast(Events.PlayerLoseEvent);
    }
    

    private IEnumerator WaitingForDeath()
    {
        yield return new WaitForSeconds(0.5f);
        _renderer.enabled = false;
        _explosionVFX.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
