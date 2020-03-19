using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace HT.Framework.ILHotfix
{
    public sealed class ILHotfixProcedureAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(ILHotfixProcedureBase);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adaptor);
            }
        }
        
        public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adaptor(appdomain, instance);
        }

        public sealed class Adaptor : ILHotfixProcedureBase, CrossBindingAdaptorType
        {
            private AppDomain _appdomain;
            private ILTypeInstance _instance;
            private IMethod _onInit;
            private IMethod _onEnter;
            private IMethod _onLeave;
            private IMethod _onUpdate;
            private IMethod _onUpdateSecond;
            private bool _onInitGot = false;
            private bool _onEnterGot = false;
            private bool _onLeaveGot = false;
            private bool _onUpdateGot = false;
            private bool _onUpdateSecondGot = false;

            public ILTypeInstance ILInstance
            {
                get
                {
                    return _instance;
                }
            }

            public Adaptor()
            {

            }

            public Adaptor(AppDomain appdomain, ILTypeInstance instance)
            {
                _appdomain = appdomain;
                _instance = instance;
            }
            
            public override void OnInit()
            {
                if (!_onInitGot)
                {
                    _onInit = _instance.Type.GetMethod("OnInit", 0);
                    _onInitGot = true;
                }
                if (_onInit != null)
                {
                    _appdomain.Invoke(_onInit, _instance, null);
                }
            }
            
            public override void OnEnter()
            {
                if (!_onEnterGot)
                {
                    _onEnter = _instance.Type.GetMethod("OnEnter", 0);
                    _onEnterGot = true;
                }
                if (_onEnter != null)
                {
                    _appdomain.Invoke(_onEnter, _instance, null);
                }
            }
            
            public override void OnLeave()
            {
                if (!_onLeaveGot)
                {
                    _onLeave = _instance.Type.GetMethod("OnLeave", 0);
                    _onLeaveGot = true;
                }
                if (_onLeave != null)
                {
                    _appdomain.Invoke(_onLeave, _instance, null);
                }
            }
            
            public override void OnUpdate()
            {
                if (!_onUpdateGot)
                {
                    _onUpdate = _instance.Type.GetMethod("OnUpdate", 0);
                    _onUpdateGot = true;
                }
                if (_onUpdate != null)
                {
                    _appdomain.Invoke(_onUpdate, _instance, null);
                }
            }
            
            public override void OnUpdateSecond()
            {
                if (!_onUpdateSecondGot)
                {
                    _onUpdateSecond = _instance.Type.GetMethod("OnUpdateSecond", 0);
                    _onUpdateSecondGot = true;
                }
                if (_onUpdateSecond != null)
                {
                    _appdomain.Invoke(_onUpdateSecond, _instance, null);
                }
            }
        }
    }
}
