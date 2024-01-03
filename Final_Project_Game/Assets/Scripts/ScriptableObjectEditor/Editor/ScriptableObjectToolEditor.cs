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
    private Dictionary<FoldHeader, List<ScriptableObjectData>> _filterDic = new Dictionary<FoldHeader, List<ScriptableObjectData>>();
    private List<ScriptableObjectData> _allObjects = new List<ScriptableObjectData>();
    private EditorWindow _window;
    private List<ScriptableObjectData> _scriptableObjectDatas = new List<ScriptableObjectData>();
    private List<FoldHeader> _loadedFoldHeaderData = new List<FoldHeader>();
    private Vector2 _scrollViewPosition;
    private bool _isGettingData;
    private string _searchString = "";
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

        // update draw zone
        DrawSearchZone();
        DrawFoldGroup();
        UpdateDragAndDrop();
        SaveState();
    }
    private void Update()
    {
        // update data zone
        FilterData();
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
        Debug.Log(_allObjects.Count);
        SplitData(_allObjects);
        _isGettingData = false;
    }
    private void SplitData(List<ScriptableObjectData> scriptableObjectDatas)
    {
        // Split into Type and list
        Dictionary<Type, List<ScriptableObjectData>> tempDic = new Dictionary<Type, List<ScriptableObjectData>>();
        int index = 0;
        while(index < scriptableObjectDatas.Count)
        {
            ScriptableObjectData data = scriptableObjectDatas[index];

            // Add to dictionary if can
            if(tempDic.TryGetValue(data.Type, out List<ScriptableObjectData> list))
            {
                list.Add(data);   
            }            
            else
            {
                var l = new List<ScriptableObjectData>
                {
                    data
                };
                tempDic.Add(data.Type, l);
            }
            index++;
        }

        // convert to FoldHeader and list 
        _scriptableObjectDic.Clear();
        _scriptableObjectDic = tempDic.ToDictionary(pair => new FoldHeader(pair.Key, false), pair => pair.Value);
        _scriptableObjectDic = _scriptableObjectDic.OrderBy(pair => pair.Key.Name).ToDictionary(pair => pair.Key, pair => pair.Value);
        ApplyLoadedData();
    }
    private void FilterData()
    {
        if (_searchString != "")
        {
            List<ScriptableObjectData> filterList = _allObjects.Where(x => x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            SplitData(filterList);
        }
        else
            SplitData(_allObjects);
    }
    #endregion

    #region Draw functions
    private void DrawSearchZone()
    {
        GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
        {
            _searchString = GUILayout.TextField(_searchString, GUI.skin.FindStyle("ToolbarSearchTextField"));
            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSearchCancelButton")))
            {
                // Remove focus if cleared
                _searchString = "";
                GUI.FocusControl(null);
            }
        }
        GUILayout.EndHorizontal();
    }
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
                    ChangeLoadedData(pairCopy.Key);
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
        _loadedFoldHeaderData = JsonConvert.DeserializeObject<List<FoldHeader>>(json);

        // Apply data
        ApplyLoadedData();
    }
    private void ApplyLoadedData()
    {
        foreach(var pair in _scriptableObjectDic)
        {
            if(_loadedFoldHeaderData.Exists(x => x.Name == pair.Key.Name))
            {
                FoldHeader foldHeader = _loadedFoldHeaderData.First(x => x.Name == pair.Key.Name);
                if(foldHeader != null)
                {
                    pair.Key.IsFold = foldHeader.IsFold;
                }
            }
        }
    }
    private void ChangeLoadedData(FoldHeader changedHeader)
    {
        if(_loadedFoldHeaderData.Any(x => x.Compare(changedHeader)))
        {
            _loadedFoldHeaderData.First(x => x.Compare(changedHeader)).IsFold = changedHeader.IsFold;
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