using ILRuntime.Runtime.CLRBinding;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[Obfuscation(Exclude = true)]
public class ILRuntimeCLRBinding
{
    [MenuItem("HTFramework.ILHotfix/ILRuntime/Generate CLR Binding Code")]
    private static void GenerateCLRBinding()
    {
        List<Type> types = new List<Type>();
        types.Add(typeof(int));
        types.Add(typeof(float));
        types.Add(typeof(long));
        types.Add(typeof(object));
        types.Add(typeof(string));
        types.Add(typeof(Array));
        types.Add(typeof(Vector2));
        types.Add(typeof(Vector3));
        types.Add(typeof(Quaternion));
        types.Add(typeof(GameObject));
        types.Add(typeof(UnityEngine.Object));
        types.Add(typeof(Transform));
        types.Add(typeof(RectTransform));
        types.Add(typeof(Time));
        types.Add(typeof(Debug));
        types.Add(typeof(List<ILTypeInstance>));

        BindingCodeGenerator.GenerateBindingCode(types, "Assets/HTFrameworkILHotfix/RunTime/ILRuntime/Generated");
        AssetDatabase.Refresh();
    }
}