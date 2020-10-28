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
        [@MenuItem("GameObject/HTFramework ILHotfix/ILHotfix Environment", true)]
        private static bool CreateILHotfixValidate()
        {
            return Object.FindObjectOfType<ILHotfixManager>() == null;
        }
        /// <summary>
        /// 新建ILHotfix主环境
        /// </summary>
        [@MenuItem("GameObject/HTFramework ILHotfix/ILHotfix Environment", false, 0)]
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
                Log.Error("新建ILHotfix主环境失败，丢失主预制体：Assets/HTFrameworkILHotfix/HTFrameworkILHotfix.prefab");
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
            CreateScriptFormTemplate(EditorPrefsTableILHotfix.Script_ILHotfixProcedure_Folder, "ILHotfixProcedure", "ILHotfixProcedureTemplate");
        }

        /// <summary>
        /// 从模板创建脚本
        /// </summary>
        /// <param name="prefsKey">脚本配置路径Key</param>
        /// <param name="scriptType">脚本类型</param>
        /// <param name="templateName">脚本模板名称</param>
        /// <param name="replace">脚本替换字段</param>
        /// <returns>脚本名称</returns>
        public static string CreateScriptFormTemplate(string prefsKey, string scriptType, string templateName, params string[] replace)
        {
            string directory = EditorPrefs.GetString(prefsKey, "");
            string fullPath = Application.dataPath + directory;
            if (!Directory.Exists(fullPath)) fullPath = Application.dataPath;

            string path = EditorUtility.SaveFilePanel("Create " + scriptType + " Class", fullPath, "New" + scriptType, "cs");
            if (!string.IsNullOrEmpty(path))
            {
                if (!path.Contains(Application.dataPath))
                {
                    Log.Error("新建 " + scriptType + " 失败：创建路径必须在当前项目的 Assets 路径下！");
                    return "<None>";
                }

                string className = path.Substring(path.LastIndexOf("/") + 1).Replace(".cs", "");
                if (!File.Exists(path))
                {
                    TextAsset asset = AssetDatabase.LoadAssetAtPath(EditorPrefsTableILHotfix.ScriptTemplateFolder + templateName + ".txt", typeof(TextAsset)) as TextAsset;
                    if (asset)
                    {
                        string code = asset.text;
                        code = code.Replace("#SCRIPTNAME#", className);
                        if (replace != null && replace.Length > 0)
                        {
                            for (int i = 0; i < replace.Length; i++)
                            {
                                code = code.Replace(replace[i], className);
                            }
                        }
                        File.AppendAllText(path, code);
                        asset = null;
                        AssetDatabase.Refresh();

                        string assetPath = path.Substring(path.LastIndexOf("Assets"));
                        TextAsset csFile = AssetDatabase.LoadAssetAtPath(assetPath, typeof(TextAsset)) as TextAsset;
                        EditorGUIUtility.PingObject(csFile);
                        Selection.activeObject = csFile;
                        AssetDatabase.OpenAsset(csFile);
                        EditorPrefs.SetString(prefsKey, path.Substring(0, path.LastIndexOf("/")).Replace(Application.dataPath, ""));
                        return className;
                    }
                    else
                    {
                        Log.Error("新建 " + scriptType + " 失败：丢失脚本模板文件！");
                    }
                }
                else
                {
                    Log.Error("新建 " + scriptType + " 失败：已存在类文件 " + className);
                }
            }
            return "<None>";
        }
        #endregion
    }
}