using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardListener : MonoBehaviour
{
    public string VillageLiberationID;
    public string Location;
    public string NextScene;

    public static event Action VillageSet;

    private void OnEnable() => RaidRewardManager.OnRewardCollected += HandleRewardCollected;

    private void OnDisable() => RaidRewardManager.OnRewardCollected -= HandleRewardCollected;

    private void HandleRewardCollected()
    {
        LSManager.Instance.SetVillageState(VillageLiberationID, VillageState.Liberated_Done);
        GlobalStoryManager.Instance.HasExitedLiberation = true;
        VillageSet?.Invoke();

        SceneSavePositionManager.Instance.ResetPlayerPosition(gameObject.scene.name);

        SaveGameManager.Instance.SaveGame();

        Debug.Log($"[LS UI MANAGER] Next scene : {NextScene}");

        SaveManager.Instance.SaveLastLocation(Location);

        Debug.Log($"[EndCurrentScene] Next Village: {NextScene} and Location: {Location}");

        GameObject _player = PlayerManager.Instance.gameObject;
        Debug.Log($"Player: {_player.name} and saving data");

        AudioManager.Instance.FadeAudio();
        SaveManager.Instance.SavePlayerStats();

        LoadNextStage();
    }

    private void LoadNextStage() =>
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, NextScene);
}
