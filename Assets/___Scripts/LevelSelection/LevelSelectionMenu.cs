using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.FilePathAttribute;

public class LevelSelectionMenu : MonoBehaviour
{
    public GameObject menuCanvas;

    private string NextScene;
    private string Location;

    public static event Action OnPlayerLeavingLevelSelectionZone;
    private void OnEnable() => LevelSelection.PlayerActivatedMenu += HandleMenu;

    private void OnDisable() => LevelSelection.PlayerActivatedMenu -= HandleMenu;

    void Start() => menuCanvas.SetActive(false);


    private void HandleMenu(string nextScene, string location)
    {
        menuCanvas.SetActive(!menuCanvas.activeSelf);
        NextScene = nextScene;
        Location = location;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuCanvas.SetActive(false);
        }
    }

    public void ButtonClicked() =>
        ProceedToNextStage();

    private void ProceedToNextStage()
    {
        SaveManager.Instance.SaveLastLocation(Location);
        OnPlayerLeavingLevelSelectionZone?.Invoke();
        Debug.Log($"[EndCurrentScene] Next Scene: {NextScene} and Location: {Location}");

        GameObject _player = PlayerManager.Instance.gameObject;
        Debug.Log($"Player: {_player.name} and saving data");

        AudioManager.Instance.FadeAudio();
        SaveManager.Instance.SavePlayerStats();
        PlayerManager.Instance.HandleDestroy();

        LoadNextStage();
    }

    private void LoadNextStage() =>
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, NextScene);
}
