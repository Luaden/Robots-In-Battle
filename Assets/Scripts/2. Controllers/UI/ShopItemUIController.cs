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
    [SerializeField][Range(1.00f, 1.25f)] private float selectedScaleValue;
    [SerializeField] private GameObject cardPricetagObject;
    [SerializeField] private GameObject componentPricetagObject;
    [Space]
    [Header("Card Attributes")]
    [SerializeField] private Image cardBackground;
    [SerializeField] private Image cardImage;
    [SerializeField] private GameObject cardNameAttackObject;
    [SerializeField] private TMP_Text cardNameAttack;
    [SerializeField] private GameObject cardNameDefenseObject;
    [SerializeField] private TMP_Text cardNameDefense;
    [SerializeField] private GameObject cardNameNeutralObject;
    [SerializeField] private TMP_Text cardNameNeutral;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text cardCurrencyText;

    [Header("Channel Icons")]
    [SerializeField] private Image highChannelIcon;
    [SerializeField] private Image midChannelIcon;
    [SerializeField] private Image lowChannelIcon;

    [Header("Required Interaction Components")]
    [SerializeField] private RectTransform draggableRectTransform;
    [SerializeField] private CanvasGroup draggableCanvasGroup;
    [SerializeField] private GameObject cardHolder;

    [Header("Interaction Attributes")]
    [SerializeField] private float travelSpeed;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color fadeColor;
    [SerializeField] private MeshRenderer cardRenderer;
    private Transform previousParentObject;

    [Header("Card Frames")]
    [SerializeField] private Sprite attackFrame;
    [SerializeField] private Sprite defenseFrame;
    [SerializeField] private Sprite neutralFrame;
    [SerializeField] private Material attackMaterial;
    [SerializeField] private Material defenseMaterial;
    [SerializeField] private Material neutralMaterial;

    [Header("Card Icons")]
    [SerializeField] private Sprite punchIcon;
    [SerializeField] private Sprite kickIcon;
    [SerializeField] private Sprite specialIcon;
    [SerializeField] private Sprite counterIcon;
    [SerializeField] private Sprite guardIcon;
    [Space]
    [Header("Component Attributes")]
    [SerializeField] private TMP_Text componentName;
    [SerializeField] private TMP_Text componentCurrencyCostText;
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
    private string componentSpriteID;
    private string connectionComponentSpriteID;
    private string tertiaryComponentID;
    private ElementType componentElement;


    private bool notInMech = true;
    public bool isPickedUp = false;
    private SOItemDataObject baseSOItemDataObject;
    private MechComponentDataObject mechComponentDataObject;
    private BaseSlotController<ShopItemUIController> itemShopUISlotController;

    public bool NotInMech { get => notInMech; set => UpdateMechStatus(value); }
    public SOItemDataObject BaseSOItemDataObject { get => baseSOItemDataObject; }
    public MechComponentDataObject MechComponentDataObject { get => mechComponentDataObject; set => mechComponentDataObject = value; }
    public BaseSlotController<ShopItemUIController> ItemSlotController { get => itemShopUISlotController; set => UpdateItemSlot(value); }
    public Transform PreviousParentObject { get => previousParentObject; set => previousParentObject = value; }

    public void InitUI(SOItemDataObject sOItemData, MechComponentDataObject oldMechComponentData = null)
    {
        baseSOItemDataObject = sOItemData;
        itemType = sOItemData.ItemType;
        itemName = sOItemData.ItemName;
        itemDescription = sOItemData.ItemDescription;
        itemImage = sOItemData.ItemShopImage;
        currencyCost = sOItemData.CurrencyCost;
        chanceToSpawn = sOItemData.ChanceToSpawn;

        if(itemType == ItemType.Card)
        {
            energyText.text = sOItemData.EnergyCost.ToString();
            cardCurrencyText.text = currencyCost.ToString();

            switch (sOItemData.CardCategory)
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

            switch (sOItemData.CardType)
            {
                case CardType.Attack:
                    cardBackground.sprite = attackFrame;
                    cardNameAttack.text = sOItemData.CardName;
                    cardNameAttackObject.SetActive(true);
                    cardRenderer.material = attackMaterial;
                    break;
                case CardType.Defense:
                    cardBackground.sprite = defenseFrame;
                    cardNameDefense.text = sOItemData.CardName;
                    cardNameDefenseObject.SetActive(true);
                    cardRenderer.material = defenseMaterial;
                    break;
                case CardType.Neutral:
                    cardBackground.sprite = neutralFrame;
                    cardNameNeutral.text = sOItemData.CardName;
                    cardNameNeutralObject.SetActive(true);
                    cardRenderer.material = neutralMaterial;
                    break;
            }

            if (sOItemData.PossibleChannels.HasFlag(Channels.High))
                highChannelIcon.color = fullColor;
            if (sOItemData.PossibleChannels.HasFlag(Channels.Mid))
                midChannelIcon.color = fullColor;
            if (sOItemData.PossibleChannels.HasFlag(Channels.Low))
                lowChannelIcon.color = fullColor;

            cardUIObject.SetActive(true);
        }
        else
        {
            componentName.text = itemName;
            componentSpriteID = sOItemData.PrimaryComponentSpriteID;
            connectionComponentSpriteID = sOItemData.SecondaryComponentSpriteID;
            tertiaryComponentID = sOItemData.TertiaryComponentID;
            componentCurrencyCostText.text = currencyCost.ToString();
            mechComponentDataObject = oldMechComponentData;

            switch (sOItemData.ComponentElement)
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

            componentUIObject.SetActive(true);
        }

        notInMech = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DowntimeManager.instance != null)
            DowntimeManager.instance.PopupUIManager.HandlePopup(baseSOItemDataObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (DowntimeManager.instance != null)
            DowntimeManager.instance.PopupUIManager.ClearAllPopups();
        else if (CombatManager.instance != null)
            CombatManager.instance.PopupUIManager.ClearAllPopups();
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

        if (DowntimeManager.instance != null && (DowntimeManager.instance.ShopManager.CurrentItemSelected == null || DowntimeManager.instance.ShopManager.CurrentItemSelected == this))
        {
            transform.localScale = Vector3.one * selectedScaleValue;
            DowntimeManager.instance.ShopManager.CurrentItemSelected = this;
            return;
        }

        if (DowntimeManager.instance != null && DowntimeManager.instance.ShopManager.CurrentItemSelected != this)
        {
            ShopItemUIController shopItem = DowntimeManager.instance.ShopManager.CurrentItemSelected;
            shopItem.transform.localScale = Vector3.one;
            transform.localScale = Vector3.one * selectedScaleValue;

            DowntimeManager.instance.ShopManager.CurrentItemSelected = this;
            return;
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

    private void UpdateMechStatus(bool value)
    {
        notInMech = value;

        if(notInMech && itemType == ItemType.Component)
            componentPricetagObject.SetActive(true);
        else if (!notInMech && itemType == ItemType.Component)
            componentPricetagObject.SetActive(false);

        if (CombatManager.instance != null)
        {
            cardPricetagObject.SetActive(false);
            componentPricetagObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if(DowntimeManager.instance != null)
        {
            ShopItemUIController item = DowntimeManager.instance.ShopManager.CurrentItemSelected;
            if (item == this)
                item.transform.localScale = Vector3.one;
        }
    }
}
