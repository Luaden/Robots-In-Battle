using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collection", menuName = "Cards or Components/New Collection")]
public class ShopItemCollectionObject : ScriptableObject
{
    [SerializeField] protected ItemType itemType;
    [SerializeField] protected List<SOItemDataObject> itemsInCollection;
    [SerializeField] protected float fightsBeforeAppearance;


    public List<SOItemDataObject> ItemsInCollection { get => itemsInCollection; }
    public ItemType CollectionType { get => itemType; }
    public float FightsBeforeAppearance { get => fightsBeforeAppearance; }



}
