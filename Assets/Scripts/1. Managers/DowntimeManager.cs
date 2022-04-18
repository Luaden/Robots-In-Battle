using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DowntimeManager : MonoBehaviour
{
    private static DowntimeManager instance;
    private ShopManager shopManager;
/*    private ShopCartSlotManager shopCartSlotManager;
    private ShopItemSlotManager shopItemSlotManager;*/
    public static DowntimeManager Instance { get { return instance; } }
    public ShopManager ShopManager { get => shopManager; }
/*    public ShopCartSlotManager ShopCartSlotManager { get => shopCartSlotManager; }
    public ShopItemSlotManager ShopItemSlotManager { get => shopItemSlotManager; }*/
    private void Awake()
    {
        if(instance != this && instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        shopManager = GetComponentInChildren<ShopManager>(true);
/*        shopItemSlotManager = FindObjectOfType<ShopItemSlotManager>(true);
        shopCartSlotManager = FindObjectOfType<ShopCartSlotManager>(true);*/
    }
}
