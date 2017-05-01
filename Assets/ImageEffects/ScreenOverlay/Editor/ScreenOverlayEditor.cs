using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ScreenOverlay))]
public class ScreenOverlayEditor : Editor {
    SerializedProperty _color;

    void OnEnable() {
        _color = serializedObject.FindProperty("_color");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(_color);

        serializedObject.ApplyModifiedProperties();
    }
}