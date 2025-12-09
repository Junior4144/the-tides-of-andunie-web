using UnityEngine;

public class VillageIDManager : MonoBehaviour
{
    public static VillageIDManager Instance;

    public string villageId;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
