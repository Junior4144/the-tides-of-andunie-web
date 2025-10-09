using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class endgameidk : MonoBehaviour
{
    [SerializeField]
    private float _timeToWaitBeforeExit;

    [SerializeField]
    private SceneController _sceneController;

    private GameObject _playerHero;

    PlayerHeroMovement _playerHeroMovement;

    public String nextScene;

    void Awake()
    {
        _playerHero = GameObject.FindWithTag("Player");
        _playerHeroMovement = _playerHero.GetComponent<PlayerHeroMovement>();
    }

    void Start()
    {   
        _playerHero = GameObject.FindWithTag("Player");
        _playerHeroMovement = _playerHero.GetComponent<PlayerHeroMovement>();
        if (_playerHeroMovement)
            _playerHeroMovement.enabled = true;
    }

    public void EndCurrentSession() =>
        Invoke(nameof(EndSession), _timeToWaitBeforeExit);

    private void EndSession() =>
        _sceneController.LoadScene("Main Menu");

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {   
            EndGameChangeStats();
            LoadNextStage();
        }
    }

    void EndGameChangeStats()
    {   
        _playerHero = GameObject.FindWithTag("Player");
        _playerHeroMovement = _playerHero.GetComponent<PlayerHeroMovement>();
        _playerHeroMovement.enabled = false;
    }

    void LoadNextStage()
    {
        SceneManager.LoadScene("PersistentGameplay");
        SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
    }
}
