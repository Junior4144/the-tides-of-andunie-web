using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveVillageSelectionMenu : MonoBehaviour
{
    public GameObject menuCanvas;

    private string NextScene;
    private string Location;

    public static event Action OnPlayerLeavingLevelSelectionZone;
    private void OnEnable() => VillageToLevelSelection.PlayerActivatedLeaveVillageMenu += HandleMenu;

    private void OnDisable() => VillageToLevelSelection.PlayerActivatedLeaveVillageMenu -= HandleMenu;

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

        OnPlayerLeavingLevelSelectionZone?.Invoke();
        SaveManager.Instance.SaveLastLocation(Location);
        Debug.Log($"[LeaveVillageSelectionMenu] Next Scene: {NextScene} and Last Location: {Location}");

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
