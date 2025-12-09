using UnityEngine;

public class LevelSelectionDisableController : MonoBehaviour
{
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        RaidController.OnRaidStart += HandleRaidStart;
    }
    private void OnDestroy()
    {
        RaidController.OnRaidStart -= HandleRaidStart;
    }

    private void HandleRaidStart()
    {
        _collider.enabled = false;
    }

}
