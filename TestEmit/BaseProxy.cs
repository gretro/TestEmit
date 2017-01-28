using System;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace TestEmit
{
    public abstract class BaseProxy
    {
        private readonly dynamic _target;

        public BaseProxy(object target)
        {
            _target = target;
        }

        protected object GetValue(string field)
        {
            var binder = Binder.GetMember(CSharpBinderFlags.None, field, _target.GetType(),
                new[]
                {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                });

            var site = CallSite<Func<CallSite, object, object>>.Create(binder);

            var value = site.Target(site, _target);
            return value;
        }

        protected void SetValue(string field, object value)
        {
            var binder = Binder.SetMember(CSharpBinderFlags.None, field, _target.GetType(),
                new []
                {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                });

            var site = CallSite<Func<CallSite, object, object, object>>.Create(binder);

            site.Target(site, _target, value);
        }
    }
}
