using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DowntimeManager : MonoBehaviour
{
    private static DowntimeManager instance;

    private CardShopManager cardShopManager;
    //private ComponentShopManager componentShopManager;

    public static DowntimeManager Instance { get { return instance; } }
    public CardShopManager CardShopManager { get => cardShopManager; }
    //public ComponentShopManager ComponentShopManager { get => componentShopManager; }



    // old
    private ShopManager shopManager;
    public ShopManager ShopManager { get => shopManager; }

    private void Awake()
    {
        if(instance != this && instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        cardShopManager = GetComponentInChildren<CardShopManager>(true);



        // old
        shopManager = GetComponentInChildren<ShopManager>(true);
    }
}
