using System.Collections.Generic;
using UnityEngine;

public class PirateSpawnCollectable : MonoBehaviour
{
    [SerializeField]
    private PirateAttributes _enemyAttribute;
    [SerializeField]
    private List<GameObject> _collectablePrefabs;
    public void RandomlyDropCollectable()
    {
        float random = Random.Range(0f, 1f);

        if (_enemyAttribute.ChanceOfCollectableDrop >= random)
        {
            SpawnCollectable(transform.position);

        }
    }

    public void SpawnCollectable(Vector2 position)
    {
        int index = Random.Range(0, _collectablePrefabs.Count);
        var selectedCollectable = _collectablePrefabs[index];

        Instantiate(selectedCollectable, position, Quaternion.identity);
    }
}
