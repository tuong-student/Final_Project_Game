using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

namespace NOOD.Extension
{
    public static class EnumExtension
    {
        public static T AddToEnum<T>(this T sourceEnum, string value, T defaultValue) where T : struct
        {
            if(string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            T result;
            return Enum.TryParse<T>(value, out result) ? result : defaultValue;
        }
    }

    public static class StringExtension
    {
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            T result;
            return Enum.TryParse<T>(value, out result) ? result : defaultValue;
        }
    }

    public static class CollectorExtension
    {
        public static T GetRandom<T>(this List<T> list) where T : class
        {
            int r = UnityEngine.Random.Range(0, list.Count - 1);
            return list[r];
        }
    }

#if UNITY_EDITOR
    public static class EnumCreator
    {
        const string extension = ".cs";
        public static void WriteToEnum<T>(string enumFolderPath, string enumFileName, ICollection<string> data)
        {
            using (StreamWriter file = File.CreateText(enumFolderPath + enumFileName + extension))
            {
                file.WriteLine("public enum " + typeof(T).Name + "\n{ ");

                int i = 0;
                foreach (var line in data)
                {
                    string lineRep = line.ToString().Replace(" ", string.Empty);
                    if (!string.IsNullOrEmpty(lineRep))
                    {
                        file.WriteLine(string.Format("	{0} = {1},",
                            lineRep, i));
                        i++;
                    }
                }

                file.WriteLine("}");
                AssetDatabase.ImportAsset(enumFolderPath + enumFileName + extension);
            }
        }
    }
#endif

#if UNITY_EDITOR
    public static class RootPathExtension<T> 
    {
        public static string RootPath
        {
            get 
            {
                var g = AssetDatabase.FindAssets($"t:Script {typeof(T).Name}");
                return AssetDatabase.GUIDToAssetPath(g[0]);
            }
        }
    }
#endif

    public static class FileExtension
    {
        public static FileStream CreateFile(string folderPath, string fileName, string extension)
        {
            string filePath = Path.Combine(folderPath, fileName + extension);
            return File.Create(filePath);
        }
        public static void WriteToFile(string filePath, string text)
        {
            CreateFolderIfNeed(Path.GetDirectoryName(filePath));

            using(StreamWriter file = File.CreateText(filePath))
            {
                file.Write(text);
            }
        }
        public static string ReadFile(string filePath, string extension)
        {
            return File.ReadAllText(filePath + extension);
        }
        public static void CreateFolderIfNeed(string directory)
        {
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        public static bool IsExitFileInDefaultFolder(string fileName)
        {
            UnityEngine.Object resources = Resources.Load(Path.Combine("Datas", fileName));
            if (resources == null) return false;
            return true;
        }
    }
    public static class InputActionExtension
    {
        public static void SaveKeyBinding(this InputAction inputAction)
        {
            PlayerPrefs.SetString(inputAction.name, inputAction.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        }
        public static void LoadKeyBinding(this InputAction inputAction)
        {
            if(PlayerPrefs.HasKey(inputAction.name))
            {
                inputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(inputAction.name));
            }
        }
    }
}
