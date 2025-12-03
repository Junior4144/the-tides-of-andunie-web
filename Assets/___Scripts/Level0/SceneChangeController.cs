using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeController : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        LoadNextStage();
    }

    public void SkipStage()
    {
        LoadNextStage();
    }

    public void NextStage()
    {
        LoadNextStage();
    }

    void LoadNextStage()
    {
        AudioManager.Instance.FadeAudio();
        SaveManager.Instance.SavePlayerStats();

        if (PlayerManager.Instance && gameObject.scene.name != "Level0Cutscene")
            PlayerManager.Instance.HandleDestroy();


        SceneSavePositionManager.Instance.SaveLastScene(nextScene);
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, nextScene);
    }
}
