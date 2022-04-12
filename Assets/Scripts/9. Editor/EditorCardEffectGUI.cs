using UnityEditor;

[CustomEditor(typeof(SOCardEffectObject))]
public class EditorCardEffectGUI : Editor
{
    public CardEffectTypes effectAttributesToDisplay;

    public override void OnInspectorGUI()
    {
        SerializedProperty EffectTypeProperty = serializedObject.FindProperty("cardEffect");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardEffect"));

        EditorGUILayout.Space();

        switch (EffectTypeProperty.enumValueIndex)
        {
            case (int)CardEffectTypes.AdditionalElementStacks:
                DisplayMagnitudeAttribute();
                break;
            case (int)CardEffectTypes.GainShields:
                DisplayMagnitudeAttribute();
                break;
            case (int)CardEffectTypes.MultiplyShield:
                DisplayMagnitudeAttribute();
                break;
            case (int)CardEffectTypes.IncreaseOutgoingCardTypeDamage:
                DisplayCardTypeAttribute();
                DisplayMagnitudeAttribute();
                DisplayDurationAttribute();
                break;
            case (int)CardEffectTypes.IncreaseOutgoingChannelDamage:
                DisplayMagnitudeAttribute();
                DisplayDurationAttribute();
                break;
            case (int)CardEffectTypes.KeyWordInitialize:
                DisplayKeyWordAttribute();
                DisplayMagnitudeAttribute();
                break;
            case (int)CardEffectTypes.KeyWordExecute:
                DisplayKeyWordAttribute();
                break;
            case (int)CardEffectTypes.PlayMultipleTimes:
                DisplayMagnitudeAttribute();
                break;
            case (int)CardEffectTypes.ReduceIncomingChannelDamage:
                DisplayMagnitudeAttribute();
                DisplayDurationAttribute();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayCardTypeAttribute()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardTypeToBoost"));
    }

    private void DisplayDurationAttribute()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectDuration"));
    }

    private void DisplayMagnitudeAttribute()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectMagnitude"));
    }

    private void DisplayKeyWordAttribute()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardKeyWord"));
    }
}