using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace PetrushevskiApps.UIManager
{
    [CustomEditor(typeof(UIButton), true)]
    [CanEditMultipleObjects]
    public class UIButtonEditor : ButtonEditor
    {
        private GameObject gameObject;

        private List<Type> extensions = new List<Type>();
        private List<string> extensionNames = new List<string>();

        protected override void OnEnable()
        {
            base.OnEnable();
            UIButton buttonObject = (UIButton) target;
            this.gameObject = buttonObject.gameObject;
            SetExtensions();
        }

        private void SetExtensions()
        {
            Type[] types = AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ButtonExtension));
            
            if (types.Length != extensions.Count)
            {
                extensions.Clear();
                extensionNames.Clear();
                
                extensions = types.ToList();
                extensionNames.Add("select");
                extensions.ForEach(extension => extensionNames.Add(extension.Name));
            }
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            DrawExtensionsWidgetInInspector();
            serializedObject.ApplyModifiedProperties();
        }

        
        private void DrawExtensionsWidgetInInspector()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Extensions", EditorStyles.boldLabel);
            Rect rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
            rect = EditorGUI.PrefixLabel(rect, new GUIContent("Add Extension"));

            int selection = EditorGUI.Popup(rect, 0, extensionNames.ToArray());
            
            if (selection > 0)
            {
                Type extType =  extensions[selection - 1];
                if (gameObject.GetComponent(extType) == null)
                    Undo.AddComponent(gameObject, extType);
            }
        }
    }
}
