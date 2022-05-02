using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
                              IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
                              IEndDragHandler, IDragHandler
{
    [SerializeField] protected Image componentImage;
    [SerializeField] protected TMP_Text componentName;

    private bool isPickedUp = false;
    private Transform previousParentObject;
    [SerializeField] protected float travelSpeed;

    private RectTransform draggableRectTransform;
    private CanvasGroup draggableCanvasGroup;

/*    [Header("Component Icons")]
    [SerializeField] protected Sprite headIcon;
    [SerializeField] protected Sprite torsoIcon;
    [SerializeField] protected Sprite armsIcon;
    [SerializeField] protected Sprite legsIcon;*/


/*    private MechComponentUIObject mechComponentUIObject;
    public MechComponentUIObject MechComponentUIObject { get => mechComponentUIObject; }*/

    private BaseSlotController<NodeUIController> nodeSlotController;
    public BaseSlotController<NodeUIController> NodeSlotController
    {
        get => nodeSlotController;
        set => UpdateItemSlot(value);
    }
    public Transform PreviousParentObject
    {
        get => previousParentObject;
        set => previousParentObject = value;
    }

    private void OnEnable()
    {
        isPickedUp = false;
    }
    private void OnDisable()
    {
        isPickedUp = false;
    }

    private void Start()
    {
        
    }
    public void InitUI(MechComponentUIObject mechComponent)
    {
/*        componentName.text = mechComponent.ComponentName;
        switch (mechComponent.ComponentType)
        {
            case MechComponent.None:
                break;
            case MechComponent.Head:
                componentImage.sprite = headIcon;
                break;
            case MechComponent.Torso:
                componentImage.sprite = torsoIcon;
                break;
            case MechComponent.Arms:
                componentImage.sprite = armsIcon;
                break;
            case MechComponent.Legs:
                componentImage.sprite = legsIcon;
                break;
            case MechComponent.Back:
                break;
            default:
                break;
        }

        this.mechComponentUIObject = mechComponent;
        mechComponent.MechComponentUIController = this.gameObject;*/
    }

    public void OnDrag(PointerEventData eventData)
    {
        nodeSlotController.HandleDrag(eventData);

    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        isPickedUp = true;
        draggableCanvasGroup.blocksRaycasts = false;
        draggableCanvasGroup.alpha = .6f;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isPickedUp = false;
        draggableCanvasGroup.blocksRaycasts = true;
        draggableCanvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetParent(nodeSlotController.SlotManager.MainCanvas.transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.SetParent(previousParentObject);
    }

    private void Awake()
    {
        draggableRectTransform = GetComponent<RectTransform>();
        draggableCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        MoveToSlot();
    }

    private void MoveToSlot()
    {
        if (isPickedUp || nodeSlotController == null)
            return;

        if (transform.parent == null)
            transform.SetParent(previousParentObject);

        draggableRectTransform.position =
            Vector3.MoveTowards(draggableRectTransform.position,
            nodeSlotController.gameObject.GetComponent<RectTransform>().position,
            travelSpeed * Time.deltaTime);
    }
    private void UpdateItemSlot(BaseSlotController<NodeUIController> newSlot)
    {
        nodeSlotController = newSlot;
        transform.SetParent(newSlot.transform);
        previousParentObject = newSlot.transform;
    }

}
