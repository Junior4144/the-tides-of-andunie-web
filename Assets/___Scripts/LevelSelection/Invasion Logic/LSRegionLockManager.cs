using System.Collections.Generic;
using UnityEngine;
public enum Region
{
    Orrostar,
    Hyarrostar,
    Hyarnustar,
    Andustar,
    Forostar,
    None,
}

[System.Serializable]
public class RegionNode
{
    public Region region;
    public List<Region> prerequisites; // all regions required before unlocking this one
}

public class LSRegionLockManager : MonoBehaviour
{
    public static LSRegionLockManager Instance;

    [Header("Region Locks")]
    public bool _orrostarLocked = false;
    public bool _hyarrostarLocked = true;
    public bool _hyarnustarLocked = true;
    public bool _andustarLocked = true;
    public bool _forostarLocked = true;
    public RegionProgression progressionData;

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
    public bool IsRegionLocked(RegionInfo regionInfo)
    {
        return IsRegionLocked(regionInfo.region);
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

    public List<Region> GetPrerequisiteRegions(Region region)
    {
        HashSet<Region> result = new HashSet<Region>();
        CollectPrerequisites(region, result);
        return new List<Region>(result);
    }

    private void CollectPrerequisites(Region region, HashSet<Region> result)
    {
        // Find the region node
        RegionNode node = progressionData.regionNodes.Find(n => n.region == region);
        if (node == null)
            return;

        foreach (var prereq in node.prerequisites)
        {
            // Add prereq and recursively collect its prereqs
            if (result.Add(prereq))
                CollectPrerequisites(prereq, result);
        }
    }
}
