using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopController shopController;

    public ShopController ShopController { get => shopController; }
}
