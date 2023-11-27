using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;

using UObject = UnityEngine.Object;
using ItemType = ScriptableObjectTreeViewItem.ItemType;

public static class SerializedPropertyExtension {
    private static bool[] checkedTypes = new bool[] {
        true,  false, false, false,
        false, false, true,  false,
        false, false, false, false,
        false, false, false, false,
        false, false, false, false,
        false, false, false, false,
        false, true
    };
    public static bool NextForObjectRefs(this SerializedProperty property, SerializedProperty endProperty, ref SerializedPropertyType propertyType) {
        var result = property.Next(checkedTypes[(int)propertyType+1]);
        while (result && property != endProperty) {
            propertyType = property.propertyType;
            if (checkedTypes[(int)propertyType+1]) return true;
            result = property.Next(false);
        }
        return false;
    }
}

public static class SerializedObjectExtension {
    public static SerializedProperty StartProperty(this SerializedObject so, out SerializedProperty endProperty, out bool valid, System.Text.StringBuilder sb) {
        var result = so.GetIterator();
        string type = result.type;
        if (sb != null) {
            sb.AppendFormat("\tpath: {0} - type: {1} - propertyType: {2} - isArray: {3} - hasChildren: {4}\n",
                    result.propertyPath, result.type, result.propertyType, result.isArray, result.hasChildren);
        }
        result.Next(true);
        endProperty = result.GetEndProperty();
        if (string.CompareOrdinal(type, "MonoBehaviour") == 0) {
            for (int i = 0; i<8; i++)
                result.Next(false);
        } else if (string.CompareOrdinal(type, "Behaviour") == 0) {
            for (int i = 0; i<4; i++)
                result.Next(false);
        }
        valid = result.Next(false);
        return result;
    }
}

class ScriptableObjectTreeView : TreeView {
    public ScriptableObjectTreeView(TreeViewState treeViewState) : base(treeViewState) {
        Reload();
    }

    private HashSet<ScriptableObject>    orphanObjectSet;
    private HashSet<ScriptableObject>     assetObjectSet;
    private Dictionary<Scene, HashSet<ScriptableObject>> sceneScriptableObjects;
    private Dictionary<ScriptableObject, HashSet<UObject>> sceneBackReferences;

    // Caches
    private Dictionary<Scene, SerializedObject[]> allSerializedObjectsCreate;
    private Dictionary<Scene, SerializedObject[]> allSerializedObjectsCarry;
    private Dictionary<UObject, SerializedObject> fullSerializedObjectCacheCreate;
    private Dictionary<UObject, SerializedObject> fullSerializedObjectCacheCarry;
    private Dictionary<ScriptableObject, SerializedProperty> nameProps;
    private HashSet<UObject>    checkedObjects;
    private HashSet<GameObject> checkedPrefabs;
    private HashSet<UObject>    checkedRefs;
    private HashSet<int>        uniqueIDs;
    private List<SerializedObject> currentSerializedObjects;
    private List<SerializedObject> nextSerializedObjects;
    private List<ScriptableObject> tempScriptableObjects;
    private List<TreeViewItem>     tempAllItems;

    private List<ScriptableObject> objectsToFocus;

    private Texture2D _clipboardIcon, _folderIcon;

