using UnityEngine;

public class DynamicSpriteSorting : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private int _baseSortingOrder = 100;

    private static int instanceCounter = 0;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        instanceCounter++;
        _spriteRenderer.sortingOrder = _baseSortingOrder + instanceCounter;
    }

    private void OnDestroy()
    {
        instanceCounter--;
    }
}
