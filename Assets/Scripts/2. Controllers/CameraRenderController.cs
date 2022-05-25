using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRenderController : MonoBehaviour
{
    [SerializeField] private GameObject imageObject;
    //[SerializeField] private Material screenCaptureDestination;
    [SerializeField] private Image screenCaptureDestination;
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
        texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(CaptureFrame());
        }
    }

    private IEnumerator CaptureFrame()
    {
        yield return new WaitForEndOfFrame();

        Rect screenCapture = new Rect(0, 0, Screen.width, Screen.height);

        texture2D.ReadPixels(screenCapture, 0, 0, false);
        texture2D.Apply();
        CreateImage();
    }

    private void CreateImage()
    {
        Sprite screenCaptureSprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(.5f, .5f), 100f);
        screenCaptureDestination.sprite = screenCaptureSprite;
        imageObject.SetActive(true);
    }

    //[ContextMenu("Capture Screen")]
    //private void CaptureScreen()
    //{
    //    height = Screen.width;
    //    width = Screen.height;

    //    Debug.Log("Screen width and height: " + height + " / " + width);
    //    Debug.Log("Camera aspect ratio: " + mainCamera.aspect);
    //    depth = 24;

    //    renderTexture = new RenderTexture(width, height, depth);
    //    mainCamera.targetTexture = renderTexture;

    //    mainCamera.Render();

    //    texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
    //    Rect rect = new Rect(0, 0, width, height);

    //    RenderTexture currentRenderTexture = RenderTexture.active;
    //    RenderTexture.active = renderTexture;
    //    texture2D.ReadPixels(rect, 0, 0);
    //    texture2D.Apply();

    //    mainCamera.targetTexture = null;
    //    RenderTexture.active = currentRenderTexture;

    //    Destroy(renderTexture);

    //    Sprite sprite = Sprite.Create(texture2D, rect, Vector2.zero);
    //    //screenCaptureDestination.mainTexture = texture2D;
    //    imageObject.GetComponent<Image>().sprite = sprite;

    //    //imageObject.GetComponent<RectTransform>().pivot = new Vector2(.5f, .5f);
    //    imageObject.SetActive(true);
    //}
}
