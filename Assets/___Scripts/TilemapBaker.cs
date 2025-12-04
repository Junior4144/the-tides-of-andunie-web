using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;

public class TilemapBaker : MonoBehaviour
{
    [Header("Source")]
    public Tilemap sourceTilemap;

    [Header("Tile Settings")]
    public int tilePixelSize = 16;     // IMPORTANT: must match your tile size

    [Header("Chunk Settings")]
    public int chunks = 5;             // splits output vertically (increase if too large)

    [ContextMenu("Bake Tilemap (Chunked)")]
    public void Bake()
    {
        if (sourceTilemap == null)
        {
            Debug.LogError("[TilemapBaker] No source tilemap assigned!");
            return;
        }

        BoundsInt bounds = sourceTilemap.cellBounds;

        int totalHeightInTiles = bounds.size.y;
        int chunkHeightInTiles = Mathf.CeilToInt((float)totalHeightInTiles / chunks);

        Debug.Log($"[TilemapBaker] Starting bake in {chunks} chunks...");
        Debug.Log($"[TilemapBaker] Bounds: {bounds.size.x} x {bounds.size.y} tiles.");

        for (int c = 0; c < chunks; c++)
        {
            BakeChunk(bounds, c, chunkHeightInTiles);
        }

        Debug.Log("[TilemapBaker] Bake completed!");
        AssetDatabase.Refresh();
    }


    private void BakeChunk(BoundsInt bounds, int chunkIndex, int chunkHeightInTiles)
    {
        int startYTile = bounds.y + chunkIndex * chunkHeightInTiles;
        int endYTile = Mathf.Min(bounds.y + (chunkIndex + 1) * chunkHeightInTiles, bounds.y + bounds.size.y);

        int widthInPixels = bounds.size.x * tilePixelSize;
        int heightInPixels = (endYTile - startYTile) * tilePixelSize;

        Debug.Log($"[TilemapBaker] Chunk {chunkIndex} size: {widthInPixels} x {heightInPixels} px");

        // Unity / GPU max texture size
        if (widthInPixels > 16384 || heightInPixels > 16384)
        {
            Debug.LogError($"[TilemapBaker] Chunk {chunkIndex} exceeds 16K texture size limit. Increase chunks!");
            return;
        }

        Texture2D atlas = new Texture2D(widthInPixels, heightInPixels, TextureFormat.RGBA32, false);

        // Clear to transparent
        Color[] clearPixels = new Color[widthInPixels * heightInPixels];
        for (int i = 0; i < clearPixels.Length; i++)
            clearPixels[i] = new Color(0, 0, 0, 0);

        atlas.SetPixels(0, 0, widthInPixels, heightInPixels, clearPixels);

        int writtenCount = 0;

        // Loop tilemap positions
        for (int yTile = startYTile; yTile < endYTile; yTile++)
        {
            for (int xTile = bounds.x; xTile < bounds.x + bounds.size.x; xTile++)
            {
                Vector3Int cell = new Vector3Int(xTile, yTile, 0);

                Sprite spr = sourceTilemap.GetSprite(cell);
                if (spr == null)
                    continue;

                Texture2D tex = spr.texture;

                // SAFETY CHECK: texture must be readable
                if (!tex.isReadable)
                {
                    Debug.LogError($"[TilemapBaker] Texture NOT readable: {tex.name}. Enable Read/Write on this texture!");
                    continue;
                }

                Rect r = spr.textureRect;

                Color[] pixels = tex.GetPixels(
                    Mathf.FloorToInt(r.x),
                    Mathf.FloorToInt(r.y),
                    Mathf.FloorToInt(r.width),
                    Mathf.FloorToInt(r.height)
                );

                int px = (xTile - bounds.x) * tilePixelSize;
                int py = (yTile - startYTile) * tilePixelSize;

                atlas.SetPixels(px, py, (int)r.width, (int)r.height, pixels);
                writtenCount++;
            }
        }

        atlas.Apply();

        string fileName = $"Baked_Tilemap_{chunkIndex}.png";
        string path = Application.dataPath + "/" + fileName;
        File.WriteAllBytes(path, atlas.EncodeToPNG());

        Debug.Log($"[TilemapBaker] Chunk {chunkIndex} saved → {fileName} | Tiles written: {writtenCount}");
    }
}