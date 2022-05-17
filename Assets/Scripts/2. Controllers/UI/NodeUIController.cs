using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
                              IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
                              IEndDragHandler, IDragHandler
{
    [SerializeField] protected TMP_Text fighterName;
    [SerializeField] protected TMP_Text fighterDescription;
    [SerializeField] protected TMP_Text health;
    [SerializeField] protected TMP_Text energy;

    [SerializeField] private Image fighterHair;
    [SerializeField] private Image fighterEyes;
    [SerializeField] private Image fighterNose;
    [SerializeField] private Image fighterMouth;
    [SerializeField] private Image fighterClothes;
    [SerializeField] private Image fighterBody;

    private bool isPickedUp = false;
    public Transform previousParentObject;
    [SerializeField] protected float travelSpeed;

    private RectTransform draggableRectTransform;
    private CanvasGroup draggableCanvasGroup;


    [SerializeField] private GameObject nodeDataObject;
    public GameObject NodeDataObject { get => nodeDataObject; }

    [SerializeField] private BaseSlotController<NodeUIController> nodeSlotController;
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
    public void InitUI(NodeDataObject newNodeData)
    {
        fighterName.text = newNodeData.FighterName;

        fighterHair.sprite = newNodeData.FighterDataObject.FighterCompleteCharacter.PilotUIObject.FighterHair;
        fighterEyes.sprite = newNodeData.FighterDataObject.FighterCompleteCharacter.PilotUIObject.FighterEyes;
        fighterNose.sprite = newNodeData.FighterDataObject.FighterCompleteCharacter.PilotUIObject.FighterNose;
        fighterMouth.sprite = newNodeData.FighterDataObject.FighterCompleteCharacter.PilotUIObject.FighterMouth;
        fighterClothes.sprite = newNodeData.FighterDataObject.FighterCompleteCharacter.PilotUIObject.FighterClothes;
        fighterBody.sprite = newNodeData.FighterDataObject.FighterCompleteCharacter.PilotUIObject.FighterBody;

        nodeDataObject = newNodeData.gameObject;
        newNodeData.NodeUIController = this.GetComponent<NodeUIController>();
        //nodeData
        // information about the mech
        // name, hp, description?

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

        if(eventData.pointerDrag.GetComponent<NodeDataObject>() != null)
        {
            
            eventData.pointerDrag.GetComponent<NodeDataObject>().NextNode = nodeSlotController.GetComponent<NodeDataObject>().NextNode;
            eventData.pointerDrag.GetComponent<NodeDataObject>().PairNode = nodeSlotController.GetComponent<NodeDataObject>().PairNode;

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetParent(nodeSlotController.SlotManager.MainCanvas.transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // popup appears
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // popup removed
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

    public void SetInactive()
    {
        this.enabled = false;
        
        GetComponent<Image>().color = Color.red;
    }
    public void SetActive()
    {
        this.enabled = true;
    }

}
