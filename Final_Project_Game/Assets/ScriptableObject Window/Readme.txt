
********************************

      What are ScriptableObjects ?

********************************

ScriptableObjects are general data containers that Unity can use
to store any data that Unity is familiar with natively.

Even when serialized, these objects can be used with Generics,
and they can be stored either in a scene, a separate file or
in an asset bundle.

If you're unfamiliar with ScriptableObjects, at this point,
make sure to check out this tutorial, made by Unity, before you start.

  https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/scriptable-objects


******************************

             How to use the
        ScriptableObject Window

*****************************


1. Import ScriptableObject Window from the Asset Store.

2. Open the window by clicking on 'Window' -> 'Scriptable Objects'.

3. The window shows any loaded ScriptableObjects at all times.
 * Objects that are only referenced from a scene
 * Objects that live in *.asset files
 * Objects that are temporarily only stored in memory.
 * Objects that are loaded from asset bundles (Unity version 2017.1 and newer)

4. If you imported the 'Example', create a new object by
   clicking on the 'Create' button in the window.

   If you didn't import the 'Example' folder, you will need to do
   this later, because you might not yet have any ScriptableObject
   classes that have the [CreateAssetMenu] attribute.
   In that case, nothing will happen when you click on 'Create'.

5. Add your own ScriptableObject classes to the 'Create' button by
   adding the 'CreateAssetMenu' attribute to the class, like this:

     //
     // Animal.cs
     //
     [CreateAssetMenu]
     public class Animal : ScriptableObject {
        // Example data below
        public Color color;
     }

   Now the 'Animal' option will be listed when you click the
   'Create' button.

6. Assigning your instance.

   When an instance of your class has been created, it will
   be listed in the 'Temporary' section of the window.
   This is because there is nothing in any of the open
   scenes that is refering to it yet.

   Before you can assign your ScriptableObject, you need
   to have a MonoBehaviour that refrers to it:

     //
     // AnimalComponent.cs
     //
     public class AnimalComponent : MonoBehaviour {
        public Animal animal;
     }

   After you have connected an object in the scene,
   (that has an 'Animal' reference) to your new instance,
   it will no longer be listed under 'Temporary'.

   a. Select the GameObject that has an 'Animal' reference.
   b. Make sure you also have an Inspector Window open.
   c. Use drag-and-drop to assign your object from the
      ScriptableObject Window to the Inspector Window.

   The instance will list under the section that represents the
   scene in which the object that refers to the instance
   lives.

7. Creating assets.
   When an object is stored as an asset, it ceases to be
   stored inside the scenes that use it. Unity automatically
   handles this, and makes sure that the object is available
   from any scene that needs it regardless.

   To make an asset (a file) from an object that is not yet
   listed in the 'Assets' section of the window, you can
   drag-and-drop it from the scene it's currently stored
   in, into the 'Assets' section.

   For objects that are listed in the 'Temporary' section,
   this only works when you hold the shift button while
   drag-and-dropping. This is because objects that are
   assets, but not refered from any open scene, are not
   listed at all. So when you do this, an asset will be
   created in the project, but the object will seem to
   disappear in the ScriptableObject Window.
   It means that it's not relevant for the currently
   open scene, and will not load alongside it, unless
   it's loaded through code.

   If you intend to use the object from the scene, make
   sure to assign it to some object in the scene before
   you create the asset file.
   Or you can simply assign it by drag-and-dropping from
   the project view instead :-)
