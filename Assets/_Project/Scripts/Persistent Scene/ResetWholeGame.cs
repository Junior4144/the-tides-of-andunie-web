using UnityEngine;
using UnityEngine.SceneManagement;

public class EndWholeGame : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        NextStage();
    }
    
    public void NextStage()
    {
        Debug.Log("[EndWholeGame] Reseting Whole Game");

        Utility.ResetGameSceneSetup();

        LoadNextStage();
    }

    void LoadNextStage() =>
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, nextScene);
}
