using UnityEditor;
#if UNITY_EDITOR
[CustomEditor(typeof(SOCardEffectObject))]
public class EditorCardEffectGUI : Editor
{
    public CardEffectTypes effectAttributesToDisplay;

    public override void OnInspectorGUI()
    {
        SerializedProperty EffectTypeProperty = serializedObject.FindProperty("effectType");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectType"));

        EditorGUILayout.Space();

        switch (EffectTypeProperty.enumValueIndex)
        {
            case (int)CardEffectTypes.AdditionalElementStacks:
                DisplayMagnitudeAttribute();
                break;
            case (int)CardEffectTypes.GainShields:
                DisplayMagnitudeAttribute();
                break;
            case (int)CardEffectTypes.GainShieldWithFalloff:
                DisplayMagnitudeAttribute();
                DisplayFalloff();
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
            case (int)CardEffectTypes.ReduceOutgoingChannelDamage:
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

    private void DisplayFalloff()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fallOffPerTurn"));
    }
}
#endif