    private Texture2D clipboardIcon {
        get { return _clipboardIcon ?? (_clipboardIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ScriptableObject Window/Icons/Clipboard.png")); } }
    private Texture2D folderIcon {
        get { return _folderIcon ?? (_folderIcon = EditorGUIUtility.FindTexture("Folder Icon")); } }

    protected override TreeViewItem BuildRoot() {
        ClearCollection(ref sceneScriptableObjects);
        ClearCollection(ref tempScriptableObjects);
        ClearCollection(ref orphanObjectSet);
        ClearCollection(ref assetObjectSet);
        ClearCollection(ref fullSerializedObjectCacheCreate);
        InitCollection (ref fullSerializedObjectCacheCarry);
        ClearCollection(ref nameProps);
        ClearCollection(ref checkedPrefabs);
        ClearCollection(ref checkedRefs);
        ClearCollection(ref allSerializedObjectsCreate);
        InitCollection (ref allSerializedObjectsCarry);
        ClearEntryLists(ref sceneBackReferences);

        foreach (ScriptableObject o in UObject.FindObjectsOfType(typeof(ScriptableObject)))
            if (!AssetDatabase.Contains(o))
                orphanObjectSet.Add(o);

        GetSerializedComponents(ref allSerializedObjectsCarry, ref allSerializedObjectsCreate);

        //Dictionary<string, int> typeCounter = new Dictionary<string, int>();

        var allSerializedObjectsEnumerator = allSerializedObjectsCarry.GetEnumerator();
        try { while (allSerializedObjectsEnumerator.MoveNext()) {
            var sceneSerializedObjects = allSerializedObjectsEnumerator.Current;
            ClearCollection(ref checkedObjects);
            InitCollection(sceneSerializedObjects.Value, ref currentSerializedObjects);
            HashSet<ScriptableObject> finalResult;
            GetNamedSet(sceneScriptableObjects, out finalResult, sceneSerializedObjects.Key);
            while (currentSerializedObjects.Count > 0) {
                ClearCollection(ref nextSerializedObjects);
                for (int i = 0; i < currentSerializedObjects.Count; i++) {
                    SerializedObject so = currentSerializedObjects[i];
                    if (!checkedObjects.Contains(so.targetObject)) {
                        checkedObjects.Add(so.targetObject);
                        SerializedPropertyType propertyType;
                        SerializedProperty end; bool valid;
                        SerializedProperty sp = so.StartProperty(out end, out valid, null);
                        if (valid && sp != end) {
                            propertyType = sp.propertyType;
                            do {
                                //int typeCount;
                                //var t = sp.type + " - " + so.targetObject.GetType().ToString();
                                //if (typeCounter.TryGetValue(t, out typeCount)) {
                                //    typeCounter[t] = typeCount+1;
                                //} else /*if (!t.StartsWith("PPtr<Transform>"))*/ {
                                //    typeCounter[t] = 1;
                                //}
                                if (propertyType == SerializedPropertyType.ObjectReference) {
                                    var scriptableObjectReference = sp.objectReferenceValue as ScriptableObject;
                                    if (scriptableObjectReference != null) {
                                        if (!assetObjectSet.Contains(scriptableObjectReference)) {
                                            if (AssetDatabase.Contains(scriptableObjectReference)) {
                                                assetObjectSet.Add(scriptableObjectReference);
                                            } else {
                                                finalResult.Add(scriptableObjectReference);
                                                orphanObjectSet.Remove(scriptableObjectReference);
                                            }
                                        }
                                        HashSet<UObject> objectSet;
                                        if (!sceneBackReferences.TryGetValue(scriptableObjectReference, out objectSet))
                                            sceneBackReferences[scriptableObjectReference] = objectSet = new HashSet<UObject>();
                                        var componentValue = so.targetObject as Component;
                                        if (componentValue != null)
                                            objectSet.Add( componentValue.gameObject );
                                        else {
                                            var scriptableObjectValue = so.targetObject as ScriptableObject;
                                            if (scriptableObjectValue != null)
                                                objectSet.Add( scriptableObjectValue );
                                        }

                                        SerializedObject cachedSO;
                                        if (fullSerializedObjectCacheCarry.TryGetValue(scriptableObjectReference, out cachedSO)) {
                                            fullSerializedObjectCacheCreate[scriptableObjectReference] = cachedSO;
                                            cachedSO.UpdateIfRequiredOrScript();
                                            nextSerializedObjects.Add(cachedSO);
                                        } else {
                                            SerializedObject newSO = new SerializedObject(scriptableObjectReference);
                                            nextSerializedObjects.Add(newSO);
                                            fullSerializedObjectCacheCreate[scriptableObjectReference] = newSO;
                                            nameProps[scriptableObjectReference] = newSO.FindProperty("m_Name");
                                        }
                                    } else {
                                        var objectRef = sp.objectReferenceValue;
                                        if (objectRef != null && EditorUtility.IsPersistent(objectRef)) {
                                            var prefabAssetType = PrefabUtility.GetPrefabAssetType(objectRef);
                                            if (prefabAssetType == PrefabAssetType.Regular ||
                                                prefabAssetType == PrefabAssetType.Variant   ) {

                                                GameObject gameObjectReference = objectRef as GameObject;
                                                if (gameObjectReference == null)
                                                    gameObjectReference = (objectRef as Component).gameObject;
                                                gameObjectReference = gameObjectReference.transform.root.gameObject;
                                                if (!checkedPrefabs.Contains(gameObjectReference)) {
                                                    checkedPrefabs.Add(gameObjectReference);
                                                    var gameObjectComponents = gameObjectReference.GetComponentsInChildren<Component>(true);
                                                    for (int j = 0; j < gameObjectComponents.Length; j++) {
                                                        var componentRef = gameObjectComponents[j];
                                                        if (componentRef != null && !emptyTypes.Contains(componentRef.GetType())) {
                                                            SerializedObject cachedSO;
                                                            if (fullSerializedObjectCacheCarry.TryGetValue(componentRef, out cachedSO)) {
                                                                fullSerializedObjectCacheCreate[componentRef] = cachedSO;
                                                                cachedSO.UpdateIfRequiredOrScript();
                                                                nextSerializedObjects.Add(cachedSO);
                                                            } else {
                                                                SerializedObject newSO = new SerializedObject(componentRef);
                                                                nextSerializedObjects.Add(newSO);
                                                                fullSerializedObjectCacheCreate[componentRef] = newSO;
                                                            }
                                                        }
                                                    }
                                                }
                                            } else if (prefabAssetType == PrefabAssetType.NotAPrefab && !checkedRefs.Contains(objectRef)) {
                                                checkedRefs.Add(objectRef);
                                                if(objectRef != null && checkedAssetTypes.Contains(objectRef.GetType()) && AssetDatabase.Contains(objectRef)) {
                                                    // AnimatorControllers will end up here, and they might link further
                                                    SerializedObject cachedSO;
                                                    if (fullSerializedObjectCacheCarry.TryGetValue(objectRef, out cachedSO)) {
                                                        fullSerializedObjectCacheCreate[objectRef] = cachedSO;
                                                        cachedSO.UpdateIfRequiredOrScript();
                                                        nextSerializedObjects.Add(cachedSO);
                                                    } else {
                                                        SerializedObject newSO = new SerializedObject(objectRef);
                                                        nextSerializedObjects.Add(newSO);
                                                        fullSerializedObjectCacheCreate[objectRef] = newSO;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            } while (sp.NextForObjectRefs(end, ref propertyType));
                        }
                    }
                }
                var unusedList = currentSerializedObjects; // Reuse list for GC benefit
                currentSerializedObjects = nextSerializedObjects;
                nextSerializedObjects = unusedList;
            }
        } } finally {
            allSerializedObjectsEnumerator.Dispose();
        }

        //var sb = new StringBuilder();
        //foreach (var kvp in typeCounter) {
        //    sb.AppendFormat("{0}: {1}\n", kvp.Key, kvp.Value);
        //}
        //Debug.Log(sb.ToString());

        //Debug.LogFormat("{0} propertyPaths, {1} propertyType checks {2} serialized objects", c.Count, cCount, fullSerializedObjectCacheCarry.Count);
        //var sb = new System.Text.StringBuilder();
        //var enumerator = c.GetEnumerator();
        //for (int i = 0; i < c.Count && i < 40; i++) {
        //    if (!enumerator.MoveNext()) break;
        //    sb.AppendLine(enumerator.Current);
        //}
        //Debug.Log(sb.ToString());


        var tmp = fullSerializedObjectCacheCarry;
        fullSerializedObjectCacheCarry = fullSerializedObjectCacheCreate;
        fullSerializedObjectCacheCreate = tmp;

        RemoveEmptySets(sceneBackReferences, ref tempScriptableObjects);

        var root = new ScriptableObjectTreeViewItem( null, -1, "Root" );
        ClearCollection(ref tempAllItems);

        AddItems(sceneScriptableObjects, tempAllItems);
        AddItems("Assets",    folderIcon,    true,  assetObjectSet,  tempAllItems);
        AddItems("Temporary", clipboardIcon, false, orphanObjectSet, tempAllItems);

        SetupUniqueIds (tempAllItems);
        SetupParentsAndChildrenFromDepths (root, tempAllItems);

        return root;
    }

    public void RefocusItems() {
        if (objectsToFocus != null) {
            var selectedIds = new List<int>();
            RefocusItems(rootItem, selectedIds);
            SetSelection(selectedIds, TreeViewSelectionOptions.RevealAndFrame);
            objectsToFocus = null;
        }
    }

    private bool RefocusItems(TreeViewItem root, List<int> selection) {
        var c = root.children;
        if (c != null) {
            List<TreeViewItem>.Enumerator e = c.GetEnumerator();
            while (e.MoveNext()) {
                var item = e.Current;
                if (RefocusItems(item, selection))
                    selection.Add(item.id);
            }
        }
        var sItem = root as ScriptableObjectTreeViewItem;
        if (sItem == null) return false;
        else return objectsToFocus.Contains(sItem.scriptableObject);
    }

    public void SetSearchMode(int searchMode) {
        ((ScriptableObjectTreeViewState) state).searchMode = searchMode;
        Reload();
    }

    protected override bool DoesItemMatchSearch(TreeViewItem item, string search) {
        var sItem = item as ScriptableObjectTreeViewItem;
        if (sItem.type == ItemType.TopLevel || sItem.type == ItemType.Scene) return true;
        var searchMode = ((ScriptableObjectTreeViewState) state).searchMode;
        if (searchMode == 0) {
            string[] tokens = search.Split(null);

            string[] acceptedTypes  = Array.ConvertAll(Array.FindAll(tokens, t => t.StartsWith("t:") && t.Length != 2), t => t.Substring(2));
            string[] requiredLabels = Array.ConvertAll(Array.FindAll(tokens, t => t.StartsWith("l:") && t.Length != 2), t => t.Substring(2));
            string[] requiredNames  = Array.FindAll(tokens, t => !(t.StartsWith("t:") || t.StartsWith("l:")));

            if (acceptedTypes .Length != 0 && !DoesItemMatchType(sItem, acceptedTypes)) return false;
            if (requiredLabels.Length != 0 && !sItem.HasLabels  (requiredLabels)      ) return false;
            if (requiredNames .Length != 0 && Array.Exists(requiredNames, n => item.displayName.IndexOf(n, StringComparison.OrdinalIgnoreCase) == -1)) return false;

            return true;
        } else if (searchMode == 1)
            return item.displayName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        else // if (searchMode == 2)
            return DoesItemMatchType(sItem, search);
    }

    protected Type[] SearchableTypes {
        get { return _SearchableTypes ?? (_SearchableTypes = GetSearchableTypes()); }
    }
    protected Type[] _SearchableTypes;

    protected Type[] GetSearchableTypes() {
        var typesList = new List<Type>();
        var referencedAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < referencedAssemblies.Length; i++) {
            var assemblyTypes = referencedAssemblies[i].GetTypes();
            for (int j = 0 ; j < assemblyTypes.Length; j++) {
                var type = assemblyTypes[j];
                if (typeof(ScriptableObject).IsAssignableFrom(type))
                    typesList.Add(type);
            }
        }
        return typesList.ToArray();
    }

    protected bool DoesItemMatchType(ScriptableObjectTreeViewItem sItem, params string[] search) {
        if (sItem.type == ItemType.ScriptableObject) {
            var oType = sItem.scriptableObject.GetType();
            var st = SearchableTypes;
            for (int i = 0; i < search.Length; i++) {
                var canonical = search[i].Contains(".");
                for (int j = 0; j < st.Length; j++) {
                    var t = st[j];
                    if ((!canonical && t.    Name.Equals(search[i], StringComparison.OrdinalIgnoreCase) ||
                          canonical && t.FullName.Equals(search[i], StringComparison.OrdinalIgnoreCase)   ) &&
                        t.IsAssignableFrom(oType)) {
                        return true;
                    }
                }
            }
        } else if (sItem.type == ItemType.FolderGroup) {
            return Array.Exists(search, term => term.Equals("folder", StringComparison.Ordinal));
        }
        return false;
    }

    private GUIStyle selectionStyle {
        get { return _selectionStyle ?? (_selectionStyle = new GUIStyle("PR Label")); }
    } private GUIStyle _selectionStyle;

    protected override void RowGUI(TreeView.RowGUIArgs args) {
        var item = args.item as ScriptableObjectTreeViewItem;
        if (item == null) return;
        if (item.type == ItemType.Scene || item.type == ItemType.TopLevel) {
            var color = GUI.color;
            GUI.color *= new Color(1f, 1f, 1f, 0.9f);
            GUI.Label(args.rowRect, GUIContent.none, "ProjectBrowserTopBarBg");
            GUI.color = color;

            if (Event.current.type == EventType.Repaint) {
                selectionStyle.Draw(args.rowRect, false, false, args.selected, args.focused);
                if (isDragging && dragTarget != null && item.id == dragTarget.id && CanBeParent(item))
                    selectionStyle.Draw(args.rowRect, GUIContent.none, true, true, false, false);
            }
        }
        base.RowGUI(args);
    }

    private List<Component> components;
    private void GetSerializedComponents(ref Dictionary<Scene, SerializedObject[]> carry, ref Dictionary<Scene, SerializedObject[]> create) {
        for (int i = 0; i < EditorSceneManager.sceneCount; i++) {
            Scene scene = EditorSceneManager.GetSceneAt(i);
            var rootObjects = scene.GetRootGameObjects();

            ClearCollection(ref components);
            for (int j = 0; j < rootObjects.Length; j++)
                AddUsableItems(rootObjects[j].GetComponentsInChildren<Component>(true), components);
            SerializedObject[] aso;
            bool gotCachedList = false;
            if (!carry.ContainsKey(scene))
                aso = create[scene] = new SerializedObject[components.Count];
            else {
                var prevSerializedObjects = carry[scene];
                if (prevSerializedObjects.Length != components.Count) {
                    aso = create[scene] = new SerializedObject[components.Count];
                } else {
                    gotCachedList = true;
                    aso = create[scene] = prevSerializedObjects;
                }
            }
            if (gotCachedList && CheckCache(aso, components))
                for (int j = 0; j < components.Count; j++)
                    aso[j].UpdateIfRequiredOrScript();
            else
                for (int j = 0; j < components.Count; j++)
                    aso[j] = new SerializedObject(components[j]);
        }
        var tmp = carry;
        carry = create;
        create = tmp;
    }

    private bool CheckCache(SerializedObject[] cache, List<Component> values) {
        for (int j = 0; j < cache.Length; j++)
            if (cache[j].targetObject != values[j])
                return false;
        return true;
    }

    private static HashSet<Type> emptyTypes = new HashSet<Type> {
        typeof(Transform), typeof(MeshFilter), typeof(TextMesh), typeof(MeshRenderer),
        typeof(SkinnedMeshRenderer), typeof(ParticleSystem), typeof(TrailRenderer), typeof(LineRenderer),
        typeof(LensFlare),
        //typeof(Halo),
        typeof(Projector), typeof(Rigidbody), typeof(CharacterController),
        typeof(BoxCollider), typeof(SphereCollider), typeof(CapsuleCollider), typeof(MeshCollider),
        typeof(WheelCollider), typeof(TerrainCollider), typeof(Cloth), typeof(HingeJoint),
        typeof(FixedJoint), typeof(SpringJoint), typeof(CharacterJoint), typeof(ConfigurableJoint),
        typeof(ConstantForce),
        //typeof(Shadow), //typeof(Outline), //typeof(PositionAsUV1),
        typeof(Text), typeof(Image), typeof(RawImage), typeof(Mask),
        typeof(RectMask2D),
        //typeof(Button), //typeof(InputField), //typeof(Toggle), //typeof(ToggleGroup),
        //typeof(Slider), //typeof(Scrollbar), //typeof(Dropdown), //typeof(ScrollRect),
        //typeof(Selectable),
        typeof(AudioListener), typeof(AudioSource), typeof(AudioReverbZone), typeof(AudioLowPassFilter),
        typeof(AudioHighPassFilter), typeof(AudioEchoFilter), typeof(AudioDistortionFilter), typeof(AudioReverbFilter),
        typeof(AudioChorusFilter), typeof(NavMeshAgent),
        //typeof(OffMeshLink),
        typeof(NavMeshObstacle), typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(CircleCollider2D),
        typeof(EdgeCollider2D), typeof(PolygonCollider2D), typeof(CapsuleCollider2D), typeof(CompositeCollider2D),
        typeof(DistanceJoint2D), typeof(FixedJoint2D), typeof(FrictionJoint2D), typeof(HingeJoint2D),
        typeof(RelativeJoint2D), typeof(SliderJoint2D), typeof(SpringJoint2D), typeof(TargetJoint2D),
        typeof(WheelJoint2D), typeof(AreaEffector2D), typeof(BuoyancyEffector2D),
        typeof(PointEffector2D), typeof(PlatformEffector2D), typeof(SurfaceEffector2D), typeof(ConstantForce2D),
        //typeof(Animator), //typeof(NetworkView),
        typeof(Terrain), typeof(WindZone), typeof(BillboardRenderer),
        //typeof(EventSystem), //typeof(EventTrigger),
        typeof(Physics2DRaycaster), typeof(PhysicsRaycaster), typeof(StandaloneInputModule),
        //typeof(TouchInputModule),
        typeof(GraphicRaycaster), typeof(Camera), typeof(Skybox),
        typeof(FlareLayer),
        //typeof(GUILayer),
        typeof(Light), typeof(LightProbeGroup),
        typeof(LightProbeProxyVolume), typeof(ReflectionProbe), typeof(OcclusionArea),
        typeof(OcclusionPortal), typeof(LODGroup), typeof(SpriteRenderer), typeof(SortingGroup),
        typeof(CanvasRenderer),
        //typeof(GUITexture), typeof(GUIText),
        typeof(RectTransform),
        //typeof(Canvas),
        typeof(CanvasGroup), typeof(CanvasScaler), typeof(LayoutElement), typeof(ContentSizeFitter),
        typeof(AspectRatioFitter), typeof(HorizontalLayoutGroup), typeof(VerticalLayoutGroup),
        typeof(GridLayoutGroup),
        //typeof(NetworkAnimator),
        //typeof(NetworkDiscovery), typeof(NetworkIdentity),
        //typeof(NetworkLobbyManager),
        //typeof(NetworkLobbyPlayer), typeof(NetworkManager),
        //typeof(NetworkManagerHUD), typeof(NetworkMigrationManager), typeof(NetworkProximityChecker),
        //typeof(NetworkStartPosition), typeof(NetworkTransform)
        //typeof(NetworkTransformChild),
        //typeof(NetworkTransformVisualizer),
    };

    private static HashSet<Type> checkedAssetTypes = new HashSet<Type> {
        typeof(AnimationClip),
        typeof(AnimatorController),
        typeof(AnimatorOverrideController),
        typeof(AnimatorState),
        typeof(AnimatorStateMachine)
    };

    private void AddUsableItems(Component[] items, List<Component> destination) {
        var itemsCount = items.Length;
        for (int i = 0; i < itemsCount; i++) {
            var item = items[i];
            if (item != null && !emptyTypes.Contains(item.GetType()))
                destination.Add(item);
        }
    }

    private void AddItems(Dictionary<Scene, HashSet<ScriptableObject>> items, List<TreeViewItem> destination) {
        var e = items.GetEnumerator();
        try { while (e.MoveNext()) {
            destination.Add(new ScriptableObjectTreeViewItem(e.Current.Key));
            var e2 = e.Current.Value.GetEnumerator();
            try { while (e2.MoveNext()) {
                destination.Add(new ScriptableObjectTreeViewItem(e2.Current, 1));
            } } finally { e2.Dispose(); }
        } } finally { e.Dispose(); }
    }

    private void AddItems(string groupName, Texture2D icon, bool canBeParent, HashSet<ScriptableObject> items, List<TreeViewItem> destination) {
        destination.Add(new ScriptableObjectTreeViewItem(groupName, icon, canBeParent, null));
        var e = items.GetEnumerator();
        try { if (e.MoveNext()) {
            if (EditorUtility.IsPersistent(e.Current))
                AddPersistentItems(e, destination);
            else do { destination.Add(new ScriptableObjectTreeViewItem(e.Current, 1)); } while (e.MoveNext());
        } } finally { e.Dispose(); }
    }

    private struct ScriptableObjectWithPath {
        public ScriptableObject scriptableObject;
        public string path;
        public ScriptableObjectWithPath(ScriptableObject scriptableObject) {
            this.scriptableObject = scriptableObject;
            this.path = AssetDatabase.GetAssetPath(scriptableObject);
        }
    }

    private readonly char[] fSlash = {'/'};
    private void AddPersistentItems(HashSet<ScriptableObject>.Enumerator e, List<TreeViewItem> destination) {
        // Collect a list where we can sort our objects based on their paths
        var objects = new List<ScriptableObjectWithPath>();
        do {
            objects.Add(new ScriptableObjectWithPath(e.Current));
        } while (e.MoveNext());
        objects.Sort(ComparePersistentObjects);

        string previousPath = null;
        string[] split = null;
        int depth = 1;
        for (int i = 0; i < objects.Count; i++) {
            var objectPath = objects[i].path;

            var objectPathBuilder = new StringBuilder();
            objectPathBuilder.Append("Assets");

            // Now add folder items
            //
            // First, just establish 'split' and 'previousPath'
            if (previousPath == null) {
                split = objectPath.Split(fSlash, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 1; j < split.Length-1; j++) {
                    objectPathBuilder.Append("/").Append(split[j]);
                    var folderObject = AssetDatabase.LoadAssetAtPath<UObject>(objectPathBuilder.ToString());
                    destination.Add(new ScriptableObjectTreeViewItem(split[j], folderIcon, true, folderObject, depth));
                    depth += 1;
                }
            } else {
                string[] newSplit = objectPath.Split(fSlash, StringSplitOptions.RemoveEmptyEntries);
                int commonFolders = 0;
                // If the previous folder is contained, we know how many folders are common
                // Else we skip 'Assets' and start counting equal folder names in the prefix
                if (objectPath.StartsWith(previousPath))
                    commonFolders = split.Length - 2;
                else
                    for (int j = 1; j < split.Length-1 && j < newSplit.Length-1; j++) {
                        if (string.CompareOrdinal(split[j], newSplit[j]) == 0)
                            commonFolders++;
                        else
                            break;
                    }
                depth = 1 + commonFolders;

                for (int j = 1; j < depth; j++)
                    objectPathBuilder.Append("/").Append(newSplit[j]);

                // The common folders are already added, the rest will added here
                for (int j = depth; j < newSplit.Length - 1; j++) {
                    objectPathBuilder.Append("/").Append(newSplit[j]);
                    var folderObject = AssetDatabase.LoadAssetAtPath<UObject>(objectPathBuilder.ToString());
                    destination.Add(new ScriptableObjectTreeViewItem(newSplit[j], folderIcon, true, folderObject, depth));
                    depth += 1;
                }
                split = newSplit;
            }
            previousPath = Path.GetDirectoryName(objectPath);

            // And finally add the asset item
            destination.Add(new ScriptableObjectTreeViewItem(objects[i].scriptableObject, depth));
        }
    }

    private int ComparePersistentObjects(ScriptableObjectWithPath a, ScriptableObjectWithPath b) {
        return ComparePersistentObjects(a.path, b.path);
    }
    private int ComparePersistentObjects(string a, string b, bool debug = false) {
        var numSlashesA = CountCharacter(a, '/');
        var numSlashesB = CountCharacter(b, '/');
        var minSlashes = numSlashesA < numSlashesB ? numSlashesA : numSlashesB;

        int folderStartA, folderStartB;
        folderStartA = folderStartB = 0;

        // While both are in a folder, the earliest supperior folder wins
        for (int i = 0; i < minSlashes; i++) { // skip 'Assets' and the file name
            var folderEndA = a.IndexOf('/', folderStartA);
            var folderEndB = b.IndexOf('/', folderStartB);

            if (folderEndA == -1) folderEndA = a.Length;
            if (folderEndB == -1) folderEndB = b.Length;

            int folderComparison = NaturalCompare(a, folderStartA, folderEndA,
                                                  b, folderStartB, folderEndB );
            if (folderComparison != 0)
                return folderComparison;

            folderStartA = folderEndA + 1;
            folderStartB = folderEndB + 1;
        }

        // If one path is a subpath of the other, the deepest path wins
        if (numSlashesA != numSlashesB)
            return numSlashesB - numSlashesA;

        // It now has come down to the file name
        folderStartA = a.LastIndexOf('/') + 1;
        folderStartB = b.LastIndexOf('/') + 1;
        return NaturalCompare(a, folderStartA, a.Length,
                              b, folderStartB, b.Length);
    }

    private static int NaturalCompare(string s1, int i1, int s1End, string s2, int i2, int s2End/*, bool debug = false*/) {
        while (i1 < s1End && i2 < s2End) {
            var numIndex1 = IndexOfNumber(s1, i1, s1End);
            var numIndex2 = IndexOfNumber(s2, i2, s2End);
            if (numIndex1 == i1 && numIndex2 == i2) {
                var numC = NumCompare(s1, numIndex1, s1End, s2, numIndex2, s2End);
                if (numC != 0) return numC;
                var next1 = IndexOfNonNumber(s1, numIndex1, s1End);
                var next2 = IndexOfNonNumber(s2, numIndex2, s2End);
                i1 = next1;
                i2 = next2;
            } else {
                if      (numIndex1 == i1) numIndex1 = IndexOfNonNumber(s1, i1, s1End);
                else if (numIndex2 == i2) numIndex2 = IndexOfNonNumber(s2, i2, s2End);
                var pLength1 = numIndex1 - i1;
                var pLength2 = numIndex2 - i2;
                var pLength  = pLength1 < pLength2 ? pLength1 : pLength2;
                var pVal     = string.Compare(s1, i1, s2, i2, pLength, true);
                if (pVal != 0) return pVal;
                i1 += pLength;
                i2 += pLength;
            }
        }
        return (s1End - i1) - (s2End - i2);
    }

    private static int CountCharacter(string s, char c) {
        var total = 0;
        for (int i = 0; i < s.Length; i++) if (s[i] == c) total++;
        return total;
    }

    private static int NumCompare(string s1, int i1, int s1End, string s2, int i2, int s2End) {
        int m, v1, v2;
        v1 = m = (int) (s1[i1++] - '0');
        while (i1 < s1End && 0 <= (m = (int) (s1[i1++] - '0')) && m <= 9)
            v1 = 10 * v1 + m;

        v2 = m = (int) (s2[i2++] - '0');
        while (i2 < s2End && 0 <= (m = (int) (s2[i2++] - '0')) && m <= 9)
            v2 = 10 * v2 + m;

        return v1 - v2;
    }

    public static int IndexOfNumber(string s, int index, int end) {
        for (int i = index; i < end; i++) {
            var c = s[i];
            if ('0' <= c && c <= '9') return i;
        }
        return end;
    }

    public static int IndexOfNonNumber(string s, int index, int end) {
        for (int i = index; i < end; i++) {
            var c = s[i];
            if (c < '0' || '9' < c) return i;
        }
        return end;
    }

    private void SetupUniqueIds(List<TreeViewItem> items) {
        ClearCollection(ref uniqueIDs);
        for (int i = 0; i < items.Count; i++) {
            var item = items[i];
            if (uniqueIDs.Contains(item.id)) {
                int nextID = item.id+1;
                while (uniqueIDs.Contains(nextID))
                    nextID++;
                item.id = nextID;
            }
            uniqueIDs.Add(item.id);
        }
    }

    private void ClearCollection<T>   (ref          List<T> c) { if (c == null) c = new          List<T>(); else c.Clear(); }
    private void ClearCollection<T, U>(ref Dictionary<T, U> c) { if (c == null) c = new Dictionary<T, U>(); else c.Clear(); }
    private void ClearCollection<T>   (ref       HashSet<T> c) { if (c == null) c = new       HashSet<T>(); else c.Clear(); }
    private void ClearCollection<T>   (ref         Stack<T> c) { if (c == null) c = new         Stack<T>(); else c.Clear(); }

    private void InitCollection<T, U>(ref Dictionary<T, U> c) { if (c == null) c = new Dictionary<T, U>(); }

    private void InitCollection<T>(List<T> source, ref HashSet<T> destination) {
        if (destination == null) destination = new HashSet<T>(source);
        else { destination.Clear(); destination.UnionWith(source); }
    }

    private void InitCollection<T>(T[] source, ref List<T> destination) {
        if (destination == null)
            destination = new List<T>(source);
        else {
            destination.Clear();
            for (int i = 0; i < source.Length; i++)
                destination.Add(source[i]);
        }
    }

    private void GetNamedSet<K, T>(Dictionary<K, HashSet<T>> source, out HashSet<T> result, K key) {
        if (!(source.TryGetValue(key, out result)))
            source[key] = result = new HashSet<T>();
    }

    private void ClearEntryLists<T, U>(ref Dictionary<T, HashSet<U>> c) {
        if (c == null) c = new Dictionary<T, HashSet<U>>();
        else {
            var enumerator = c.GetEnumerator();
            try { while (enumerator.MoveNext())
                enumerator.Current.Value.Clear();
            } finally { enumerator.Dispose(); }
        }
    }

    private void RemoveEmptySets<T, U>(Dictionary<T, HashSet<U>> c, ref List<T> keyCache) {
        ClearCollection(ref keyCache);
        var e = c.GetEnumerator();
        try { while (e.MoveNext())
            if (e.Current.Value.Count == 0)
                keyCache.Add(e.Current.Key);
        } finally { e.Dispose(); }
        for (int i = 0; i < keyCache.Count; i++)
            c.Remove(keyCache[i]);
    }

    // Reordering

    protected override bool CanStartDrag (CanStartDragArgs args) {
        return true;
    }

    protected override void SetupDragAndDrop (SetupDragAndDropArgs args) {
        DragAndDrop.PrepareStartDrag ();

        var sortedDraggedIDs = SortItemIDsInRowOrder (args.draggedItemIDs);
        List<UnityEngine.Object> objList = new List<UnityEngine.Object> (sortedDraggedIDs.Count);
        foreach (var id in sortedDraggedIDs) {
            var sItem = FindItem(id, rootItem) as ScriptableObjectTreeViewItem;
            if (sItem != null && sItem.type == ItemType.ScriptableObject) {
                objList.Add(sItem.scriptableObject);
            }
        }

        DragAndDrop.objectReferences = objList.ToArray();
        string title = objList.Count > 1 ? "<Multiple>" : objList.Count > 0 ? objList[0].name : "<None>";
        DragAndDrop.StartDrag (title);

    }

    private TreeViewItem dragTarget;
    protected override DragAndDropVisualMode HandleDragAndDrop (DragAndDropArgs args) {
        var parentItem   = args.parentItem as ScriptableObjectTreeViewItem;
        var rejected     = parentItem == null || !parentItem.canBeParent;
        var uponItem     = args.dragAndDropPosition == DragAndDropPosition.UponItem;
        var visualMode   = rejected ? DragAndDropVisualMode.Rejected : DragAndDropVisualMode.Move;
        dragTarget       = uponItem ? parentItem : null;

        if (args.performDrop) {
            dragTarget = null;
            ClearCollection(ref objectsToFocus);
            if (parentItem.type == ItemType.Scene) {
                var refs = DragAndDrop.objectReferences;
                for (int i = 0; i < refs.Length; i++) {
                    var so = refs[i] as ScriptableObject;
                    if (so == null) continue;
                    if (EditorUtility.IsPersistent(so)) {
                        var copy = (ScriptableObject) UObject.Instantiate(so);
                        copy.name = so.name;
                        objectsToFocus.Add(copy);
                        // Assign the new object to all the places in all the scenes where it used to be.
                        foreach (UObject o in sceneBackReferences[so]) {
                            if (o.GetType() == typeof(GameObject)) {
                                foreach (Component component in ((GameObject) o).GetComponents<Component>()) {
                                    var serializedObject = new SerializedObject(component);
                                    var sp = serializedObject.GetIterator();
                                    do {
                                        if (sp.propertyType == SerializedPropertyType.ObjectReference && sp.objectReferenceValue == so)
                                            sp.objectReferenceValue = copy;
                                    } while (sp.Next(true));
                                    serializedObject.ApplyModifiedProperties();
                                }
                            } else {
                                var serializedObject = new SerializedObject(o);
                                var sp = serializedObject.GetIterator();
                                do {
                                    if (sp.propertyType == SerializedPropertyType.ObjectReference && sp.objectReferenceValue == so)
                                        sp.objectReferenceValue = copy;
                                } while (sp.Next(true));
                                serializedObject.ApplyModifiedProperties();
                            }
                        }
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(so));
                    }
                }
            } else {
                var refs = DragAndDrop.objectReferences;
                for (int i = 0; i < refs.Length; i++) {
                    var so = refs[i] as ScriptableObject;
                    if (so == null) continue;
                    objectsToFocus.Add(so);
                    if (EditorUtility.IsPersistent(so)) {
                        var oldPath    = AssetDatabase.GetAssetPath(so);
                        var fileName   = Path.GetFileName(oldPath);
                        var newPathRaw = parentItem.path + "/" + fileName;
                        if (oldPath != newPathRaw) {
                            var newPath    = AssetDatabase.GenerateUniqueAssetPath( newPathRaw );
                            var moveError = AssetDatabase.MoveAsset(oldPath, newPath);
                            if (string.IsNullOrEmpty(moveError)) AssetDatabase.SaveAssets();
                            else                                 Debug.LogWarning(moveError);
                        }
                    } else {
                        if (so != null) {
                            HashSet<UObject> sceneParents;
                            if (Event.current.shift || sceneBackReferences.TryGetValue(so, out sceneParents) && sceneParents.Count != 0) {
                                AssetDatabase.CreateAsset( so, AssetDatabase.GenerateUniqueAssetPath( parentItem.path + "/" + so.name + ".asset") );
                            }
                        }
                    }
                }
            }
            Reload();
            RefocusItems();
        }

        return visualMode;
    }

    protected override bool CanBeParent(TreeViewItem item) {
        var sItem = item as ScriptableObjectTreeViewItem;
        return sItem != null && sItem.canBeParent;
    }

    // Rename
    //--------

    protected override bool CanRename(TreeViewItem item) {
        if (hasSearch) return false;
        var sItem = item as ScriptableObjectTreeViewItem;
        if (sItem != null) {
            switch (sItem.type) {
                case ItemType.ScriptableObject:
                case ItemType.NameGroup:
                case ItemType.FolderGroup:
                    return true;
                default:
                    return false;
            }
        } else {
            return false;
        }
    }

    protected override void RenameEnded(RenameEndedArgs args)
    {
        if (args.acceptedRename)
        {
            var item = FindItem(args.itemID, rootItem) as ScriptableObjectTreeViewItem;
            if (item != null) {
                if (EditorUtility.IsPersistent(item.scriptableObject) && AssetDatabase.IsMainAsset(item.scriptableObject)) {
                    var oldPath = AssetDatabase.GetAssetPath(item.scriptableObject);
                    var moveError = AssetDatabase.RenameAsset(oldPath, args.newName);
                    if (string.IsNullOrEmpty(moveError)) AssetDatabase.SaveAssets();
                    else                                 Debug.LogWarning(moveError);
                } else {
                    Undo.RecordObject(item.scriptableObject, "Rename ScriptableObject");
                    item.scriptableObject.name = args.newName;
                }
            }
        }
    }

    //protected override void ContextClicked() {
    //    Debug.Log("General context click");

    //    for (int i = 0; i < EditorSceneManager.sceneCount; i++) {
    //        var scriptableObjects = new List<ScriptableObject>();
    //        foreach (GameObject o in EditorSceneManager.GetSceneAt(i).GetRootGameObjects()) {
    //            foreach (Component c in o.GetComponentsInChildren<Component>(true)) {
    //                var sb = new System.Text.StringBuilder();
    //                sb.AppendFormat("go: {0} - {1}\n", o.name, c.GetType().ToString());
    //                SerializedObject so = new SerializedObject(c);
    //                SerializedProperty end; bool valid;
    //                SerializedProperty iterator = so.StartProperty(out end, out valid, sb);
    //                if (valid)
    //                if (iterator != end) {
    //                    SerializedPropertyType propertyType = iterator.propertyType;
    //                    do {
    //                        sb.AppendFormat("\tpath: {0} - type: {1} - propertyType: {2} - isArray: {3} - hasChildren: {4}\n",
    //                                iterator.propertyPath, iterator.type, iterator.propertyType, iterator.isArray, iterator.hasChildren);
    //                        if (iterator.propertyType == SerializedPropertyType.ObjectReference) {
    //                            var scriptableObjectReference = iterator.objectReferenceValue as ScriptableObject;
    //                            if (scriptableObjectReference != null)
    //                                scriptableObjects.Add(scriptableObjectReference);
    //                        }
    //                    } while (iterator.NextForObjectRefs(end, ref propertyType));
    //                }
    //                Debug.Log(sb.ToString());
    //            }
    //        }
    //        foreach (ScriptableObject s in scriptableObjects) {
    //            var sb = new System.Text.StringBuilder();
    //            sb.AppendFormat("so: {0} - {1}\n", s.name, s.GetType().ToString());
    //            SerializedObject so = new SerializedObject(s);
    //            SerializedProperty end; bool valid;
    //            SerializedProperty iterator = so.StartProperty(out end, out valid, sb);
    //            if (valid)
    //            if (iterator != end) {
    //                SerializedPropertyType propertyType = iterator.propertyType;
    //                do {
    //                    sb.AppendFormat("\tpath: {0} - type: {1} - propertyType: {2} - isArray: {3} - hasChildren: {4}\n",
    //                            iterator.propertyPath, iterator.type, iterator.propertyType, iterator.isArray, iterator.hasChildren);
    //                } while (iterator.NextForObjectRefs(end, ref propertyType));
    //            }
    //            Debug.Log(sb.ToString());
    //        }
    //    }

    //}

    protected override void ContextClickedItem(int id) {
        var selection = GetSelection();
        List<ScriptableObjectTreeViewItem> selectedSItems = null;
        ScriptableObjectTreeViewItem sItem = null;
        if (selection.Contains(id)) {
            selectedSItems = new List<ScriptableObjectTreeViewItem> (selection.Count);
            var selectedItems = FindRows(selection);
            foreach (TreeViewItem item in selectedItems) {
                var selectedSItem = item as ScriptableObjectTreeViewItem;
                if (selectedSItem != null && selectedSItem.type == ItemType.ScriptableObject) {
                    selectedSItems.Add(selectedSItem);
                    if (selectedSItem.id == id)
                        sItem = selectedSItem;
                }
            }
        } else {
            sItem = FindItem(id, rootItem) as ScriptableObjectTreeViewItem;
            if (sItem != null && sItem.type == ItemType.ScriptableObject)
                selectedSItems = new List<ScriptableObjectTreeViewItem> { sItem };
        }
        if (selectedSItems == null || selectedSItems.Count == 0)
            return;

        var itemContextMenu = new GenericMenu();
        itemContextMenu.AddItem(new GUIContent("Duplicate"),             false, DuplicateScriptableObjects, selectedSItems);
        itemContextMenu.AddItem(new GUIContent("Destroy"),               false,   DestroyScriptableObjects, selectedSItems);
        if (sItem != null && EditorUtility.IsPersistent(sItem.scriptableObject))
            itemContextMenu.AddItem(new GUIContent("Ping containing asset"), false, PingAsset, sItem.scriptableObject);
        itemContextMenu.ShowAsContext();

        Event.current.Use();
    }

    public void DuplicateSelection() {
        DuplicateScriptableObjects(FindRows(GetSelection()).Cast<ScriptableObjectTreeViewItem>().ToList());
    }

    protected void DuplicateScriptableObjects(object sObjects) {
        var selectedSItems = sObjects as List<ScriptableObjectTreeViewItem>;
        ClearCollection(ref objectsToFocus);
        foreach (ScriptableObjectTreeViewItem sItem in selectedSItems) {
            var newItem = UnityEngine.Object.Instantiate(sItem.scriptableObject);
            objectsToFocus.Add(newItem);
            Undo.RegisterCreatedObjectUndo(newItem, "Duplicate object");
        }
        Reload();
        RefocusItems();
    }

    public void CreateScriptableObject(Type type, string name) {
        var newObject = ScriptableObject.CreateInstance(type);
        newObject.name = string.IsNullOrEmpty(name) ? type.Name : name;
        Selection.objects = new UObject[] { newObject };
        ClearCollection(ref objectsToFocus);
        objectsToFocus.Add(newObject);
        Reload();
        RefocusItems();
    }

    public void DeleteSelection() {
        DestroyScriptableObjects(FindRows(GetSelection()).Cast<ScriptableObjectTreeViewItem>().ToList());
    }

    protected void DestroyScriptableObjects(object sObjects) {
        int numAssets = 0;
        var selectedSItems = sObjects as List<ScriptableObjectTreeViewItem>;
        foreach (ScriptableObjectTreeViewItem sItem in selectedSItems) {
            if (sItem.type == ItemType.ScriptableObject)
                if (EditorUtility.IsPersistent(sItem.scriptableObject))
                    numAssets++;
        }

        if (numAssets == selectedSItems.Count) {
            if (numAssets == 1) {
                if (EditorUtility.DisplayDialog("Delete selected asset?", "  " + selectedSItems[0].scriptableObject.name + "\n\nYou cannot undo this action.", "Delete", "Cancel"))
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedSItems[0].scriptableObject));
            } else if (numAssets > 1) {
                var sb = new StringBuilder();
                for (int i = 0; i < numAssets && i < 10; i++) {
                    sb.Append("  ").Append(AssetDatabase.GetAssetPath(selectedSItems[i].scriptableObject)).Append("\n");
                }
                if (numAssets > 10) {
                    sb.Append("and ").Append(numAssets - 10).Append(" more assets.\n");
                }
                sb.Append("\nYou cannot undo this action.");

                if (EditorUtility.DisplayDialog("Delete assets?", sb.ToString(), "Delete", "Cancel"))
                    for (int i = 0; i < numAssets; i++)
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedSItems[i].scriptableObject));
            }
        } else if (numAssets == 0)
            for (int i = 0; i < selectedSItems.Count; i++)
                Undo.DestroyObjectImmediate(selectedSItems[i].scriptableObject);
        else if (EditorUtility.DisplayDialog("Delete assets and gameObjects?", "Are you sure you want to delete objects from scenes and assets?", "Delete", "Cancel"))
            for (int i = 0; i < selectedSItems.Count; i++)
                if (EditorUtility.IsPersistent(selectedSItems[i].scriptableObject))
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedSItems[i].scriptableObject));
                else
                    Undo.DestroyObjectImmediate(selectedSItems[i].scriptableObject);
        Reload();
    }

    protected void PingAsset(object ao) {
        var assetObject = ao as UnityEngine.Object;
        if (AssetDatabase.IsMainAsset(assetObject))
            EditorGUIUtility.PingObject(assetObject);
        else
            EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(assetObject)));
    }

    protected override void SelectionChanged(IList<int> selectedIds) {
        var idCount = selectedIds.Count;
        var selectedSItems    = new List<UnityEngine.Object> (idCount);
        var selectedItems = FindRows(selectedIds);
        foreach (TreeViewItem item in selectedItems) {
            var selectedSItem = item as ScriptableObjectTreeViewItem;
            if (selectedSItem != null && selectedSItem.type == ItemType.ScriptableObject) {
                selectedSItems.Add(selectedSItem.scriptableObject);
            }
        }
        Selection.objects = selectedSItems.ToArray();
    }

    public void ExpandToRevealSelection() {
        if (HasSelection()) {
            ExpandIfChildSelected(rootItem, GetSelection());
            SetFocus();
        }
    }

    private bool ExpandIfChildSelected(TreeViewItem root, IList<int> selection) {
        var c = root.children;
        if (c != null) {
            List<TreeViewItem>.Enumerator e = c.GetEnumerator();
            while (e.MoveNext()) {
                var item = e.Current;
                if (ExpandIfChildSelected(item, selection)) {
                    SetExpanded(item.id, true);
                    return true;
                }
            }
        }
        return selection.Contains(root.id);
    }

    private List <TreeViewItem> tempRowItems;
    private List <TreeViewItem> tempGroupRowItems;
    private Stack<TreeViewItem> tempGroupRowItemStack;
    protected override IList<TreeViewItem> BuildRows(TreeViewItem root) {
        ClearCollection(ref tempRowItems);
        if (this.hasSearch) this.SearchTree     (root);
        else                this.AddExpandedRows(root, tempRowItems);
        return tempRowItems;
    }

    private void SearchTree(TreeViewItem root) {
        foreach (var topChild in root.children) {
            tempRowItems.Add(topChild);

            ClearCollection(ref tempGroupRowItems);
            ClearCollection(ref tempGroupRowItemStack);
            tempGroupRowItemStack.Push(topChild);
            while (tempGroupRowItemStack.Count != 0) {
                var item = tempGroupRowItemStack.Pop();
                if (item.children == null) continue;
                foreach (var itemChild in item.children) {
                    if (itemChild == null) continue;
                    if (DoesItemMatchSearch(itemChild, searchString))
                        tempGroupRowItems.Add(itemChild);
                    tempGroupRowItemStack.Push(itemChild);
                }
            }

            tempRowItems.AddRange(tempGroupRowItems);
        }
    }
}
