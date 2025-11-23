using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortalController : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    [SerializeField]
    private GameObject lastPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        SceneSavePositionManager.Instance.SavePlayerPosition(gameObject.scene.name, lastPosition.transform.position, lastPosition.transform.rotation);

        Utility.PreSceneChangeSetup();

        LoadNextStage();
    }

    public void NextStage() =>
        LoadNextStage();

    void LoadNextStage() =>
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, nextScene);
}
