using UnityEngine;
using System.IO;

public class Baking : MonoBehaviour
{
    public RenderTexture sourceRT;

    [ContextMenu("Save RenderTexture")]
    void SaveRT()
    {
        // Force render
        GetComponent<Camera>().Render();

        // Bind RT
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = sourceRT;

        // Read pixels
        Texture2D tex = new Texture2D(sourceRT.width, sourceRT.height, TextureFormat.RGBA32, false, false); // <- gamma (last "false" is key)
        tex.ReadPixels(new Rect(0, 0, sourceRT.width, sourceRT.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentRT;

        // Encode to PNG
        byte[] bytes = tex.EncodeToPNG();
        string path = Application.dataPath + "/BakedOuterWaterv3.png";
        File.WriteAllBytes(path, bytes);

        Debug.Log("Saved gamma-corrected PNG to " + path);
    }
}