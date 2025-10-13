using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCurrentScene : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Debug.Log("[EndCurrentScene] Next Scene is starting");
        GameObject _player = PlayerManager.Instance.gameObject;
        Debug.Log($"Player: {_player.name} and saving data");

        AudioManager.Instance.FadeAudio();

        SaveManager.Instance.SavePlayerStats();

        PlayerManager.Instance.HandleDestroy();

        LoadNextStage();
    }
    public void NextStage() =>
        LoadNextStage();

    void LoadNextStage() =>
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, nextScene);
}
