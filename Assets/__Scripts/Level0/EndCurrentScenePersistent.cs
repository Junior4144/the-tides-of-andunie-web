using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCurrentScenePersistent : MonoBehaviour
{
    [SerializeField] private float _timeToWaitBeforeExit;
    [SerializeField] private SceneController _sceneController;

    private PlayerHeroMovement _playerHeroMovement;
    private GameObject _player;

    public string nextScene;

    void Start()
    {
        _player = PlayerManager.Instance.gameObject;
        _playerHeroMovement = _player.GetComponent<PlayerHeroMovement>();
        if (_playerHeroMovement)
            _playerHeroMovement.enabled = true;
    }

    public void EndCurrentSession()
    {
        Invoke(nameof(EndSession), _timeToWaitBeforeExit);
    }

    private void EndSession()
    {
        _sceneController.LoadScene("Main Menu");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            _playerHeroMovement.enabled = false;
            Debug.Log(_player);
            EndGameChangeStats();
            LoadNextStage();
        }
    }

    void EndGameChangeStats()
    {
        if (_playerHeroMovement)
            _playerHeroMovement.enabled = false;
    }

    void LoadNextStage()
    {
        _sceneController.LoadNextStage("PersistentGameplay", gameObject.scene.name, nextScene);
    }
}
