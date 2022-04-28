using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    //private InventorySlotManager inventorySlotManager;
    private List<MechComponentDataObject> inventoryList;
    [SerializeField] protected InventorySlotManager InventorySlotManager;
    [SerializeField] protected GameObject itemPrefab; // contains TMP texts and an image

    private void CreateItemToDisplay(MechComponentDataObject mechComponentData)
    {
        MechComponentUIObject mechComponentUIObject = new MechComponentUIObject(mechComponentData);

        GameObject inventoryUIGameObject;
        inventoryUIGameObject = Instantiate(itemPrefab, transform);
        inventoryUIGameObject.transform.position = transform.position;

        mechComponentUIObject.MechComponentUIController = inventoryUIGameObject;

        InventoryUIController inventoryUIController = inventoryUIGameObject.GetComponent<InventoryUIController>();
        EquipmentUIController equipmentUIController = inventoryUIGameObject.GetComponent<EquipmentUIController>();

        inventoryUIController.InitUI(mechComponentUIObject);
        equipmentUIController.InitUI(mechComponentUIObject);

        inventoryUIController.enabled = true;

        DowntimeManager.instance.InventoryManager.InventoryController.InventorySlotManager.AddItemToCollection(inventoryUIController, null);

        inventoryUIGameObject.SetActive(true);
    }

    public void UpdateItemsToDisplay()
    {
        if (inventoryList == null)
            inventoryList = new List<MechComponentDataObject>();

        if(GameManager.instance.PlayerInventoryController == null)
        {
            Debug.Log("instance of PlayerInventoryController is null");
            return;
        }
        if (GameManager.instance.PlayerInventoryController.PlayerInventory == null)
        {
            Debug.Log("instance of PlayerInventoryController.PlayerInventory is null");
            return;
        }
        List<MechComponentDataObject> mechComponentDatas = GameManager.instance.PlayerInventoryController.PlayerInventory;
        for (int i = 0; i < mechComponentDatas.Count; i++)
        {
            if (!inventoryList.Contains(mechComponentDatas[i]))
            {
                CreateItemToDisplay(mechComponentDatas[i]);
                inventoryList.Add(mechComponentDatas[i]);
            }
        }
    }

}
