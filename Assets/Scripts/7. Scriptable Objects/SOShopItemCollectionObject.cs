using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Collection", menuName = "Item Collections/New Collection")]
public class SOShopItemCollectionObject : ScriptableObject
{
    [SerializeField] protected List<SOItemDataObject> itemsInCollection;
    [SerializeField] protected float fightsBeforeAppearance;


    public List<SOItemDataObject> ItemsInCollection { get => itemsInCollection; }
    public float FightsBeforeAppearance { get => fightsBeforeAppearance; }
}
