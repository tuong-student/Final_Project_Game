using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;
using UnityEditor.IMGUI.Controls;

using UObject = UnityEngine.Object;

public class ScriptableObjectTreeViewItem : TreeViewItem {
    public ScriptableObject scriptableObject { get; private set; }
    public UObject               assetObject { get; private set; }

    private Scene scene;
    private bool representsObject;

    public ItemType type;
    public bool canBeParent;

    public enum ItemType {
        ScriptableObject,
        NameGroup,
        FolderGroup,
        Scene,
        TopLevel
    }

    public ScriptableObjectTreeViewItem(Scene scene) : base() {
        base.icon = (Texture2D) EditorGUIUtility.IconContent("SceneAsset Icon").image;
        base.depth = 0;
        base.displayName = scene.name;
        base.id = 0;
        this.scriptableObject = null;
        this.assetObject = null;
        this.scene = scene;
        this.type = ItemType.Scene;
        this.canBeParent = true;
    }

    public ScriptableObjectTreeViewItem(string groupName, Texture2D icon, bool canBeParent, UObject assetObject, int depth = 0) : base() {
        base.icon = icon;
        base.depth = depth;
        base.displayName = groupName;
        base.id = 0;
        this.scriptableObject = null;
        this.assetObject = assetObject;
        this.scene = default(Scene);
        this.type = depth == 0 ? ItemType.TopLevel : ItemType.FolderGroup;
        this.canBeParent = canBeParent;
    }

    public ScriptableObjectTreeViewItem(ScriptableObject scriptableObject, int depth) : this(scriptableObject, depth, scriptableObject.name) {}
    public ScriptableObjectTreeViewItem(ScriptableObject scriptableObject, int depth, string displayName) : this(scriptableObject, depth, displayName, ItemType.ScriptableObject) {}
    public ScriptableObjectTreeViewItem(ScriptableObject scriptableObject, int depth, string displayName, ItemType type) : base() {
        base.depth = depth;
        base.displayName = displayName;

        if ((representsObject = scriptableObject != null)) {
            base.id               = scriptableObject.GetInstanceID();
            this.scriptableObject = scriptableObject;
            this.assetObject      = scriptableObject;
            base.icon             = (Texture2D) EditorGUIUtility.ObjectContent(scriptableObject, scriptableObject.GetType()).image;
        } else {
            base.id               = 0;
            this.scriptableObject = null;
        }

        this.scene = default(Scene);
        this.type = type;
        this.canBeParent = false;
    }

    public bool HasLabels(string[] labels) {
        return AllFound(labels, AssetDatabase.GetLabels(assetObject));
    }

    private bool AllFound(string[] labels, string[] assetLabels) {
        for (int i = 0; i < labels.Length; i++) {
            var found = false;
            var label = labels[i];
            for (int j = 0; j < assetLabels.Length; j++) {
                if (label.Equals(assetLabels[j], StringComparison.Ordinal)) {
                    found = true;
                    break;
                }
            }
            if (!found) return false;
        }
        return true;
    }

    public override string displayName {
        get {
            if (type == ItemType.Scene)   return string.IsNullOrEmpty(scene.name) ? "Untitled" : scene.name;
            if (!representsObject)        return base.displayName;
            if (scriptableObject == null) return "deleted scriptable object";
            else                          return scriptableObject.name;
        }
        set { base.displayName = value; }
    }

    public string path {
        get {
            string[] folders = new string[depth+1];
            TreeViewItem currentItem = this;
            for (int i = depth; i >= 0; i--) {
                folders[i] = currentItem.displayName;
                currentItem = currentItem.parent;
            }
            return string.Join("/", folders);
        }
    }
}

