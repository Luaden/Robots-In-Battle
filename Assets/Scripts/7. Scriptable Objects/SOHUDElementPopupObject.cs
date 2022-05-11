using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HUD Element Popup", menuName = "Popup Descriptions/General HUD Element")]
public class SOHUDElementPopupObject : ScriptableObject
{
    [SerializeField] private GeneralHUDElement elementType;
    [SerializeField] private string nameText;
    [TextArea(1, 5)]
    [SerializeField] private string descriptionText;

    public GeneralHUDElement ElementType { get => elementType; }
    public string NameText { get => nameText; }
    public string DescriptionText { get => descriptionText; }
}
