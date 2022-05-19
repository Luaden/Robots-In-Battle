using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(SOItemDataObject))]
public class EditorItemGUI : Editor
{
    public ItemType itemAttributesToDisplay;
    public CardType cardAttributesToDisplay;

    public override void OnInspectorGUI()
    {   
        SerializedProperty ItemTypeProperty = serializedObject.FindProperty("itemType");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemType"));

        EditorGUILayout.Space();

        DisplayItemBaseAttributes();

        switch (ItemTypeProperty.enumValueIndex)
        {
            case (int)ItemType.Card:
                DisplayCardAttributes();
                break;

            case (int)ItemType.Component:
                DisplayComponentAttributes();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayAttackCardAttributes()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardCategory"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("affectedChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("energyCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("baseDamage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("applyEffectsFirst"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardEffects"));
    }

    private void DisplayDefenseCardAttributes()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardCategory"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("affectedChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("energyCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("applyEffectsFirst"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardEffects"));
    }

    private void DisplayNeutralCardAttributes()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardCategory"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("affectedChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("energyCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("baseDamage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("applyEffectsFirst"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardEffects"));
    }

    private void DisplayItemBaseAttributes()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemShopImage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("currencyCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("timeCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("chanceToSpawn"));


        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayCardAttributes()
    {
        SerializedProperty CardTypeProperty = serializedObject.FindProperty("cardType");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardType"));

        switch (CardTypeProperty.enumValueIndex)
        {
            case (int)CardType.Attack:
                DisplayAttackCardAttributes();
                break;

            case (int)CardType.Defense:
                DisplayDefenseCardAttributes();
                break;

            case (int)CardType.Neutral:
                DisplayNeutralCardAttributes();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayComponentAttributes()
    {
        SerializedProperty ComponentTypeProperty = serializedObject.FindProperty("componentType");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("componentType"));

        switch (ComponentTypeProperty.enumValueIndex)
        {
            case (int)MechComponent.Head:
                DisplayImages(1);
                break;

            case (int)MechComponent.Arms:
                DisplayImages(2);
                break;

            case (int)MechComponent.Legs:
                DisplayImages(3);
                break;

            case (int)MechComponent.Back:
                DisplayImages(2);
                break;

            case (int)MechComponent.Torso:
                DisplayImages(1);
                break;
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("componentHP"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("componentEnergy"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("componentElement"));

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("cDMFromComponent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cDMToComponent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("extraElementStacks"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("energyGainModifier"));
    }

    private void DisplayImages(int numberToDisplay)
    {
        if(numberToDisplay == 1)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("primaryComponentSpriteID"));
        }
        if(numberToDisplay == 2)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("primaryComponentSpriteID"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("secondaryComponentSpriteID"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("altPrimaryComponentSpriteID"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("altSecondaryComponentSpriteID"));
        }
        if(numberToDisplay == 3)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("primaryComponentSpriteID"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("secondaryComponentSpriteID"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tertiaryComponentSpriteID"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("altPrimaryComponentSpriteID"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("altSecondaryComponentSpriteID"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("altTertiaryComponentSpriteID"));
        }
    }
}
#endif