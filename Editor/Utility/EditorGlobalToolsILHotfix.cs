using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HT.Framework.ILHotfix
{
    /// <summary>
    /// HT.Framework.ILHotfix编辑器全局工具
    /// </summary>
    public static class EditorGlobalToolsILHotfix
    {
        #region 层级视图新建菜单
        /// <summary>
        /// 【验证函数】新建ILHotfix主环境
        /// </summary>
        [@MenuItem("GameObject/HTFramework.ILHotfix/ILHotfix Environment", true)]
        private static bool CreateILHotfixValidate()
        {
            return Object.FindObjectOfType<ILHotfixManager>() == null;
        }
        /// <summary>
        /// 新建ILHotfix主环境
        /// </summary>
        [@MenuItem("GameObject/HTFramework.ILHotfix/ILHotfix Environment", false, 0)]
        private static void CreateILHotfix()
        {
            Object asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/HTFrameworkILHotfix/HTFrameworkILHotfix.prefab");
            if (asset)
            {
                GameObject main = PrefabUtility.InstantiatePrefab(asset) as GameObject;
                main.name = "HTFrameworkILHotfix";
                main.transform.localPosition = Vector3.zero;
                main.transform.localRotation = Quaternion.identity;
                main.transform.localScale = Vector3.one;
                Selection.activeGameObject = main;
                EditorSceneManager.MarkSceneDirty(main.scene);
            }
            else
            {
                GlobalTools.LogError("新建ILHotfix主环境失败，丢失主预制体：Assets/HTFrameworkILHotfix/HTFrameworkILHotfix.prefab");
            }
        }
        #endregion

        #region 工程视图新建菜单
        /// <summary>
        /// 【验证函数】新建ILHotfixProcedure类
        /// </summary>
        [@MenuItem("Assets/Create/HTFramework ILHotfix/[ILHotfix] C# ILHotfixProcedure Script", true)]
        private static bool CreateILHotfixProcedureValidate()
        {
            return AssetDatabase.IsValidFolder("Assets/ILHotfix");
        }

        /// <summary>
        /// 新建ILHotfixProcedure类
        /// </summary>
        [@MenuItem("Assets/Create/HTFramework ILHotfix/[ILHotfix] C# ILHotfixProcedure Script", false, 0)]
        private static void CreateILHotfixProcedure()
        {
            string path = EditorUtility.SaveFilePanel("新建 ILHotfixProcedure 类", Application.dataPath + "/ILHotfix", "NewILHotfixProcedure", "cs");
            if (path != "")
            {
                string className = path.Substring(path.LastIndexOf("/") + 1).Replace(".cs", "");
                if (!File.Exists(path))
                {
                    TextAsset asset = AssetDatabase.LoadAssetAtPath("Assets/HTFrameworkILHotfix/Editor/ILHotfix/Template/ILHotfixProcedureTemplate.txt", typeof(TextAsset)) as TextAsset;
                    if (asset)
                    {
                        string code = asset.text;
                        code = code.Replace("#SCRIPTNAME#", className);
                        File.AppendAllText(path, code);
                        asset = null;
                        AssetDatabase.Refresh();

                        string assetPath = path.Substring(path.LastIndexOf("Assets"));
                        TextAsset cs = AssetDatabase.LoadAssetAtPath(assetPath, typeof(TextAsset)) as TextAsset;
                        EditorGUIUtility.PingObject(cs);
                        Selection.activeObject = cs;
                        AssetDatabase.OpenAsset(cs);
                    }
                }
                else
                {
                    GlobalTools.LogError("新建ILHotfixProcedure失败，已存在类型 " + className);
                }
            }
        }
        #endregion
    }
}