using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new(0, 2f, 0);
    private Transform player;
    private Camera cam;
    private RectTransform rect;

    private IEnumerator Start()
    {
        rect = GetComponent<RectTransform>();
        
        yield return new WaitUntil(() => CameraManager.Instance != null && PlayerManager.Instance != null);
        yield return new WaitUntil(() => PlayerManager.Instance.transform != null);

        cam = CameraManager.Instance.GetComponentInChildren<Camera>();
        player = PlayerManager.Instance.transform;
        Debug.Log($"FollowPlayer: Bound to {player.name} and {cam.name}");
    }

    private void LateUpdate()
    {
        if (player == null || cam == null) return;
        Vector3 target = player.position + offset;
        rect.position = cam.WorldToScreenPoint(target);
    }
}
