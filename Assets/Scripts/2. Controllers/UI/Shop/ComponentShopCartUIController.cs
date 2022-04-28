using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ComponentShopCartUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
                              IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
                              IEndDragHandler, IDragHandler
{

    [SerializeField] protected Image componentImage;
    [SerializeField] protected TMP_Text componentName;
    [SerializeField] protected TMP_Text currencyCost;
    [SerializeField] protected TMP_Text timeCost;

    private RectTransform draggableRectTransform;
    private CanvasGroup draggableCanvasGroup;
    private Transform previousParentObject;

    [SerializeField] protected float travelSpeed;
    private bool isPickedUp = false;

    [Header("Component Icons")]
    [SerializeField] protected Sprite headIcon;
    [SerializeField] protected Sprite torsoIcon;
    [SerializeField] protected Sprite armsIcon;
    [SerializeField] protected Sprite legsIcon;

    private ShopItemUIObject shopItemUIObject;
    public ShopItemUIObject ShopItemUIObject { get => shopItemUIObject; }

    private BaseSlotController<ComponentShopCartUIController> componentShopSlotUIController;
    public BaseSlotController<ComponentShopCartUIController> ComponentShopSlotUIController
    {
        get => componentShopSlotUIController;
        set => UpdateItemSlot(value);
    }
    public Transform PreviousParentObject
    {
        get => previousParentObject;
        set => previousParentObject = value;
    }

    public void InitUI(ShopItemUIObject shopItemUIObject)
    {

        timeCost.gameObject.SetActive(true);
        currencyCost.gameObject.SetActive(true);

        componentName.text = shopItemUIObject.ComponentName;
        switch (shopItemUIObject.ComponentType)
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

        currencyCost.text = shopItemUIObject.CurrencyCost.ToString();
        timeCost.text = shopItemUIObject.TimeCost.ToString();
        componentImage.sprite = shopItemUIObject.ComponentSprite;

        this.shopItemUIObject = shopItemUIObject;
        shopItemUIObject.ShopItemUIController = this.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        componentShopSlotUIController.HandleDrag(eventData);

    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        isPickedUp = true;
        draggableCanvasGroup.blocksRaycasts = false;
        draggableCanvasGroup.alpha = .6f;
        componentShopSlotUIController.SlotManager.RemoveItemFromCollection(this);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isPickedUp = false;
        draggableCanvasGroup.blocksRaycasts = true;
        draggableCanvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetParent(componentShopSlotUIController.SlotManager.MainCanvas.transform);
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
        if (isPickedUp || componentShopSlotUIController == null)
            return;

        if (transform.parent == null)
            transform.SetParent(previousParentObject);

        draggableRectTransform.position =
            Vector3.MoveTowards(draggableRectTransform.position,
            ComponentShopSlotUIController.gameObject.GetComponent<RectTransform>().position,
            travelSpeed * Time.deltaTime);
    }
    private void UpdateItemSlot(BaseSlotController<ComponentShopCartUIController> newSlot)
    {
        componentShopSlotUIController = newSlot;
        transform.SetParent(newSlot.transform);
        previousParentObject = newSlot.transform;
    }
}
