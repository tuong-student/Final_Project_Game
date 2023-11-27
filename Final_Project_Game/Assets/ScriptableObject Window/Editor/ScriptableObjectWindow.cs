using Type = System.Type;
using Assembly = System.Reflection.Assembly;

using UObject = UnityEngine.Object;

using System.Collections.Generic;
using System.Reflection;
using System.Text;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

public class ScriptableObjectWindow : EditorWindow {

    [SerializeField]
    ScriptableObjectTreeViewState m_TreeViewState;
    ScriptableObjectTreeView m_TreeView;

    private GenericMenu newObjectMenu;
    private GUIViewReflection guiView;

    [MenuItem("Window/Scriptable Objects")]
    public static void Init() {
        var w = EditorWindow.GetWindow<ScriptableObjectWindow>(string.Empty, true);
        w.titleContent = new GUIContent("Context", AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ScriptableObject Window/Icons/TitleBar.png"));
    }

    private void OnEnable() {
        if (m_TreeViewState == null)
            m_TreeViewState = new ScriptableObjectTreeViewState();
        m_TreeView = new ScriptableObjectTreeView(m_TreeViewState);

        Undo.undoRedoPerformed             += m_TreeView.Reload; // Hardly needed since window is being updated at 10 fps
        EditorApplication.projectChanged   += m_TreeView.Reload;

        CreateNewObjectMenu();

        guiView = this.GetParentReflection();
    }

    private void OnDisable() {
        if (m_TreeView != null) {
            Undo.undoRedoPerformed             -= m_TreeView.Reload; // Hardly needed since window is being updated at 10 fps
            EditorApplication.projectChanged   -= m_TreeView.Reload;
        }
    }

    private class ObjectCreationData {
        public string fileName;
        public Type type;
        public ObjectCreationData(string fileName, Type type) { this.fileName = fileName; this.type = type; }
    }

    private class CreateAssetMenuEntry : ObjectCreationData {
        public string name;
        public int order;
        public CreateAssetMenuEntry(string name, string fileName, Type type, int order) : base(fileName, type) { this.name = name; this.order = order; }
    }

    private void CreateNewObjectMenu() {
        newObjectMenu = new GenericMenu();

        var menuEntries = new List<CreateAssetMenuEntry>();

        Assembly[] referencedAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i< referencedAssemblies.Length; i++) {
            foreach (Type t in referencedAssemblies[i].GetTypes()) {
                foreach (var oTemp in t.GetCustomAttributes(typeof(CreateAssetMenuAttribute), true)) {
                    var attribute = (CreateAssetMenuAttribute) oTemp;
                    menuEntries.Add(new CreateAssetMenuEntry(attribute.menuName, attribute.fileName, t, attribute.order));
                }
            }
        }

        menuEntries.Sort((left, right) => left.order - right.order);
        foreach (var entry in menuEntries) {
            newObjectMenu.AddItem(new GUIContent(string.IsNullOrEmpty(entry.name) ? entry.type.Name : entry.name), false, CreateNewObjectHandler, entry);
        }
    }

    private void CreateNewObjectHandler(object data) {
        var creationData = (ObjectCreationData) data;
        var fn = creationData.fileName;
        m_TreeView.CreateScriptableObject(creationData.type, string.IsNullOrEmpty(fn) ? null : fn.EndsWith(".asset") ? fn.Substring(0, fn.Length - 6) : fn);
    }

    private void OnInspectorUpdate() {
        // m_TreeView.CheckDragAndDrop();
        m_TreeView.Reload();
        Repaint();
    }

    private string searchFilter;

    private float toolbarHeight = 17f;

    private void OnGUI() {
        if (guiView.isNull)
            guiView = GUIViewReflection.current;

        GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);

