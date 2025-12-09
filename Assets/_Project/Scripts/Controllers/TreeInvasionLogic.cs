using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreeInvasionLogic : MonoBehaviour
{
    private GameObject[] _fire_position_list;

    [SerializeField]
    private GameObject fireSprite_1;


    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
        {
            if (gameObject.scene == newScene)
                SpawnFire();
        }

    }

    private void Awake()
    {
        CacheFirePoints();
    }

    private void CacheFirePoints()
    {
        int fireCount = transform.childCount;
        _fire_position_list = new GameObject[fireCount];

        for (int i = 0; i < fireCount; i++)
        {
            _fire_position_list[i] = transform.GetChild(i).gameObject;
        }
    }

    private void SpawnFire()
    {
        if (LSManager.Instance.GetVillageState(VillageIDManager.Instance.villageId) != VillageState.Invaded)
            return;

        int fireCount = _fire_position_list.Length;

        for (int i = 0; i < fireCount; i++)
        {
            Instantiate(fireSprite_1, _fire_position_list[i].transform.position, Quaternion.identity);
        }
    }

}
