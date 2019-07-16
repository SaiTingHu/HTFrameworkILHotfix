﻿using ILRuntime.Runtime.Generated;
using Mono.Cecil.Pdb;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace HT.Framework.ILHotfix
{
    /// <summary>
    /// IL热更新模块管理者
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ILHotfixManager : MonoBehaviour
    {
        public static ILHotfixManager Current;

        /// <summary>
        /// 自动启动
        /// </summary>
        public bool IsAutoStartUp = false;
        /// <summary>
        /// 热更新库文件AB包名称
        /// </summary>
        public string ILHotfixDllAssetBundleName = "ilhotfix";
        /// <summary>
        /// 热更新库文件路径
        /// </summary>
        public string ILHotfixDllAssetsPath = "Assets/ILHotfix/ILHotfix.dll.bytes";
        /// <summary>
        /// 执行热更新逻辑事件
        /// </summary>
        public event Action UpdateILHotfixLogicEvent;

        private bool _isStartUp = false;
        private TextAsset _ILHotfixDll;
        private AppDomain _appDomain;
        private List<Type> _ILHotFixTypes = new List<Type>();
        private object _ILHotfixEnvironment;

        private void Awake()
        {
            Current = this;

            if (Main.m_Resource.Mode == ResourceMode.Resource)
            {
                GlobalTools.LogError("热更新初始化失败：热更新库不支持使用Resource加载模式！");
                return;
            }
        }

        private void Start()
        {
            if (IsAutoStartUp)
            {
                StartUp();
            }
        }

        private void Update()
        {
            if (_isStartUp)
            {
                UpdateILHotfixLogicEvent?.Invoke();
            }
        }

        /// <summary>
        /// 热更新库中的所有类型
        /// </summary>
        public List<Type> ILHotFixTypes
        {
            get
            {
                if (_ILHotFixTypes.Count <= 0)
                {
                    if (_appDomain != null)
                    {
                        foreach (var item in _appDomain.LoadedTypes.Values)
                        {
                            _ILHotFixTypes.Add(item.ReflectionType);
                        }
                    }
                }
                return _ILHotFixTypes;
            }
        }

        /// <summary>
        /// 启动热更新
        /// </summary>
        public void StartUp()
        {
            if (!_isStartUp)
            {
                _isStartUp = true;
                AssetInfo dllInfo = new AssetInfo(ILHotfixDllAssetBundleName, ILHotfixDllAssetsPath, "");
                Main.m_Resource.LoadAsset<TextAsset>(dllInfo, null, ILHotfixDllLoadDone);
            }
        }

        private void ILHotfixDllLoadDone(TextAsset asset)
        {
            _ILHotfixDll = asset;
            
            if (_ILHotfixDll != null)
            {
                _isStartUp = true;
                _appDomain = new AppDomain();
                MemoryStream dllStream = new MemoryStream(_ILHotfixDll.bytes);
                _appDomain.LoadAssembly(dllStream, null, new PdbReaderProvider());

                ILHotfixInitialize();

                _ILHotfixEnvironment = _appDomain.Instantiate("ILHotfixEnvironment");

                if (_ILHotfixEnvironment == null)
                {
                    _isStartUp = false;
                    GlobalTools.LogError("热更新初始化失败：热更新库中不存在热更新环境 ILHotfixEnvironment！");
                }

                dllStream.Dispose();
            }
            else
            {
                _isStartUp = false;
                GlobalTools.LogError("热更新初始化失败：未正确加载热更新库文件！");
            }
        }

        private void ILHotfixInitialize()
        {
            CLRBindings.Initialize(_appDomain);

            _appDomain.RegisterCrossBindingAdaptor(new ILHotfixProcedureAdapter());
        }
    }
}