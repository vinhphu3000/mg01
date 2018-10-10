using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    // TODO REVIEW
    // Have material live under text
    // move stencil mask into effects *make an efects top level element like there is
    // paragraph and character

    /// <summary>
    /// Editor class used to edit UI Labels.
    /// </summary>

    [CustomEditor(typeof(Text), true)]
    [CanEditMultipleObjects]
    public class TextEditor : GraphicEditor
    {
        SerializedProperty m_Text;
        SerializedProperty m_FontData;
        SerializedProperty m_Grey;
        SerializedProperty m_LangId;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Text = serializedObject.FindProperty("m_Text");
            m_FontData = serializedObject.FindProperty("m_FontData");
            m_Grey = serializedObject.FindProperty("m_Grey");
            m_LangId = serializedObject.FindProperty("langId");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if(m_LangId != null) EditorGUILayout.PropertyField(m_LangId);
            EditorGUILayout.PropertyField(m_Text);
            EditorGUILayout.PropertyField(m_FontData);
            if(m_Grey != null) EditorGUILayout.PropertyField(m_Grey);
            AppearanceControlsGUI();
            RaycastControlsGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
