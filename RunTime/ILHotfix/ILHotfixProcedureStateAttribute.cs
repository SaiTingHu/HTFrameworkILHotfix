using System;

namespace HT.Framework.ILHotfix
{
    /// <summary>
    /// IL热更新流程状态标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ILHotfixProcedureStateAttribute : Attribute
    {
        public ILHotfixProcedureState State;

        public ILHotfixProcedureStateAttribute(ILHotfixProcedureState state)
        {
            State = state;
        }
    }

    /// <summary>
    /// IL热更新流程状态
    /// </summary>
    public enum ILHotfixProcedureState
    {
        /// <summary>
        /// 入口流程
        /// </summary>
        Entrance,
        /// <summary>
        /// 常规流程
        /// </summary>
        Normal,
        /// <summary>
        /// 禁用的流程
        /// </summary>
        Disabled
    }
}
