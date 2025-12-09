using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GameData/RegionProgression")]
public class RegionProgression : ScriptableObject
{
    public List<RegionNode> regionNodes;
}
