using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using System.IO;
using System;

public class ScriptableObjectToolEditor : EditorWindow
{
    #region public
    #endregion

    #region private variable
    private Dictionary<FoldHeader, List<ScriptableObjectData>> _scriptableObjectDic = new Dictionary<FoldHeader, List<ScriptableObjectData>>();
    private List<ScriptableObjectData> _allObjects = new List<ScriptableObjectData>();
    private EditorWindow _window;
    private List<ScriptableObjectData> _scriptableObjectDatas = new List<ScriptableObjectData>();
    private Vector2 _scrollViewPosition;
    private bool _isGettingData;
    #endregion

    #region Show
    [MenuItem("Tools/ScriptableObjectEditor")]
    static void ShowEditorWindow()
    {
        ScriptableObjectToolEditor editor = EditorWindow.GetWindow<ScriptableObjectToolEditor>();
        editor.Show();
        editor.titleContent = new GUIContent("ScriptableObject Editor Window");
    }
    private void Awake()
    {
        _window = EditorWindow.GetWindow<ScriptableObjectToolEditor>();
    }
    private void OnEnable()
    {
        GetData();
        LoadState();
        SOToolHelper.OnAssetDatabaseChange += GetData;
        SOToolHelper.OnAssetDatabaseChange += LoadState;
    }
    private void OnDisable() 
    {
        SOToolHelper.OnAssetDatabaseChange -= GetData;
        SOToolHelper.OnAssetDatabaseChange -= LoadState;
    }
    private void OnGUI()
    {
        if (_isGettingData) return;
        DrawFoldGroup();
        UpdateDragAndDrop();
        SaveState();
    }
    #endregion

    #region data functions
    private void GetData()
    {
        _isGettingData = true;
        _allObjects.Clear();
        // Load all ScriptableObject
        var files = AssetDatabase.FindAssets("t:ScriptableObject", new[] {"Assets"});
        foreach(var file in files)
        {
            ScriptableObjectData data = new ScriptableObjectData(file);
            _scriptableObjectDatas.Add(data);
            _allObjects.Add(data);
        }

        // Split into types of ScriptableObject
        SplitData();
    }

    private void SplitData()
    {
        _scriptableObjectDic.Clear();
        while(_allObjects.Count > 0)
        {
            ScriptableObjectData firstObject = _allObjects[0];
            List<ScriptableObjectData> datas = _allObjects.Where(x => x.Type == firstObject.Type).ToList();
            foreach(var data in datas)
            {
                _allObjects.Remove(data);
            }
            _scriptableObjectDic.Add(new FoldHeader(firstObject.Type, false), datas);
        }
        _scriptableObjectDic = _scriptableObjectDic.OrderBy(pair => pair.Key.Name).ToDictionary(pair => pair.Key, pair => pair.Value);
        _isGettingData = false;
    }
    #endregion

    #region Draw functions
    private void DrawFoldGroup()
    {
        _scrollViewPosition = GUILayout.BeginScrollView(_scrollViewPosition);
        {
            for (int i = 0; i < _scriptableObjectDic.Count; i++)
            {
                var pairCopy = _scriptableObjectDic.ElementAt(i);
                GUILayout.BeginHorizontal();
                {
                    pairCopy.Key.IsFold = EditorGUILayout.Foldout(pairCopy.Key.IsFold, pairCopy.Key.Name, true);

                    DrawAddButton(pairCopy.Key.Type, "Create new Scriptable with type in a new folder");
                }
                GUILayout.EndHorizontal();
                if(pairCopy.Key.IsFold)
                {
                    foreach(var data in pairCopy.Value)
                    {
                        ScriptableOBjectPanel panel = new ScriptableOBjectPanel(data, _window);
                        GUILayout.Space(SOToolHelper.RowGap);
                        panel.DrawRow();
                    }
                }
            }
        }
        GUILayout.EndScrollView();
    }
    private void DrawAddButton(Type type, string tooltip = "")
    {
        GUIContent addSign = EditorGUIUtility.IconContent("d_ol_plus");
        addSign.tooltip = tooltip;
        if(GUILayout.Button(addSign, BasicEditorStyles.AlignLabelMiddleRight))
        {
            string rawFolderPath = SOToolHelper.ChooseFolder();
            string folderPath = rawFolderPath.Substring(rawFolderPath.IndexOf("Assets") + 0); // 0 is include A in Assets
            string fileName = EditorInputDialogue.Show("Create new asset", "Enter ScriptableObject name", "");
            if(folderPath != null && folderPath != "" && folderPath != " ")
            {
                if(fileName != null && fileName != "" && fileName != " ")
                {
                    SOToolHelper.CreateAssetAtPath(type, folderPath, fileName);
                }
            }
        }
    }
    #endregion

    #region DragAndDrop
    public void UpdateDragAndDrop()
    {
        if(Event.current.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            Event.current.Use();
        }
    }
    #endregion

