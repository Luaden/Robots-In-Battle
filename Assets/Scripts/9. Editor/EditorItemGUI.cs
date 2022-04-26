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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("componentDamageMultiplier"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardEffects"));
    }

    private void DisplayDefenseCardAttributes()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardCategory"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("affectedChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("energyCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardEffects"));
    }

    private void DisplayNeutralCardAttributes()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardBackground"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardCategory"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("affectedChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("energyCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("baseDamage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("componentDamageMultiplier"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardEffects"));
    }

    private void DisplayItemBaseAttributes()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("timeCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("currencyCost"));
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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("componentImage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("componentType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("componentHP"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("componentEnergy"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("componentElement"));

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("bonusDamageFromComponent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bonusDamageAsPercent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("reduceDamageToComponent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("reduceDamageAsPercent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("extraElementStacks"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("energyGainModifier"));
    }
}
#endif