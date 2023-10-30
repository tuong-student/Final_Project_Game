using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOOD.SerializableDictionary;
using NOOD.Extension;
using NOOD.NoodCustomEditor;
using UnityEditor;
using NOOD.Sound;

[CreateAssetMenu(fileName = "SoundData", menuName = "SoundData")]
public class SoundDataSO : ScriptableObject
{
    [SerializeField] public SerializableDictionary<string, AudioClip> soundDic = new SerializableDictionary<string, AudioClip>();
    [SerializeField] public SerializableDictionary<string, AudioClip> musicDic = new SerializableDictionary<string, AudioClip>();

#if UNITY_EDITOR
    public void GenerateSoundEnum()
    {
        string filePath = RootPathExtension<SoundManager>.RootPath;
        string folderPath = filePath.Replace("CustomEditor/SoundManagerEditor.cs", "Extension/");
        EnumCreator.WriteToEnum<SoundEnum>(folderPath, "SoundEnum", soundDic.Dictionary.Keys.ToList());
        EnumCreator.WriteToEnum<MusicEnum>(folderPath, "MusicEnum", musicDic.Dictionary.Keys.ToList());
        AssetDatabase.Refresh();
    }
#endif
}
