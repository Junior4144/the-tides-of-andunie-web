using UnityEngine;

public abstract class ItemEffect : MonoBehaviour
{
    public abstract void Apply();
    public abstract void Remove();
    public abstract string GetDescription();
}