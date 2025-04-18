using UnityEngine;

public class AutoModelCapture : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string fileName = "ModelCapture.png";
    private ModelImageCapture imageCapture;

    void Start()
    {
        imageCapture = GetComponent<ModelImageCapture>();
        if (imageCapture == null)
        {
            Debug.LogError("ModelImageCapture component not found!");
            return;
        }


        Texture2D capturedImage = imageCapture.CaptureModelImage();
        imageCapture.SaveTextureToFile(capturedImage, fileName);
        Debug.Log($"Model image saved to Assets/{fileName}");
    }
}