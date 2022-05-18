using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(SOCompleteCharacter))]
public class EditorCompleteCharacterGUI : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pilotType"));
        SerializedProperty PilotTypeProperty = serializedObject.FindProperty("pilotType");
        EditorGUILayout.Space();

        switch (PilotTypeProperty.enumValueIndex)
        {
            case (int)PilotType.Unique:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fighterPilotUIObject"));
                DisplayBasePilotAttributes();
                break;

            case (int)PilotType.Generic:
                DisplayBasePilotAttributes();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayBasePilotAttributes()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pilotName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pilotPassiveEffects"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pilotActiveEffects"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogueModule"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("behaviorModule"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("deckList"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startingCurrency"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mechModule"));
    }
}
#endif
