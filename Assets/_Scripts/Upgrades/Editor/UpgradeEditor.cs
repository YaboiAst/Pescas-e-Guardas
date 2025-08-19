using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Upgrade))]
public class UpgradeEditor : Editor
{
    private SerializedProperty _nameProp;
    private SerializedProperty _descProp;
    private SerializedProperty _costProp;
    private SerializedProperty _effectsProp;

    private Type[] _effectTypes;
    private GUIStyle _headerStyle;

    private void OnEnable()
    {
        _nameProp = serializedObject.FindProperty("Name");
        _descProp = serializedObject.FindProperty("Description");
        _costProp = serializedObject.FindProperty("Cost");
        _effectsProp = serializedObject.FindProperty("Effects");

        _effectTypes = TypeCache.GetTypesDerivedFrom<Effect>()
            .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
            .OrderBy(t => t.Name)
            .ToArray();

        _headerStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_nameProp);
        EditorGUILayout.PropertyField(_descProp);
        EditorGUILayout.PropertyField(_costProp);

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Effects", _headerStyle);
        DrawEffectsList();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawEffectsList()
    {
        using (new EditorGUILayout.VerticalScope("box"))
        {
            // List items
            for (int i = 0; i < _effectsProp.arraySize; i++)
            {
                var element = _effectsProp.GetArrayElementAtIndex(i);
                var typeName = GetNiceTypeName(element.managedReferenceFullTypename);

                using (new EditorGUILayout.VerticalScope("box"))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField($"{i + 1}. {typeName}", EditorStyles.boldLabel);
                        if (GUILayout.Button("Remove", GUILayout.Width(70)))
                        {
                            _effectsProp.DeleteArrayElementAtIndex(i);
                            break;
                        }
                    }

                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(element, includeChildren: true);
                    EditorGUI.indentLevel--;
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Effect", GUILayout.Width(100)))
                    ShowAddMenu();
            }
        }
    }

    private void ShowAddMenu()
    {
        var menu = new GenericMenu();
        if (_effectTypes.Length == 0)
        {
            menu.AddDisabledItem(new GUIContent("No Effect types found"));
        }
        else
        {
            foreach (var t in _effectTypes)
            {
                var label = new GUIContent(t.Name);
                menu.AddItem(label, false, () => AddEffectOfType(t));
            }
        }
        menu.ShowAsContext();
    }

    private void AddEffectOfType(Type type)
    {
        serializedObject.Update();
        _effectsProp.arraySize++;
        var element = _effectsProp.GetArrayElementAtIndex(_effectsProp.arraySize - 1);
        element.managedReferenceValue = Activator.CreateInstance(type);
        serializedObject.ApplyModifiedProperties();
    }

    private static string GetNiceTypeName(string full)
    {
        if (string.IsNullOrEmpty(full)) return "Null";
        var idx = full.LastIndexOf(' ');
        var typeName = idx >= 0 ? full[(idx + 1)..] : full;
        var shortName = typeName.Split('.').Last();
        return shortName;
    }
}