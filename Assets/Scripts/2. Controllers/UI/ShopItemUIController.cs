using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ShopItemUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
                              IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
                              IEndDragHandler, IDragHandler, IDropHandler
{

    [SerializeField] private GameObject cardUIObject;
    [SerializeField] private GameObject componentUIObject;
    [Space]
    [Header("Card Attributes")]
    [SerializeField] private Image cardFrame;
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardTimeCostText;
    [SerializeField] private TMP_Text cardCurrencyCostText;

    [Space]
    [SerializeField] private Image componentFrame;
    [SerializeField] private Image componentImage;
    [SerializeField] private TMP_Text componentName;
    [SerializeField] private TMP_Text componentTimeCostText;
    [SerializeField] private TMP_Text componentCurrencyCostText;

    [Header("Channel Icons")]
    [SerializeField] private Image highChannelIcon;
    [SerializeField] private Image midChannelIcon;
    [SerializeField] private Image lowChannelIcon;

    [Header("Required Interaction Components")]
    [SerializeField] private RectTransform draggableRectTransform;
    [SerializeField] private CanvasGroup draggableCanvasGroup;

    [Header("Interaction Attributes")]
    [SerializeField] private float travelSpeed;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color fadeColor;

    [Header("Card Frames")]
    [SerializeField] private Sprite attackFrame;
    [SerializeField] private Sprite defenseFrame;
    [SerializeField] private Sprite neutralFrame;

    [Header("Card Icons")]
    [SerializeField] private Sprite punchIcon;
    [SerializeField] private Sprite kickIcon;
    [SerializeField] private Sprite specialIcon;
    [SerializeField] private Sprite counterIcon;
    [SerializeField] private Sprite guardIcon;
    [Space]
    [Header("Component Attributes")]
    [SerializeField] private Image elementIcon;
    [SerializeField] private Sprite fireElement;
    [SerializeField] private Sprite iceElement;
    [SerializeField] private Sprite acidElement;
    [SerializeField] private Sprite plasmaElement;



    //Item Attributes    
    private ItemType itemType;
    private string itemName;
    private string itemDescription;
    private Sprite itemImage;
    private float timeCost;
    private int currencyCost;
    private int chanceToSpawn;

    //Base Component Attributes
    private MechComponent componentType;
    private int componentHP;
    private int componentEnergy;
    private ElementType componentElement;
    private float cDMFromComponent;
    private float cDMToComponent;
    private int extraElementStacks;
    private int energyGainModifier;

    //Card Attributes
    private CardType cardType;
    private CardCategory cardCategory;
    private Channels possibleChannels;
    private AffectedChannels affectedChannels;
    private int energyCost;
    private int baseDamage;
    private AnimationType animationType;

    public bool isPickedUp = false;
    [SerializeField] public bool notInMech = true;
    private Transform previousParentObject; 
    private SOItemDataObject baseSOItemDataObject;
    private MechComponentDataObject mechComponentDataObject;
    private BaseSlotController<ShopItemUIController> itemShopUISlotController;

    public SOItemDataObject BaseSOItemDataObject { get => baseSOItemDataObject; }
    public MechComponentDataObject MechComponentDataObject { get => mechComponentDataObject; set => mechComponentDataObject = value; }
    public BaseSlotController<ShopItemUIController> ItemSlotController { get => itemShopUISlotController; set => UpdateItemSlot(value); }
    public Transform PreviousParentObject { get => previousParentObject; set => previousParentObject = value; }

    public void InitUI(SOItemDataObject shopItemUIObject, MechComponentDataObject oldMechComponentData = null)
    {
        baseSOItemDataObject = shopItemUIObject;
        itemType = shopItemUIObject.ItemType;
        itemName = shopItemUIObject.ItemName;
        itemDescription = shopItemUIObject.ItemDescription;
        itemImage = shopItemUIObject.ItemImage;
        timeCost = shopItemUIObject.TimeCost;
        currencyCost = shopItemUIObject.CurrencyCost;
        chanceToSpawn = shopItemUIObject.ChanceToSpawn;

        if(itemType == ItemType.Card)
        {
            cardName.text = itemName;
            switch (shopItemUIObject.CardCategory)
            {
                case CardCategory.Punch:
                    cardImage.sprite = punchIcon;
                    cardImage.SetNativeSize();
                    break;
                case CardCategory.Kick:
                    cardImage.sprite = kickIcon;
                    cardImage.SetNativeSize();
                    break;
                case CardCategory.Special:
                    cardImage.sprite = specialIcon;
                    cardImage.SetNativeSize();
                    break;
                case CardCategory.Guard:
                    cardImage.sprite = guardIcon;
                    cardImage.SetNativeSize();
                    break;
                case CardCategory.Counter:
                    cardImage.sprite = counterIcon;
                    cardImage.SetNativeSize();
                    break;
            }

            switch (shopItemUIObject.CardType)
            {
                case CardType.Attack:
                    cardFrame.sprite = attackFrame;
                    break;
                case CardType.Defense:
                    cardFrame.sprite = defenseFrame;
                    break;
                case CardType.Neutral:
                    cardFrame.sprite = neutralFrame;
                    break;
            }

            if (shopItemUIObject.PossibleChannels.HasFlag(Channels.High))
                highChannelIcon.color = fullColor;
            else
                highChannelIcon.color = fadeColor;
            if (shopItemUIObject.PossibleChannels.HasFlag(Channels.Mid))
                midChannelIcon.color = fullColor;
            else
                midChannelIcon.color = fadeColor;
            if (shopItemUIObject.PossibleChannels.HasFlag(Channels.Low))
                lowChannelIcon.color = fullColor;
            else
                lowChannelIcon.color = fadeColor;

            cardTimeCostText.text = timeCost.ToString();
            cardCurrencyCostText.text = currencyCost.ToString();
            cardUIObject.SetActive(true);
        }
        else
        {
            componentImage.sprite = itemImage;
            componentName.text = itemName;
            componentCurrencyCostText.text = currencyCost.ToString();
            componentTimeCostText.text = timeCost.ToString();
            componentUIObject.SetActive(true);
            mechComponentDataObject = oldMechComponentData;

            switch (componentElement)
            {
                case ElementType.None:
                    break;
                case ElementType.Fire:
                    elementIcon.sprite = fireElement;
                    elementIcon.color = fullColor;
                    break;
                case ElementType.Ice:
                    elementIcon.sprite = iceElement;
                    elementIcon.color = fullColor;
                    break;
                case ElementType.Plasma:
                    elementIcon.sprite = plasmaElement;
                    elementIcon.color = fullColor;
                    break;
                case ElementType.Acid:
                    elementIcon.sprite = acidElement;
                    elementIcon.color = fullColor;
                    break;
            }
        }

        notInMech = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DowntimeManager.instance.PopupUIManager.HandlePopup(baseSOItemDataObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DowntimeManager.instance.PopupUIManager.ClearAllPopups();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (notInMech)
        {
            isPickedUp = true;
            draggableCanvasGroup.blocksRaycasts = false;
            draggableCanvasGroup.alpha = .6f;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(notInMech)
        {
            itemShopUISlotController.HandleDrag(eventData);
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isPickedUp = false;
        draggableCanvasGroup.blocksRaycasts = true;
        draggableCanvasGroup.alpha = 1f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!notInMech)
            itemShopUISlotController.OnDrop(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(notInMech)
        {
            isPickedUp = true;
            transform.SetParent(itemShopUISlotController.SlotManager.MainCanvas.transform);
        }

        if (DowntimeManager.instance.ShopManager.CurrentItemSelected == null)
        {
            transform.localScale = Vector3.one * 1.5f;
            DowntimeManager.instance.ShopManager.CurrentItemSelected = this;
            return;
        }

        if(DowntimeManager.instance.ShopManager.CurrentItemSelected != this)
        {
            DowntimeManager.instance.ShopManager.CurrentItemSelected.transform.localScale = Vector3.one;

            transform.localScale = Vector3.one * 1.5f;
            DowntimeManager.instance.ShopManager.CurrentItemSelected = this;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(notInMech)
        {
            isPickedUp = false;
            transform.SetParent(previousParentObject);
        }
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
        if (isPickedUp)
            return;

        if (transform.parent == null)
            transform.SetParent(PreviousParentObject);

        if (transform.parent != null)
        {
            draggableRectTransform.position =
                    Vector3.MoveTowards(draggableRectTransform.position, transform.parent.position, travelSpeed * FindObjectOfType<Canvas>().scaleFactor * Time.deltaTime);
        }
    }

    private void UpdateItemSlot(BaseSlotController<ShopItemUIController> newSlot)
    {
        itemShopUISlotController = newSlot;

        if (newSlot != null)
        {
            previousParentObject = newSlot.transform;
            transform.SetParent(newSlot.transform);
        }
    }
}
