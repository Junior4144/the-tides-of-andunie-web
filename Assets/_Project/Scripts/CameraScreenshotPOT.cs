using UnityEngine;
using System.IO;

public class CameraScreenshotPOT : MonoBehaviour
{
    [Header("Screenshot Settings")]
    public Camera targetCamera;
    public int potWidth = 2048;   // MUST be power of two
    public int potHeight = 2048;  // MUST be power of two
    public string fileName = "CameraScreenshot_POT.png";

    [Header("Keybind")]
    public KeyCode captureKey = KeyCode.P;

    private void Update()
    {
        if (Input.GetKeyDown(captureKey))
        {
            CapturePOT();
        }
    }

    public void CapturePOT()
    {
        // Create a render texture at desired POT resolution
        RenderTexture rt = new RenderTexture(potWidth, potHeight, 24);
        targetCamera.targetTexture = rt;

        // Render the camera into the RT
        Texture2D image = new Texture2D(potWidth, potHeight, TextureFormat.RGBA32, false);
        targetCamera.Render();

        // Read pixel data from RT into Texture2D
        RenderTexture.active = rt;
        image.ReadPixels(new Rect(0, 0, potWidth, potHeight), 0, 0);
        image.Apply();

        // Cleanup
        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // Encode to PNG (lossless, high quality)
        byte[] bytes = image.EncodeToPNG();

        // Save to disk (Project folder)
        string folderPath = Application.dataPath + "/Screenshots/";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fullPath = folderPath + fileName;
        File.WriteAllBytes(fullPath, bytes);

        Debug.Log($"[CameraScreenshotPOT] Saved screenshot: {fullPath} ({potWidth}x{potHeight})");
    }
}
