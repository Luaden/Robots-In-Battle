using UnityEditor;

[CustomEditor(typeof(SOCardDataObject))]
public class EditorCardGUI : Editor
{
    public CardType cardAttributesToDisplay;

    public override void OnInspectorGUI()
    {   
        SerializedProperty CardTypeProperty = serializedObject.FindProperty("cardType");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardType"));

        EditorGUILayout.Space();

        DisplayCardBaseAttributes();

        switch (CardTypeProperty.enumValueIndex)
        {
            case (int)CardType.Attack:
                DisplayAttackCardAttributes();
                break;

            case (int)CardType.Defense:
                DisplayDefenseCardAttributes();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayAttackCardAttributes()
    {
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("affectedChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("energyCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("baseDamage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("treatBaseDamageAsPercent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardEffects"));
    }

    private void DisplayDefenseCardAttributes()
    {
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("defenseType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("affectedChannels"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("energyCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("baseDamage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("treatDamageAsPercent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardEffects"));

    }

    private void DisplayCardBaseAttributes()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardImage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardBackground"));

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}