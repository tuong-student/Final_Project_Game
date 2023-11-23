using System.Reflection;

using UnityEditor;

public class GUIViewReflection {
    private static MethodInfo currentMethodInfo;
    private static MethodInfo hasFocusMethodInfo;
    private static FieldInfo  m_ParentFieldInfo;

    public static GUIViewReflection current {
        get { return new GUIViewReflection(GUIViewReflection.currentMethodInfo.Invoke(null, new object[] {})); }
    }

    private object sourceObject;

    public GUIViewReflection(object sourceObject) {
        this.sourceObject = sourceObject;
    }

    public GUIViewReflection(EditorWindow sourceObject) {
        this.sourceObject = GUIViewReflection.m_ParentFieldInfo.GetValue(sourceObject);
    }

    public bool hasFocus { get { return (bool) hasFocusMethodInfo.Invoke(sourceObject, new object[] {}); } }
    public bool isNull   { get { return sourceObject == null; } }

    static GUIViewReflection() {
        var editorWindowType = typeof(EditorWindow);
        var assembly         = editorWindowType.Assembly;
        var guiViewType      = assembly.GetType("UnityEditor.GUIView");

        GUIViewReflection.currentMethodInfo  = guiViewType.GetProperty("current",    BindingFlags.Public    | BindingFlags.Static  ).GetGetMethod();
        GUIViewReflection.hasFocusMethodInfo = guiViewType.GetProperty("hasFocus",   BindingFlags.Public    | BindingFlags.Instance).GetGetMethod();
        GUIViewReflection.m_ParentFieldInfo  = editorWindowType.GetField("m_Parent", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}

public static class EditorWindowExtension {
    public static GUIViewReflection GetParentReflection(this EditorWindow window) {
        return new GUIViewReflection(window);
    }
}
