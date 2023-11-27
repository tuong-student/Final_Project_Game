using System;

using UnityEngine;

using UnityEditor.IMGUI.Controls;

[Serializable]
public class ScriptableObjectTreeViewState : TreeViewState {
    [SerializeField]
    private int m_SearchMode;
    public ScriptableObjectTreeViewState() : base() {
        m_SearchMode = 0;
    }
    public int searchMode { get { return m_SearchMode; } set { m_SearchMode = value; } }
}
