using UnityEngine;
using System.IO;

public class Baking : MonoBehaviour
{
    public RenderTexture sourceRT;

    [Header("Output")]
    public string fileName = "4kTotalMap.png";

    [ContextMenu("Save RenderTexture")]
    void SaveRT()
    {
        GetComponent<Camera>().Render();
        
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = sourceRT;
        
        Texture2D tex = new Texture2D(
            sourceRT.width,
            sourceRT.height,
            TextureFormat.RGBA32,
            false,
            true
        );

        tex.ReadPixels(new Rect(0, 0, sourceRT.width, sourceRT.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentRT;
        
        string fullPath = Path.Combine(Application.dataPath, fileName);
        
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(fullPath, bytes);

        Debug.Log("Saved PNG to " + fullPath);
    }
}
