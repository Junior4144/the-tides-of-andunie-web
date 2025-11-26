using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageStateSetter : MonoBehaviour
{
    [SerializeField] private GameState stateToSet;

    private VillageState villageState;
    private string villageId;

    void Update()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        Debug.Log("[State Setter]");
        if (gameObject.scene == activeScene)
        {
            if (LSManager.Instance != null)
            {
                villageId = VillageIDManager.Instance.villageId;
                villageState = LSManager.Instance.GetVillageState(villageId);
            }

            if(villageState == VillageState.Invaded) 
            { 
                stateToSet = GameState.Gameplay; 
            }

            if (GameManager.Instance != null)
                GameManager.Instance.SetState(stateToSet);

            Debug.Log($"[State Setter] {activeScene.name} is active and  now SetState to {stateToSet}");
            enabled = false;
        }
    }
}

