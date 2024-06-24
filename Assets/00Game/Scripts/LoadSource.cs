using System.IO;
using System.Linq;
using UnityEngine;

    public static class LoadSource
    {
        #region Methob
        public static T LoadObject<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
        #endregion

        #region Class
        [System.Serializable]
        public class SpriteArr
        {
            public Sprite[] spriteArr;
        }
        #endregion

#if UNITY_EDITOR
        /// <summary>
        /// Path: Bat dau tu folder ben trong Assets
        /// </summary>
        public static int NumberItem(string path)
        {
            var d = new DirectoryInfo(Application.dataPath + "/" + path);
            FileInfo[] fis = System.Array.FindAll(d.GetFiles(), f => f.Name.Contains(".meta"));

            return fis.Length;
        }

        /// <summary>
        /// Path: Bat dau tu folder ben trong Assets
        /// </summary>
        public static int NumberItem(string path, string containtedSt)
        {
            var d = new DirectoryInfo(Application.dataPath + "/" + path);
            FileInfo[] fis = System.Array.FindAll(d.GetFiles(), f => f.Name.Contains(".meta"));

            return System.Array.FindAll(fis, f => f.Name.Contains(containtedSt)).Length;
        }

        /// <summary>
        /// Path: Bat dau tu folder ben trong Assets
        /// </summary>
        public static string[] NumberItemName(string path)
        {
            var d = new DirectoryInfo(Application.dataPath + "/" + path);

            FileInfo[] fis = System.Array.FindAll(d.GetFiles(), f => f.Name.Contains(".meta"));

            var st = new string[fis.Length];
            for (int i = 0; i < st.Length; i++)
            {
                st[i] = fis[i].Name.Replace(".meta", "");
            }

            return st;
        }

        public static T[] LoadAllAssetAtPath<T>(string path) where T : Object
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { path });

            T[] objs = new T[guids.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i] = (T)UnityEditor.AssetDatabase.LoadAssetAtPath(
                    UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]), typeof(T));
            }
            return objs;
        }

        public static T LoadAssetAtPath<T>(string path) where T : Object
        {
            return (T)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T));
        }

        public static Sprite[] LoadSpriteSheet(string path)
        {
            var listObjs = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
            var sprites = new Sprite[listObjs.Length - 1];
            for (int i = 1; i < listObjs.Length; i++)
            {
                sprites[i - 1] = (Sprite)listObjs[i];
            }

            return sprites;
        }
#endif
    }

