using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCurrentScene : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        GameObject _player = PlayerManager.Instance.gameObject;
        Debug.Log($"Player: {_player.name}");

        var movement = _player.GetComponent<PlayerHeroMovement>();
        if (movement != null)
        {
            Debug.Log("Disabing player's movement");
            movement.enabled = false;
        }


        var health = _player.GetComponentInChildren<PlayerHealthController>();
        if (health != null)
        {
            Debug.Log("Disabing player's health");
            health.enabled = false;
        }

        LoadNextStage();
    }

    public void NextStage() =>
        LoadNextStage();

    void LoadNextStage() =>
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, nextScene);

}
