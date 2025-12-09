using System;
using UnityEngine;
using UnityEngine.Playables;

public class Level0CutsceneSceneChanger : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public PlayableDirector director;
    public float skipToLastSeconds = 5f;

    [Header("Scene Change System")]
    public string stageEndTag = "StageEnd";

    private bool hasSkipped = false;
    private bool sceneTriggered = false;
    private double sceneChangeTime;
    private double finalTime;
    private bool lockTimeline = false;

    void Start()
    {
        if (director == null)
            director = GetComponent<PlayableDirector>();

        double duration = director.duration;

        sceneChangeTime = duration - .1;
        finalTime = duration;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && hasSkipped == false)
            SkipTimeline();

        CheckTimeline();
    }

    private void CheckTimeline()
    {
        if (sceneTriggered || director == null) return;

        if (!hasSkipped && director.time >= sceneChangeTime)
        {
            sceneTriggered = true;
            ChangeStage();
            return;
        }

        if (hasSkipped && director.time >= finalTime - .1 )
        {
            sceneTriggered = true;
            ChangeStage();
        }
    }

    public void SkipTimeline()
    {
        if (director == null || hasSkipped) return;

        if (director.time >= finalTime - skipToLastSeconds - 1)
        {
            Debug.Log("[TimelineSkipController] Skip disabled — already past skip section.");
            lockTimeline = true;
            return;
        }
        
        if(lockTimeline) return;

        hasSkipped = true;

        double duration = director.duration;
        double targetTime = Mathf.Max(0f, (float)(duration - skipToLastSeconds));

        Debug.Log($"[TimelineSkipController] Skipping to last {skipToLastSeconds} seconds at time {targetTime:0.00}");

        director.time = targetTime;
        director.Evaluate();
        director.Play();
    }

    private void ChangeStage()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(stageEndTag);

        if (obj != null && obj.TryGetComponent(out SceneChangeController ecs))
        {
            ecs.NextStage();
        }
    }
}
