using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour
{
    private List<MechComponentDataObject> currentMechEquipment;
    [SerializeField] protected EquipmentSlotManager EquipmentSlotManager;
    [SerializeField] protected GameObject itemPrefab;
    //private List<EquipmentSlotController> slotControllers;


    private void Start()
    {
        OnCreation();
    }
  /*  private void CreateItemToDisplay(MechComponentDataObject mechComponentData)
    {
        if(slotControllers == null)
        {
            slotControllers = new List<EquipmentSlotController>();
            for(int i = 0; i < currentMechEquipment.Count; i++)
            {
                GameObject slotGO = new GameObject(name: "Slot " + i, typeof(EquipmentSlotController), typeof(Image));

                EquipmentSlotController addedSlot = slotGO.GetComponent<EquipmentSlotController>();

                EquipmentSlotManager slotManager = DowntimeManager.instance.InventoryManager.EquipmentController.EquipmentSlotManager;

                addedSlot.SetSlotManager(slotManager);
                slotManager.AddSlotToList(addedSlot);

                Vector3 scale = slotGO.transform.localScale;
                slotGO.transform.SetParent(slotManager.transform);
                slotGO.transform.localScale = scale;
            }
        }

        MechComponentUIObject mechComponentUIObject = new MechComponentUIObject(mechComponentData);

        GameObject equipmentUIGameObject;
        equipmentUIGameObject = Instantiate(itemPrefab, transform);
        equipmentUIGameObject.transform.position = transform.position;

        mechComponentUIObject.MechComponentUIController = equipmentUIGameObject;

        equipmentUIGameObject.AddComponent<InventoryUIController>();
        equipmentUIGameObject.AddComponent<EquipmentUIController>();

        InventoryUIController inventoryUIController = equipmentUIGameObject.GetComponent<InventoryUIController>();
        EquipmentUIController equipmentUIController = equipmentUIGameObject.GetComponent<EquipmentUIController>();

        inventoryUIController.InitUI(mechComponentUIObject);
        equipmentUIController.InitUI(mechComponentUIObject);

        inventoryUIController.enabled = false;

        Debug.Log(DowntimeManager.instance);
        Debug.Log(DowntimeManager.instance.InventoryManager);
        Debug.Log(DowntimeManager.instance.InventoryManager.EquipmentController);
        Debug.Log(DowntimeManager.instance.InventoryManager.EquipmentController.EquipmentSlotManager);

        DowntimeManager.instance.InventoryManager.EquipmentController.EquipmentSlotManager.AddItemToCollection(equipmentUIController, null);

        equipmentUIGameObject.SetActive(true);
    }*/
    private void CreateItemsToDisplay(MechComponentDataObject[] mechComponentData)
    {

        for (int i = 0; i < mechComponentData.Length; i++)
        {
            GameObject slotGO = new GameObject(name: "Slot " + i, typeof(EquipmentSlotController), typeof(Image));

            EquipmentSlotController addedSlot = slotGO.GetComponent<EquipmentSlotController>();

            EquipmentSlotManager slotManager = DowntimeManager.instance.InventoryManager.EquipmentController.EquipmentSlotManager;

            addedSlot.SetSlotManager(slotManager);
            slotManager.AddSlotToList(addedSlot);
            addedSlot.SetComponentType((MechComponent)i + 1);

            Vector3 scale = slotGO.transform.localScale;
            slotGO.transform.SetParent(slotManager.transform);
            slotGO.transform.localScale = scale;

            MechComponentUIObject mechComponentUIObject = new MechComponentUIObject(mechComponentData[i]);

            GameObject equipmentUIGameObject;
            equipmentUIGameObject = Instantiate(itemPrefab, transform);
            equipmentUIGameObject.transform.position = slotGO.transform.position;

            mechComponentUIObject.MechComponentUIController = equipmentUIGameObject;

            InventoryUIController inventoryUIController = equipmentUIGameObject.GetComponent<InventoryUIController>();
            EquipmentUIController equipmentUIController = equipmentUIGameObject.GetComponent<EquipmentUIController>();

            // todo: change
            inventoryUIController.InitUI(mechComponentUIObject);
            equipmentUIController.InitUI(mechComponentUIObject);

            equipmentUIController.enabled = true;

            slotManager.AddItemToCollection(equipmentUIController, addedSlot);

            equipmentUIGameObject.SetActive(true);

        }
            Debug.Log(DowntimeManager.instance.InventoryManager.EquipmentController.EquipmentSlotManager.SlotList.Count);
    }

    private void OnCreation()
    {
        MechObject currentPlayerMech = GameManager.instance.PlayerMechController.PlayerMech;

        currentMechEquipment.Add(currentPlayerMech.MechHead);
        currentMechEquipment.Add(currentPlayerMech.MechTorso);
        currentMechEquipment.Add(currentPlayerMech.MechArms);
        currentMechEquipment.Add(currentPlayerMech.MechLegs);

        CreateItemsToDisplay(currentMechEquipment.ToArray());

    }

    public void UpdateItemsToDisplay()
    {
        if (currentMechEquipment == null)
            currentMechEquipment = new List<MechComponentDataObject>();

        //MechObject currentPlayerMech = GameManager.instance.PlayerMechController.PlayerMech;

/*         if(!currentMechEquipment.Contains(currentPlayerMech.MechHead) ||
            !currentMechEquipment.Contains(currentPlayerMech.MechTorso) ||
            !currentMechEquipment.Contains(currentPlayerMech.MechArms) ||
            !currentMechEquipment.Contains(currentPlayerMech.MechLegs))
        {
            currentMechEquipment.Add(currentPlayerMech.MechHead);
            currentMechEquipment.Add(currentPlayerMech.MechTorso);
            currentMechEquipment.Add(currentPlayerMech.MechArms);
            currentMechEquipment.Add(currentPlayerMech.MechLegs);

            CreateItemToDisplay(currentPlayerMech.MechHead);
            CreateItemToDisplay(currentPlayerMech.MechTorso);
            CreateItemToDisplay(currentPlayerMech.MechArms);
            CreateItemToDisplay(currentPlayerMech.MechLegs);
        }*/

    }
}