    #region Save Load
    private string GetFilePath()
    {
        MonoScript ms = MonoScript.FromScriptableObject( this );
        string thisFilePath = AssetDatabase.GetAssetPath( ms );

        FileInfo fi = new FileInfo( thisFilePath);
        string thisFileFolder = fi.Directory.ToString();
        thisFileFolder.Replace( '\\', '/');

        // Debug.Log( thisFileFolder );

        string filePath = Path.Combine(thisFileFolder, "data.txt");
        return filePath;
    }
    private void SaveState()
    {
        List<FoldHeader> foldHeaders = _scriptableObjectDic.Keys.ToList();
        string json = JsonConvert.SerializeObject(foldHeaders);

        string filePath = GetFilePath();
        if(!File.Exists(filePath))
        {
            File.CreateText(filePath);
        }
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.Write(json);
        }
    }
    private void LoadState()
    {
        // Load data
        string filePath = GetFilePath();
        string json;
        using(StreamReader reader = new StreamReader(filePath))
        {
            json = reader.ReadToEnd();
        }
        List<FoldHeader> foldHeaders = JsonConvert.DeserializeObject<List<FoldHeader>>(json);

        // Apply data
        foreach(var pair in _scriptableObjectDic)
        {
            if(foldHeaders.Exists(x => x.Name == pair.Key.Name))
            {
                FoldHeader foldHeader = foldHeaders.First(x => x.Name == pair.Key.Name);
                if(foldHeader != null)
                {
                    pair.Key.IsFold = foldHeader.IsFold;
                }
            }
        }
    }
    #endregion
}

public class ScriptableOBjectPanel
{
    private ScriptableObjectData _data;
    public bool IsData => _data != null;
    private EditorWindow _window;

    public ScriptableOBjectPanel(ScriptableObjectData data, EditorWindow editorWindow)
    {
        _data = data;
        _window = editorWindow;
    }

    public void DrawRow()
    {
        Rect backgroundRect;
        GUILayout.BeginHorizontal();
        {
            // Draw BG
            GUILayout.Space(30);
            var space = GUILayoutUtility.GetLastRect();
            backgroundRect = new Rect(space.position.x, space.position.y, _window.position.width, SOToolHelper.RowHeight);
            if (ColorUtility.TryParseHtmlString("#303030", out Color color))
                EditorGUI.DrawRect(backgroundRect, color);
            else
                EditorGUI.DrawRect(backgroundRect, Color.black);
            backgroundRect.width -= 30;
            if(backgroundRect.Contains(Event.current.mousePosition))
            {
                if(Event.current.type == EventType.MouseDown)
                {
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new[] { _data.ScriptableObject };
                    DragAndDrop.StartDrag("Start drag");
                }
            }

            // Draw content
            Texture2D icon = AssetPreview.GetMiniThumbnail(_data.ScriptableObject);
            GUILayout.Label(icon,BasicEditorStyles.AlignLabelMiddleLeft , GUILayout.Width(SOToolHelper.IconWidth), GUILayout.Height(SOToolHelper.IconHeight)); // Draw icon
            DrawNameLabels();
            DrawAddButton("Create a new ScriptableObject in the same folder");
            DrawDeleteButton();
        }
        GUILayout.EndHorizontal();
    }
    private void DrawNameLabels()
    {
        GUILayout.BeginVertical();
        {
            // Draw name labels
            GUILayout.Space(5);
            GUIStyle labelNameStyle;
            if(DragAndDrop.objectReferences.Length > 0 && DragAndDrop.objectReferences[0] == _data.ScriptableObject)
            {
                // Current object is dragging
                labelNameStyle = SOToolHelper.ChosenNameLabelStyle;
            }
            else
            {
                labelNameStyle = SOToolHelper.NormalNameLabelStyle;
            }
            GUILayout.Label(_data.Name, labelNameStyle);
            {
                // Assign press event for name label 
                var labelName = GUILayoutUtility.GetLastRect();
                if (labelName.Contains(Event.current.mousePosition))
                {
                    if (Event.current.type == EventType.MouseDown)
                    {
                        AssetDatabase.OpenAsset(_data.ScriptableObject);
                        EditorGUIUtility.PingObject(_data.ScriptableObject);
                    }
                }
            }

            GUILayout.Label(_data.Path, SOToolHelper.PathTextLabelStyle);
            GUILayout.Space(5);
        }
        GUILayout.EndVertical();
    }
    private void DrawAddButton(string tooltip = "")
    {
        GUIContent addSign = EditorGUIUtility.IconContent("d_ol_plus_act");
        addSign.tooltip = tooltip;
        if(GUILayout.Button(addSign, SOToolHelper.AddButtonStyle, GUILayout.Height(SOToolHelper.RowHeight)))
        {
            string fileName = EditorInputDialogue.Show("Create new asset", "Enter ScriptableObject name", "");
            if(fileName != null && fileName != "" && fileName != " ")
            {
                SOToolHelper.CreateAssetAtPath(_data.Type, _data.FolderPath, fileName);
            }
        }
    }
    private void DrawDeleteButton()
    {
        GUIContent minusSign = EditorGUIUtility.IconContent("d_ol_minus");
        minusSign.tooltip = "Delete this ScriptableObject";
        if(GUILayout.Button(minusSign, SOToolHelper.AddButtonStyle, GUILayout.Height(SOToolHelper.RowHeight)))
        {
            SOToolHelper.DeleteAsset(_data.Path);
        }
    }
}