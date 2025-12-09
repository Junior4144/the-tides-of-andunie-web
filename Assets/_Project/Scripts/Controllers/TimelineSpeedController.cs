using UnityEngine;
using UnityEngine.Playables;

public class TimelineSpeedController : MonoBehaviour
{
    public PlayableDirector director;
    public float boostedSpeed = 2f;
    public float normalSpeed = 1f;

    public GameObject panel;

    void Update()
    {
        if (director == null) return;
        if (!director.playableGraph.IsValid())
            return;


        bool isHolding = Input.GetMouseButton(0);
        float targetSpeed = isHolding ? boostedSpeed : normalSpeed;

        if (isHolding)
        {
            panel.SetActive(true);
        }
        else { panel.SetActive(false); }

        director.playableGraph.GetRootPlayable(0).SetSpeed(targetSpeed);
    }
}
