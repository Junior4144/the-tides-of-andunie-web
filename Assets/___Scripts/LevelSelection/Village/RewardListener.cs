using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardListener : MonoBehaviour
{
    [SerializeField] private RaidRewardManager raidRewardManager;

    public string VillageLiberationID;
    public string Location;
    public string NextScene;

    private void OnEnable() => raidRewardManager.OnRewardCollected += HandleRewardCollected;

    private void OnDisable() => raidRewardManager.OnRewardCollected -= HandleRewardCollected;

    private void HandleRewardCollected()
    {
        LSManager.Instance.SetVillageState(VillageLiberationID, VillageState.Liberated_Done);

        SceneSavePositionManager.Instance.ResetPlayerPosition(gameObject.scene.name);

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
