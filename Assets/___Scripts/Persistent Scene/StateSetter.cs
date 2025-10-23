using UnityEngine;

public class StateSetter : MonoBehaviour
{
    [SerializeField]
    private GameState stateToSet;

    void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetState(stateToSet);
    }
}
