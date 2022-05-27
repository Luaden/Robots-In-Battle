using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private Camera transitionCamera;
    [SerializeField] private GameObject firstPage;
    [SerializeField] private GameObject secondPage;
    [SerializeField] private Material screenCaptureDestination;
    [SerializeField] private Animator firstPageAnimator;
    [SerializeField] private Animator secondPageAnimator;
    private Texture2D texture2D;
    private Camera worldCamera;
    private GameObject worldCameraGameObject;

    public delegate void onFirstPageTurned();
    public static event onFirstPageTurned OnFirstPageTurned;

    public delegate void onSecondPageTurned();
    public static event onSecondPageTurned OnSecondPageTurned;

    [ContextMenu("Capture Screen")]
    public void LoadTransitionScene()
    {
        StartCoroutine(CaptureFrame());
    }

    public void FirstPageTurned()
    {
        OnFirstPageTurned?.Invoke();
        firstPageAnimator.ResetTrigger("isTurningPage");
    }

    public void OnSceneLoaded()
    {
        worldCamera = GameManager.instance.CurrentMainCanvas.worldCamera;
        worldCameraGameObject = worldCamera.gameObject;
        GameManager.instance.CurrentMainCanvas.worldCamera = transitionCamera;
        worldCamera.enabled = false;
        
        secondPageAnimator.SetTrigger("isTurningPageTwo");
    }

    public void SecondPageTurned()
    {
        secondPageAnimator.ResetTrigger("isTurningPageTwo");
        firstPage.SetActive(false);
        secondPage.SetActive(false);


        GameManager.instance.CurrentMainCanvas.worldCamera = worldCamera;
        worldCamera.enabled = true;
        transitionCamera.enabled = false;

        OnSecondPageTurned?.Invoke();
    }

    private void Start()
    {
        texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        GameManager.OnUpdatedMainCanvas += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        GameManager.OnUpdatedMainCanvas -= OnSceneLoaded;
    }

    private IEnumerator CaptureFrame()
    {
        yield return new WaitForEndOfFrame();

        Rect screenCapture = new Rect(0, 0, Screen.width, Screen.height);

        texture2D.ReadPixels(screenCapture, 0, 0, false);
        texture2D.Apply();
        screenCaptureDestination.mainTexture = texture2D;

        firstPage.SetActive(true);
        secondPage.SetActive(true);

        firstPageAnimator.SetTrigger("isTurningPage");
        transitionCamera.enabled = true;

        if (worldCamera != null)
            worldCamera.enabled = false;
        else if (Camera.main != null)
            Camera.main.enabled = false;
    }

    private void Update()
    {
        if(worldCameraGameObject != null)
        {
            transform.position = worldCameraGameObject.transform.position;
        }
    }
}
