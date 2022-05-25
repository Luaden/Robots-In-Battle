using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private GameObject firstPage;
    [SerializeField] private GameObject secondPage;
    [SerializeField] private Material screenCaptureDestination;
    [SerializeField] private Animator firstPageAnimator;
    [SerializeField] private Animator secondPageAnimator;
    private Texture2D texture2D;

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
        firstPage.SetActive(false);
        secondPageAnimator.SetTrigger("isTurningPageTwo");
        Debug.Log("First page turn complete.");
    }

    public void SecondPageTurned()
    {
        Debug.Log("Second page turning.");
        secondPage.SetActive(false);
        OnSecondPageTurned?.Invoke();
    }

    private void Start()
    {
        texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
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
        Debug.Log("Turning first page.");
    }
}
