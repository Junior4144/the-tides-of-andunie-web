using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTester : MonoBehaviour
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
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, nextScene);
    }
}
