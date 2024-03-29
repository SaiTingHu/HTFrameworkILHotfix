﻿using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace HT.Framework.ILHotfix
{
    [GiteeURL("https://gitee.com/SaiTingHu/HTFrameworkILHotfix")]
    [CSDNBlogURL("https://wanderer.blog.csdn.net/article/details/96152656")]
    [GithubURL("https://github.com/SaiTingHu/HTFrameworkILHotfix")]
    [CustomEditor(typeof(ILHotfixManager))]
    internal sealed class ILHotfixManagerInspector : HTFEditor<ILHotfixManager>
    {
        private static readonly string SourceDllPath = "Library/ScriptAssemblies/ILHotfix.dll";
        private static readonly string AssetsDllPath = "Assets/ILHotfix/ILHotfix.dll.bytes";

        [InitializeOnLoadMethod]
        private static void CopyILHotfixDll()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                string sourceDllPath = PathToolkit.ProjectPath + SourceDllPath;
                if (File.Exists(sourceDllPath))
                {
                    File.Copy(sourceDllPath, PathToolkit.ProjectPath + AssetsDllPath, true);
                    AssetDatabase.Refresh();
                    Log.Info("已更新：Assets/ILHotfix/ILHotfix.dll");
                }
            }
        }

        private bool _ILHotfixIsCreated = false;
        private string _ILHotfixDirectory = "/ILHotfix/";
        private string _ILHotfixEnvironmentPath = "/ILHotfix/Environment/ILHotfixEnvironment.cs";
        private string _ILHotfixAssemblyDefinitionPath = "/ILHotfix/ILHotfix.asmdef";

        protected override bool IsEnableRuntimeData
        {
            get
            {
                return false;
            }
        }

        protected override void OnDefaultEnable()
        {
            base.OnDefaultEnable();

            _ILHotfixIsCreated = false;
            string hotfixDirectory = Application.dataPath + _ILHotfixDirectory;
            string hotfixEnvironmentPath = Application.dataPath + _ILHotfixEnvironmentPath;
            string hotfixAssemblyDefinitionPath = Application.dataPath + _ILHotfixAssemblyDefinitionPath;
            if (Directory.Exists(hotfixDirectory))
            {
                if (File.Exists(hotfixEnvironmentPath))
                {
                    if (File.Exists(hotfixAssemblyDefinitionPath))
                    {
                        _ILHotfixIsCreated = true;
                    }
                }
            }
        }
        protected override void OnInspectorDefaultGUI()
        {
            base.OnInspectorDefaultGUI();

            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("ILHotfix manager, the hot update in this game!", MessageType.Info);
            GUILayout.EndHorizontal();

            #region ILHotfixDll
            GUILayout.BeginVertical("box");

            PropertyField(nameof(ILHotfixManager.IsAutoStartUp), "Auto StartUp");

            GUILayout.BeginHorizontal();
            GUILayout.Label("ILHotfixDll AssetBundleName");
            GUILayout.EndHorizontal();

            PropertyField(nameof(ILHotfixManager.ILHotfixDllAssetBundleName), "");
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("ILHotfixDll AssetsPath");
            GUILayout.EndHorizontal();

            PropertyField(nameof(ILHotfixManager.ILHotfixDllAssetsPath), "");

            GUILayout.EndVertical();
            #endregion

            #region ILHotfixWizard
            if (_ILHotfixIsCreated)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("ILHotfix environment is created!", MessageType.Info);
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal();
                GUILayout.Label("ILHotfix Directory");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.TextField("Assets" + _ILHotfixDirectory);
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Correct ILHotfix Environment", "LargeButton"))
                {
                    SetILHotfixAssemblyDefinition(Application.dataPath + _ILHotfixAssemblyDefinitionPath);
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Create ILHotfix Environment", "LargeButton"))
                {
                    CreateILHotfixEnvironment();
                    _ILHotfixIsCreated = true;
                }
                GUILayout.EndHorizontal();
            }
            #endregion
        }

        private void CreateILHotfixEnvironment()
        {
            string hotfixDirectory = Application.dataPath + _ILHotfixDirectory;
            string hotfixEnvironmentPath = Application.dataPath + _ILHotfixEnvironmentPath;
            string hotfixAssemblyDefinitionPath = Application.dataPath + _ILHotfixAssemblyDefinitionPath;
            if (!Directory.Exists(hotfixDirectory))
            {
                Directory.CreateDirectory(hotfixDirectory);
            }
            if (!Directory.Exists(hotfixDirectory + "Environment/"))
            {
                Directory.CreateDirectory(hotfixDirectory + "Environment/");
            }
            if (!File.Exists(hotfixEnvironmentPath))
            {
                CreateHotfixEnvironment(hotfixEnvironmentPath);
            }
            if (!File.Exists(hotfixAssemblyDefinitionPath))
            {
                CreateILHotfixAssemblyDefinition(hotfixAssemblyDefinitionPath);
            }
        }
        private void SetILHotfixAssemblyDefinition(string filePath)
        {
            string contentOld = File.ReadAllText(filePath);
            JsonData json = JsonToolkit.StringToJson(contentOld);
            json["name"] = "ILHotfix";
            json["includePlatforms"] = new JsonData();
            json["includePlatforms"].Add("Editor");
            json["references"] = new JsonData();
            json["references"].Add("HTFramework.RunTime");
            json["references"].Add("HTFramework.ILHotfix.RunTime");
            json["autoReferenced"] = false;
            string contentNew = JsonToolkit.JsonToString(json);

            if (contentOld != contentNew)
            {
                File.WriteAllText(filePath, contentNew);
                AssetDatabase.Refresh();
                AssemblyDefinitionImporter importer = AssetImporter.GetAtPath("Assets" + _ILHotfixAssemblyDefinitionPath) as AssemblyDefinitionImporter;
                importer.SaveAndReimport();
            }
        }
        private void CreateILHotfixAssemblyDefinition(string filePath)
        {
            JsonData json = new JsonData();
            json["name"] = "ILHotfix";
            json["includePlatforms"] = new JsonData();
            json["includePlatforms"].Add("Editor");
            json["references"] = new JsonData();
            json["references"].Add("HTFramework.RunTime");
            json["references"].Add("HTFramework.ILHotfix.RunTime");
            json["autoReferenced"] = false;

            File.WriteAllText(filePath, JsonToolkit.JsonToString(json));
            AssetDatabase.Refresh();
            AssemblyDefinitionImporter importer = AssetImporter.GetAtPath("Assets" + _ILHotfixAssemblyDefinitionPath) as AssemblyDefinitionImporter;
            importer.SaveAndReimport();
        }
        private void CreateHotfixEnvironment(string filePath)
        {
            TextAsset asset = AssetDatabase.LoadAssetAtPath(EditorPrefsTableILHotfix.ScriptTemplateFolder + "ILHotfixEnvironmentTemplate.txt", typeof(TextAsset)) as TextAsset;
            if (asset)
            {
                string code = asset.text;
                File.AppendAllText(filePath, code, Encoding.UTF8);
                asset = null;
                AssetDatabase.Refresh();
            }
        }
    }
}