using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HUD Element Popup", menuName = "Popup Descriptions/Buff HUD Element")]
public class SOHUDBuffDescription : ScriptableObject
{
    [SerializeField] private HUDBuffElement elementType;
    [SerializeField] private string nameText;
    [TextArea(1, 5)]
    [SerializeField] private string descriptionText;

    public HUDBuffElement ElementType { get => elementType; }
    public string NameText { get => nameText; }
    public string DescriptionText { get => descriptionText; }
}
