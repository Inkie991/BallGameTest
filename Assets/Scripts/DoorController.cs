using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator _animator;
    private bool _isDoorOpened;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _isDoorOpened = false;
        transform.LookAt(GameObject.FindWithTag("Player").transform);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x,
        transform.eulerAngles.y + 90,
        transform.eulerAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Managers.Gameplay.GameOver) return;
        
        if (!_isDoorOpened && other.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger("PlayerClose");
            _isDoorOpened = true;
            EventManager.Broadcast(Events.PlayerWinEvent);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isDoorOpened && other.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger("PlayerAway");
            _isDoorOpened = false;
        }
    }
}
