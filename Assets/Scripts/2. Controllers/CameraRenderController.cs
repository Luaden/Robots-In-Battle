using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRenderController : MonoBehaviour
{
    [SerializeField] private GameObject imageObject;
    [SerializeField] private Material screenCaptureDestination;
    [SerializeField] private Canvas mainCanvas;

    private Camera mainCamera;
    private int height;
    private int width;
    private int depth;
    private RenderTexture renderTexture;
    private Texture2D texture2D;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    [ContextMenu("Capture Screen")]
    private void CaptureScreen()
    {
        //mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        height = Screen.width;
        width = Screen.height;
        depth = 24;

        renderTexture = new RenderTexture(width, height, depth);
        mainCamera.targetTexture = renderTexture;

        texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Rect rect = new Rect(0, 0, width, height);

        mainCamera.Render();

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(rect, 0, 0);
        texture2D.Apply();

        mainCamera.targetTexture = null;
        RenderTexture.active = currentRenderTexture;

        Destroy(renderTexture);

        //Sprite sprite = Sprite.Create(texture2D, rect, Vector2.zero);
        screenCaptureDestination.mainTexture = texture2D;
        //imageObject.GetComponent<Image>().sprite = sprite;

        //imageObject.GetComponent<RectTransform>().pivot = new Vector2(.5f, .5f);
        //mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        imageObject.SetActive(true);
    }
}
