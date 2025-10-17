using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private bool isPlayerInside = false;

    public static LevelSelection instance;

    public static event Action OnPlayerEnterSelectionZone;
    public static event Action OnPlayerExitSelectionZone;
    public static event Action OnPlayerLeavingLevelSelectionZone;

    public string location = "DefaultSpawn";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        OnPlayerEnterSelectionZone?.Invoke();
        Debug.Log("[Level Selection] Player entered level zone");
        isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        OnPlayerExitSelectionZone?.Invoke();
        Debug.Log("[Level Selection] Player left level zone");
        isPlayerInside = false;
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("[Level Selection] Enter key pressed inside zone");
            ProceedToNextStage();
        }
    }

    private void ProceedToNextStage()
    {
        SaveManager.Instance.SaveLastLocation(location);
        OnPlayerLeavingLevelSelectionZone?.Invoke();
        Debug.Log("[EndCurrentScene] Next Scene is starting");

        GameObject _player = PlayerManager.Instance.gameObject;
        Debug.Log($"Player: {_player.name} and saving data");

        AudioManager.Instance.FadeAudio();
        SaveManager.Instance.SavePlayerStats();
        PlayerManager.Instance.HandleDestroy();

        LoadNextStage();
    }

    public void NextStage() => LoadNextStage();

    private void LoadNextStage() =>
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, nextScene);
}
