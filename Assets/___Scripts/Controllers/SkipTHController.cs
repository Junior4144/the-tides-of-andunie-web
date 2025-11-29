using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipTHController : MonoBehaviour
{
    private bool HasSkipped = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !HasSkipped)
        {
            HasSkipped = true;
            HandleSkip();
        }
    }

    public void HandleSkip()
    {
        Debug.Log("SKIP pressed � handling all transitions");

        GameObject obj = GameObject.FindGameObjectWithTag("StageEnd");
        if (obj.TryGetComponent(out SceneChangeController ecs))
            ecs.NextStage();

        if (PlayerManager.Instance)
            PlayerManager.Instance.HandleDestroy();

    }
}
