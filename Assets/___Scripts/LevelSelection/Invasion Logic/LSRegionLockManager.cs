using System.Collections.Generic;
using UnityEngine;
using static SaveGameManager;
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
    public List<Region> prerequisites;
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

    private Dictionary<Region, bool> _previousLockStates;

    private void Awake()
    {
        Instance = this;

        _previousLockStates = new Dictionary<Region, bool>
    {
        { Region.Orrostar, _orrostarLocked },
        { Region.Hyarrostar, _hyarrostarLocked },
        { Region.Hyarnustar, _hyarnustarLocked },
        { Region.Andustar, _andustarLocked },
        { Region.Forostar, _forostarLocked }
    };
    }

    private void OnEnable() => RewardListener.VillageSet += HandleRegionCheck;

    private void OnDisable() => RewardListener.VillageSet -= HandleRegionCheck;

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
        _orrostarLocked = false;

        _hyarrostarLocked = !LSManager.Instance.IsRegionFullyLiberated(Region.Orrostar);

        _hyarnustarLocked = !LSManager.Instance.IsRegionFullyLiberated(Region.Hyarrostar);

        _andustarLocked = !LSManager.Instance.IsRegionFullyLiberated(Region.Hyarnustar);

        _forostarLocked = !LSManager.Instance.IsRegionFullyLiberated(Region.Andustar);

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
        RegionNode node = progressionData.regionNodes.Find(n => n.region == region);
        if (node == null)
            return;

        foreach (var prereq in node.prerequisites)
        {
            if (result.Add(prereq))
                CollectPrerequisites(prereq, result);
        }
    }

    public Region CheckForNewUnlockedRegion()
    {
        Region[] regions = new Region[]
        {
        Region.Orrostar,
        Region.Hyarrostar,
        Region.Hyarnustar,
        Region.Andustar,
        Region.Forostar
        };

        foreach (Region region in regions)
        {
            bool previouslyLocked = _previousLockStates[region];
            bool currentlyLocked = IsRegionLocked(region);
            
            if (previouslyLocked && !currentlyLocked)
            {
                _previousLockStates[region] = currentlyLocked;
                return region;
            }
            
            _previousLockStates[region] = currentlyLocked;
        }

        return Region.None;
    }

    public RegionLockSaveData GetSaveData()
    {
        return new RegionLockSaveData
        {
            orrostarLocked = _orrostarLocked,
            hyarrostarLocked = _hyarrostarLocked,
            hyarnustarLocked = _hyarnustarLocked,
            andustarLocked = _andustarLocked,
            forostarLocked = _forostarLocked
        };
    }

    public void ApplySaveData(RegionLockSaveData data)
    {
        _orrostarLocked = data.orrostarLocked;
        _hyarrostarLocked = data.hyarrostarLocked;
        _hyarnustarLocked = data.hyarnustarLocked;
        _andustarLocked = data.andustarLocked;
        _forostarLocked = data.forostarLocked;
    }

    public void ResetRegionLocks()
    {
        _orrostarLocked = false;
        _hyarrostarLocked = true;
        _hyarnustarLocked = true;
        _andustarLocked = true;
        _forostarLocked = true;

    }
}