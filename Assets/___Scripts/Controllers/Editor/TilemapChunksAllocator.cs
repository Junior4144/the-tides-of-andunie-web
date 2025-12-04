using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Linq;

public class TilemapChunksAllocator : MonoBehaviour
{
    [Header("Source Tilemap")]
    public Tilemap sourceTilemap;

    [Header("Import Settings")]
    public string pngPrefix = "Baked_Tilemap_";  // prefix PNG name
    public int chunks = 5;
    public int tilePixelSize = 16;

    [Header("Rendering")]
    public string sortingLayer = "Default";
    public int baseSortingOrder = 0;

    [Header("Options")]
    public bool disableOriginalTilemapRenderer = true;

    [ContextMenu("Allocate Baked Chunks")]
    public void Allocate()
    {
        if (sourceTilemap == null)
        {
            Debug.LogError("[Allocator] No source tilemap assigned!");
            return;
        }

        BoundsInt bounds = sourceTilemap.cellBounds;

        for (int i = 0; i < chunks; i++)
        {
            string filePath = $"Assets/{pngPrefix}{i}.png";

            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
            if (tex == null)
            {
                Debug.LogWarning($"[Allocator] PNG not found at: {filePath}");
                continue;
            }

            GameObject go = new GameObject($"Trees_Chunk_{i}");
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

            Sprite spr = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0, 0),
                tilePixelSize
            );

            sr.sprite = spr;
            sr.sortingLayerName = sortingLayer;
            sr.sortingOrder = baseSortingOrder + i;

            // Calculate vertical placement for this chunk
            int chunkHeightTiles = Mathf.CeilToInt((float)bounds.size.y / chunks);
            int startYTile = bounds.y + i * chunkHeightTiles;

            Vector3 pos = new Vector3(bounds.x, startYTile, 0);
            go.transform.position = pos;

            Debug.Log($"[Allocator] Placed chunk {i} at {pos}");
        }

        if (disableOriginalTilemapRenderer)
        {
            TilemapRenderer tmr = sourceTilemap.GetComponent<TilemapRenderer>();
            if (tmr != null)
                tmr.enabled = false;
        }

        Debug.Log("[Allocator] Finished allocating baked chunks.");
    }
}