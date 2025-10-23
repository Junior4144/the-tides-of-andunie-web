using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortalController : MonoBehaviour
{
    //Seamless transition, for initally Gameplay
    [SerializeField]
    private string nextScene;

    [SerializeField]
    private GameObject lastPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;    

        Debug.Log("[NextSceneController] Next Scene is starting");
        GameObject _player = PlayerManager.Instance.gameObject;
        Debug.Log($"Player: {_player.name} and saving data");

        AudioManager.Instance.FadeAudio();

        SaveManager.Instance.SavePlayerStats();

        SceneSavePositionManager.Instance.SavePlayerPosition(SceneManager.GetActiveScene().name, lastPosition.transform.position, lastPosition.transform.rotation);

        PlayerManager.Instance.HandleDestroy();

        LoadNextStage();
    }

    public void NextStage() =>
    LoadNextStage();

    void LoadNextStage() =>
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, nextScene);
}
