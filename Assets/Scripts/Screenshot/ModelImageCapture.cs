using UnityEngine;
using System.IO;

public class ModelImageCapture : MonoBehaviour
{
    [SerializeField] private Camera renderCamera;
    [SerializeField] private int imageWidth = 256;
    [SerializeField] private int imageHeight = 256;

    public Texture2D CaptureModelImage() {
        // Create a temporary camera if none is assigned
        Camera cam = renderCamera;
        if (cam == null)
        {
            GameObject tempCam = new GameObject("TempRenderCamera");
            cam = tempCam.AddComponent<Camera>();
            cam.enabled = false;
        }

        // Configure camera for transparent background
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0); // Fully transparent background

        // Create render texture
        RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        renderTexture.format = RenderTextureFormat.ARGB32; // Support alpha channel
        Texture2D resultImage = new Texture2D(imageWidth, imageHeight, TextureFormat.RGBA32, false);

        // Store the current camera settings
        RenderTexture previousRT = RenderTexture.active;
        cam.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;

        // Clear with transparent background and render
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        cam.Render();
        resultImage.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        resultImage.Apply();

        // Cleanup
        cam.targetTexture = null;
        RenderTexture.active = previousRT;
        if (renderCamera == null)
        {
            Destroy(cam.gameObject);
        }
        renderTexture.Release();

        return resultImage;
    }

    private Bounds GetObjectBounds(GameObject obj)
    {
        Bounds bounds = new Bounds(obj.transform.position, Vector3.zero);
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        
        return bounds;
    }

    private float GetCameraDistance(Bounds bounds, float fov)
    {
        float maxDimension = Mathf.Max(bounds.size.x, bounds.size.y);
        return (maxDimension / 2.0f) / Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
    }

    public void SaveTextureToFile(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToPNG();
        string filePath = Path.Combine(Application.dataPath, fileName);
        File.WriteAllBytes(filePath, bytes);
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }
}