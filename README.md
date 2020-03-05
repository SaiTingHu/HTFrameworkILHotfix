# Unity HTFramework ILHotfix

基于ILRuntime的Unity跨平台热更新模块，必须依赖于HTFramework主框架使用。

## 环境

- Unity版本：2018.3.0及以上。

- .NET API版本：4.x。

- [ILRuntime](https://github.com/Ourpalm/ILRuntime)。

- [HTFramework](https://github.com/SaiTingHu/HTFramework)。

## 模块简介

- [ILHotfix](https://wanderer.blog.csdn.net/article/details/96152656) - 基于ILRuntime实现的跨平台热更新框架，开发非常方便，新项目只需要拉取框架源码及本模块，一键即可创建热更新环境，之后便可以正常开发。

## 使用方法

- 1.拉取框架到项目中的Assets文件夹下（Assets/HTFramework/）。

- 2.将框架根目录下的HTFramework.prefab拖到主场景，并删除主场景其他的东西（除了框架的主要模块，其他任何东西都应该是动态加载的）。

- 3.拉取本模块到项目中的Assets文件夹下（Assets/HTFrameworkILHotfix/）。

- 4.将HTFrameworkILHotfix目录下的HTFrameworkILHotfix.prefab拖到主场景，并一键创建热更新环境，新建热更新入口流程及其他流程。

- 5.开始开发。
