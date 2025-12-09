using UnityEngine;

public class AgentTarget : MonoBehaviour
{
    void OnEnable()
    {
        TargetEvents.OnClearAllTargets += HandleClearEvent;
    }

    void OnDisable()
    {
        TargetEvents.OnClearAllTargets -= HandleClearEvent;
    }

    void HandleClearEvent()
    {
        Destroy(gameObject);
    }
}