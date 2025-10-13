using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Raids/RaidConfig")]
public class RaidConfig : ScriptableObject
{
    public List<WaveConfig> waves;
}
