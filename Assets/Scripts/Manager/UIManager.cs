using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour  , IGameManager
{
    public ManagerStatus Status { get; private set; }
    
    [SerializeField] private Canvas _winCanvas;
    [SerializeField] private Canvas _loseCanvas;

    public void Startup()
    {
        Debug.Log("UI manager starting...");

        EventManager.AddListener<PlayerWinEvent>(OnWinEvent); 
        EventManager.AddListener<PlayerLoseEvent>(OnLoseEvent);

        Status = ManagerStatus.Started;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerWinEvent>(OnWinEvent); 
        EventManager.RemoveListener<PlayerLoseEvent>(OnLoseEvent);
    }

    private void Start()
    {
        ToggleWin(false);
        ToggleLose(false);
    }

    void CollectCanvas()
    {
        _winCanvas= GameObject.FindWithTag("WinCanvas").GetComponent<Canvas>();
        _loseCanvas = GameObject.FindWithTag("LoseCanvas").GetComponent<Canvas>();
        
        ToggleWin(false);
        ToggleLose(false);
    }

    private void Update()
    {
        
    }

    private void OnWinEvent(PlayerWinEvent evt)
    {
        ToggleWin(true);
    }
    
    private void OnLoseEvent(PlayerLoseEvent evt)
    {
        ToggleLose(true);
    }
    
    private void ToggleWin(bool value)
    {
        _winCanvas.gameObject.SetActive(value);
    }

    private void ToggleLose(bool value)
    {
        _loseCanvas.gameObject.SetActive(value);
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

}
