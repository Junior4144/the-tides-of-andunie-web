using UnityEngine;

public abstract class ItemEffect : MonoBehaviour
{
    public abstract void Apply();
    public abstract string GetDescription();
}