        var dropDownStyle = GUI.skin.FindStyle("toolbarDropDown");
        var createContent = new GUIContent("Create");
        Rect rect = GUILayoutUtility.GetRect(createContent, dropDownStyle, null);
        if (Event.current.type == EventType.Repaint)
            dropDownStyle.Draw(rect, createContent, false, false, false, false);
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition)) {
            newObjectMenu.ShowAsContext();
        }

        GUILayout.FlexibleSpace();

        var searchWidth = EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth + 5f;
        var searchStyle = GUI.skin.FindStyle("ToolbarSeachTextField");

        Rect searchFieldRect = GUILayoutUtility.GetRect(searchWidth * 0.2f, searchWidth * 1.5f, 16f, 16f, searchStyle);

        GUIContent[] names = { new GUIContent("All"), new GUIContent("Name"), new GUIContent("Type") };
        var searchFieldControlId = GUIUtility.GetControlID(FocusType.Keyboard, searchFieldRect);
        EditorGUI.BeginChangeCheck();
        searchFilter = ToolbarSearchField(searchFieldControlId, searchFieldRect, names, searchFilter);
        if (EditorGUI.EndChangeCheck())
            m_TreeView.searchString = searchFilter;
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape && searchFilter != "" && GUIUtility.hotControl == 0)
            Event.current.Use();

        if (GUI.GetNameOfFocusedControl() != "SearchFilter") {
            if (Event.current.type == EventType.KeyDown) {
                if (Event.current.keyCode == KeyCode.Delete) {
                    m_TreeView.DeleteSelection();
                    Event.current.Use();
                }
            } else if (Event.current.commandName == "Duplicate") {
                if (Event.current.type == EventType.ExecuteCommand) {
                    m_TreeView.DuplicateSelection();
                    Event.current.Use();
                } else if (Event.current.type == EventType.ValidateCommand) {
                    Event.current.Use();
                }
            }
        }

        GUILayout.EndHorizontal();

        m_TreeView.OnGUI(new Rect(0, toolbarHeight, position.width, position.height - toolbarHeight));
    }


    private string ToolbarSearchField(int id, Rect position, GUIContent[] searchModes, string text) {
        Rect rect = position;
        rect.width = 20f;
        var searchMode = m_TreeViewState.searchMode;
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            EditorUtility.DisplayCustomMenu(position, searchModes, searchMode, SelectSearchType, null);
        text = ToolbarSearchField(id, position, text);
        if (text == "" && (GUIUtility.keyboardControl != id || !EditorGUIUtility.editingTextField || !guiView.hasFocus) && Event.current.type == EventType.Repaint) {
            position.width -= 14f;
            using (new EditorGUI.DisabledScope(true)) {
                var tp = GUI.skin.FindStyle("ToolbarSeachTextFieldPopup");
                tp.Draw(position, searchModes[searchMode], id, false);
            }
        }
        return text;
    }

    private string ToolbarSearchField(int id, Rect position, string text) {
        Rect position2 = position;
        position2.width -= 14f;
        text = DoTextField(id, position2, text, GUI.skin.FindStyle("ToolbarSeachTextFieldPopup"));
        Rect position3 = position;
        position3.x += position.width - 14f;
        position3.width = 14f;
        var buttonStyle = (!(text != "")) ? GUI.skin.FindStyle("ToolbarSeachCancelButtonEmpty") : GUI.skin.FindStyle("ToolbarSeachCancelButton");
        if (GUI.Button(position3, GUIContent.none, buttonStyle) && text != "") {
            text = "";
            GUIUtility.keyboardControl = 0;
            m_TreeView.ExpandToRevealSelection();
        }
        return text;
    }

    private string DoTextField(int id, Rect position, string text, GUIStyle style) {
        FieldInfo recycledTextEditorField = typeof(EditorGUI).GetField("s_RecycledEditor", BindingFlags.NonPublic | BindingFlags.Static);
        object editor = recycledTextEditorField.GetValue(null);

#if UNITY_2021_1_OR_NEWER
        Type[] DoTextField_types = new[] { recycledTextEditorField.FieldType, typeof(int), typeof(Rect), typeof(string), typeof(GUIStyle), typeof(string), /*out*/ typeof(bool).MakeByRefType(), typeof(bool), typeof(bool), typeof(bool) };
        MethodInfo dynMethod = typeof(EditorGUI).GetMethod("DoTextField", BindingFlags.NonPublic | BindingFlags.Static, null, DoTextField_types, null);
#else
        MethodInfo dynMethod = typeof(EditorGUI).GetMethod("DoTextField", BindingFlags.NonPublic | BindingFlags.Static);
#endif

        object[] arguments = new object[] { editor, id, position, text, style, null, false, false, false, false };
        return (string) dynMethod.Invoke(this, arguments );
    }

    private void SelectSearchType(object userData, string[] options, int selected) {
        if (selected != m_TreeViewState.searchMode) {
            m_TreeView.SetSearchMode((m_TreeViewState.searchMode = selected));
        }
    }
}
