using UnityEngine;
using UnityEngine.SceneManagement;

public class Level0CutsceneSceneChanger : MonoBehaviour
{
    public float changeTime;
    public string sceneName;

    [Header("Loading Settings")]
    public float preloadTime = 2f;
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;

    public string nextScene;
    public bool timerStop = false;
    private float timer;

    void Start()
    {
        timer = changeTime;

        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0f;
            fadeCanvas.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipCutscene();
            return;
        }

        if (timerStop) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timerStop = true;
            NextStage();
        }
    }

    public void SkipCutscene()
    {
        if (timerStop) return;

        timerStop = true;
        StopAllCoroutines();
        Debug.Log("Cutscene skipped.");
        NextStage();
    }

    public void NextStage()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("StageEnd");
        if (obj.TryGetComponent(out SceneChangeController ecs))
            ecs.NextStage();

    }
}
