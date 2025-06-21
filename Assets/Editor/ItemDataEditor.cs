using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FishData))]
public class ItemDataEditor : Editor
{
    private FishData _itemData;

    private void OnEnable() => _itemData = (FishData)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Visual Shape Editor", EditorStyles.boldLabel);


        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        for (int y = 0; y < _itemData.Height; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < _itemData.Width; x++)
            {
                if (y >= _itemData.Shape.Count || x >= _itemData.Shape[y].Cols.Count) 
                    continue;

                if (!_itemData.HasCustomShape)
                {
                    GUI.backgroundColor = _itemData.Shape[y].Cols[x] ? new Color(0.4f, 1f, 0.6f, 0.6f) : Color.white;    
                   
                    if (!GUILayout.Button("", GUILayout.Width(30), GUILayout.Height(30))) 
                        continue;
                }
                else
                {
                    GUI.backgroundColor = _itemData.Shape[y].Cols[x] ? new Color(0.4f, 1f, 0.6f, 1f) : Color.white;
                    
                    Undo.RecordObject(_itemData, "Toggle Shape Cell");
                    
                    if (!GUILayout.Button("", GUILayout.Width(30), GUILayout.Height(30))) 
                        continue;
                    
                    _itemData.Shape[y].Cols[x] = !_itemData.Shape[y].Cols[x];
                }
                
                
                EditorUtility.SetDirty(_itemData);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = Color.white;
    }
}