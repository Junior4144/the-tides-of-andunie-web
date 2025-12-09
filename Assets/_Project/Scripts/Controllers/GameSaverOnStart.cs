using System.Collections;
using UnityEngine;

public class GameSaverOnStart : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(HandleSetup());
    }

    private IEnumerator HandleSetup()
    {
        yield return new WaitForSeconds(0.5f);
        SceneSavePositionManager.Instance.SaveLastScene(gameObject.scene.name);
        SaveGameManager.Instance.SaveGame();
    }
}
