using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScriptableObjectWindow_Example_MonoBehaviour : MonoBehaviour {
    public ScriptableObjectWindow_Example_ScriptableObject[] exampleScriptableObjects;
}

#if UNITY_EDITOR
[CustomEditor(typeof(ScriptableObjectWindow_Example_MonoBehaviour))]
public class ScriptableObjectWindow_Example_MonoBehaviour_Editor : Editor {
    public override void OnInspectorGUI() {
        GUIStyle labelStyle = "Label";
        labelStyle.richText = true;
        EditorGUILayout.LabelField(@"
<size=18>Background:</size>

<size=14>Any <i>ScriptableObject</i> that is referenced from anywhere in a scene
(<i>MonoBehaviours</i>, <i>Prefabs</i> etc.) will be saved in that scene unless
the object is an asset.

<b>This is standard Unity behaviour</b>.

<size=18>Purpose:</size>

The <b><i>ScriptableObject Window</i></b> aids the creation
and assignment of any type of <i>ScriptableObject</i>.

Open the window by clicking on ""<b>Window -> Scriptable Objects</b>"".

Create an '<i>Example ScriptableObject</i>' and
drag it into the example list below (this <i>MonoBehaviour</i>).

Go back to the <b><i>ScriptableObject Window</i></b> and watch
how the object is now listed within the section representing
the example scene.

Optionally, drag it into the assets folder to create an asset.

<size=18>Note:</size>

Only <i>ScriptableObjects</i> that are linked from any of the currently open
scenes (and their parent folders) will show in the window.</size>", labelStyle, GUILayout.Height(EditorGUIUtility.singleLineHeight * 32));
        DrawDefaultInspector();
    }
}
#endif
