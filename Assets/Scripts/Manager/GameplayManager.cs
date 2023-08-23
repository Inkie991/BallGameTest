using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour, IGameManager
    {
        public bool GameOver { get; private set; }
        public ManagerStatus Status { get; private set; }

        private Transform _player;
        private Transform _door;
        
        [HideInInspector]public float scalerStepTime = 0.2f;
        [HideInInspector]public float obstacleInfectionTime = 0.5f;

        public void Startup()
        {
             Debug.Log("Gameplay manager starting...");
             
             EventManager.AddListener<PlayerWinEvent>(OnPlayerWin); 
             EventManager.AddListener<PlayerLoseEvent>(OnPlayerLose);
             SceneManager.sceneLoaded += OnSceneLoaded;

             GameOver = false;

             Status = ManagerStatus.Started;
        }
        
        private void OnDestroy()
        {
            EventManager.RemoveListener<PlayerWinEvent>(OnPlayerWin); 
            EventManager.RemoveListener<PlayerLoseEvent>(OnPlayerLose);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _player = GameObject.FindWithTag("Player").transform;
            _door = GameObject.FindWithTag("Door").transform;
        }

        void Update()
        {
            //if (GameOver) return;
        }

        void OnPlayerWin(PlayerWinEvent evt)
        {
            GameOver = true;
        }

        void OnPlayerLose(PlayerLoseEvent evt)
        {
            GameOver = true;
        }

    }