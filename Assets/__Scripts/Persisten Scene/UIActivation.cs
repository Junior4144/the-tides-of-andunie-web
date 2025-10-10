using UnityEngine;

public class UIActivation: MonoBehaviour
{
    public enum UIType { None, Gameplay}

    [SerializeField] private UIType sceneUIType;

    private void Start()
    {
        UIManager.Instance.ShowUI(sceneUIType);
    }
}