using UnityEngine;
using UnityEngine.SceneManagement;

public class EndWholeGame : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Debug.Log("[EndWholeGame] Reseting Whole Game");

        SaveManager.Instance.ResetSaveData();

        AudioManager.Instance.FadeAudio();

        PlayerManager.Instance.HandleDestroy();

        LoadNextStage();
    }
    public void NextStage() =>
        LoadNextStage();

    void LoadNextStage() =>
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, nextScene);
}
