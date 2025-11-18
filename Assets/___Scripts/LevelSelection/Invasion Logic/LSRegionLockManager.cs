using UnityEngine;

public class LSRegionLockManager : MonoBehaviour
{
    public static LSRegionLockManager Instance;

    [Header("Region Locks")]
    public bool _orrostarLocked = false;
    public bool _hyarrostarLocked = true;
    public bool _hyarnustarLocked = true;
    public bool _andustarLocked = true;
    public bool _forostarLocked = true;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable() => RaidRewardManager.OnRewardCollected += HandleRegionCheck;

    private void OnDisable() => RaidRewardManager.OnRewardCollected -= HandleRegionCheck;
    //check if region which region are unlock or locked
    //the check will be based on if every village is in a region is liberated -> based on some liberated event

    public bool IsRegionLocked(Region region)
    {
        return region switch
        {
            Region.Orrostar => _orrostarLocked,
            Region.Hyarrostar => _hyarrostarLocked,
            Region.Hyarnustar => _hyarnustarLocked,
            Region.Andustar => _andustarLocked,
            Region.Forostar => _forostarLocked,
            _ => true,
        };
    }

    private void HandleRegionCheck()
    {
        _orrostarLocked = !LSManager.Instance.IsRegionFullyLiberated(Region.Orrostar);
        _hyarrostarLocked = !LSManager.Instance.IsRegionFullyLiberated(Region.Hyarrostar);
        _hyarnustarLocked = !LSManager.Instance.IsRegionFullyLiberated(Region.Hyarnustar);
        _andustarLocked = !LSManager.Instance.IsRegionFullyLiberated(Region.Andustar);
        _forostarLocked = !LSManager.Instance.IsRegionFullyLiberated(Region.Forostar);

        Debug.Log("[Region Lock Manager] Region lock states updated.");
    }
}
