using System;
using System.Reflection;
using System.Reflection.Emit;

namespace TestEmit
{
    public class ProxyEmitter
    {
        public TInterface EmitProxy<TInterface>(object target)
        {
            var interfaceType = typeof (TInterface);

            var assembly = GetAssemblyBuilder("TestEmit.Proxy.Emit");
            var module = GetModuleBuilder(assembly);
            var proxyBuilder = GetProxyTypeBuilder(module, "ProxyOf" + interfaceType.Name);

            proxyBuilder.AddInterfaceImplementation(interfaceType);

            BuildCtor(proxyBuilder);
            BuildProperties(proxyBuilder, interfaceType);

            var proxyType = proxyBuilder.CreateType();

            assembly.Save("DynamicProxies.dll");

            var instance = Activator.CreateInstance(proxyType, target);
            return (TInterface)instance;
        }

        private static void BuildCtor(TypeBuilder proxyBuilder)
        {
            var ctorBuilder =
                proxyBuilder.DefineConstructor(
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                    MethodAttributes.RTSpecialName,
                    CallingConventions.Standard, new[] {typeof (object)});

            
            var objectCtor = typeof (BaseProxy).GetConstructor(new[] { typeof (Object) });

            ILGenerator cil = ctorBuilder.GetILGenerator();
            cil.Emit(OpCodes.Ldarg_0);
            cil.Emit(OpCodes.Ldarg_1);
            cil.Emit(OpCodes.Call, objectCtor);
            cil.Emit(OpCodes.Nop);
            cil.Emit(OpCodes.Nop);
            cil.Emit(OpCodes.Ret);
        }

        private static void BuildProperties(TypeBuilder proxyBuilder, Type interfaceType)
        {
            var baseType = typeof (BaseProxy);
            var properties = interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var getValueMethodInfo = baseType.GetMethod("GetValue", BindingFlags.Instance | BindingFlags.NonPublic);
            var setValueMethodInfo = baseType.GetMethod("SetValue", BindingFlags.Instance | BindingFlags.NonPublic);


            foreach (var propertyInfo in properties)
            {
                PropertyBuilder propertyBuilder =
                        proxyBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.HasDefault,
                            propertyInfo.PropertyType, null);

                const MethodAttributes attributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
                                                    MethodAttributes.SpecialName | MethodAttributes.Virtual | MethodAttributes.Final;

                if (propertyInfo.CanRead)
                {
                    MethodBuilder getterMethodBuilder = BuildGetter(proxyBuilder, getValueMethodInfo, propertyInfo, attributes);
                    propertyBuilder.SetGetMethod(getterMethodBuilder);
                }

                if (propertyInfo.CanWrite)
                {
                    MethodBuilder setterMethodBuilder = BuildSetter(proxyBuilder, setValueMethodInfo, propertyInfo, attributes);

                    propertyBuilder.SetSetMethod(setterMethodBuilder);
                }
            }
        }

        private static MethodBuilder BuildGetter(TypeBuilder proxyBuilder, MethodInfo getValueMethodInfo, PropertyInfo propertyInfo, MethodAttributes attributes)
        {
            var getterMethodBuilder = proxyBuilder.DefineMethod(String.Format("get_{0}", propertyInfo.Name),
                                    attributes, propertyInfo.PropertyType, Type.EmptyTypes);

            var cil = getterMethodBuilder.GetILGenerator();
            //TODO: Is this really necessary? It does not seem to do anything...
            var targetInstruction = cil.DefineLabel();

            cil.DeclareLocal(propertyInfo.PropertyType);
            cil.Emit(OpCodes.Nop);
            cil.Emit(OpCodes.Ldarg_0);
            cil.Emit(OpCodes.Ldstr, propertyInfo.Name);
            cil.Emit(OpCodes.Call, getValueMethodInfo);

            if (propertyInfo.PropertyType.IsByRef)
            {
                cil.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
            }
            else
            {
                cil.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
            }
            cil.Emit(OpCodes.Stloc_0);
            cil.Emit(OpCodes.Br_S, targetInstruction);
            cil.MarkLabel(targetInstruction);
            cil.Emit(OpCodes.Ldloc_0);
            cil.Emit(OpCodes.Ret);
            return getterMethodBuilder;
        }

        private static MethodBuilder BuildSetter(TypeBuilder proxyBuilder, MethodInfo setValueMethodInfo, PropertyInfo propertyInfo, MethodAttributes attributes)
        {
            var setterMethodBuilder = proxyBuilder.DefineMethod(String.Format("set_{0}", propertyInfo.Name),
                                    attributes, null, new[] { propertyInfo.PropertyType });

            var cil = setterMethodBuilder.GetILGenerator();
            cil.Emit(OpCodes.Nop);
            cil.Emit(OpCodes.Ldarg_0);
            cil.Emit(OpCodes.Ldstr, propertyInfo.Name);
            cil.Emit(OpCodes.Ldarg_1);

            if (!propertyInfo.PropertyType.IsByRef)
            {
                cil.Emit(OpCodes.Box, propertyInfo.PropertyType);
            }

            cil.Emit(OpCodes.Call, setValueMethodInfo);
            cil.Emit(OpCodes.Nop);
            cil.Emit(OpCodes.Ret);
            return setterMethodBuilder;
        }

        public void Test(TypeBuilder tb)
        {
            PropertyBuilder pbNewProp = tb.DefineProperty("NewProp", PropertyAttributes.HasDefault, typeof(int), null);
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define the "get" accessor method
            MethodBuilder mbNewPropGetAccessor = tb.DefineMethod(
                "get_NewProp",
                getSetAttr,
                typeof(int),
                Type.EmptyTypes);

            ILGenerator NewPropGetIL = mbNewPropGetAccessor.GetILGenerator();
            NewPropGetIL.Emit(OpCodes.Ldarg_0);
            NewPropGetIL.Emit(OpCodes.Ldc_I4_1);
            NewPropGetIL.Emit(OpCodes.Call, typeof(Test).GetMethod("GetVal"));
            NewPropGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method 
            MethodBuilder mbNewPropSetAccessor = tb.DefineMethod(
                "set_NewProp",
                getSetAttr,
                null,
                new Type[] { typeof(int) });

            ILGenerator NewPropSetIL = mbNewPropSetAccessor.GetILGenerator();
            NewPropSetIL.Emit(OpCodes.Ldarg_0);
            NewPropSetIL.Emit(OpCodes.Ldc_I4_1);
            NewPropSetIL.Emit(OpCodes.Ldarg_1);
            NewPropSetIL.Emit(OpCodes.Call, typeof(Test).GetMethod("SetVal"));
            NewPropSetIL.Emit(OpCodes.Ret);

            // Map the accessor methods
            pbNewProp.SetGetMethod(mbNewPropGetAccessor);
            pbNewProp.SetSetMethod(mbNewPropSetAccessor);
        }

        private AssemblyBuilder GetAssemblyBuilder(string assemblyName)
        {
            var assName = new AssemblyName(assemblyName);
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assName, AssemblyBuilderAccess.RunAndSave);

            return assemblyBuilder;
        }

        private ModuleBuilder GetModuleBuilder(AssemblyBuilder assemblyBuilder)
        {
            var builder = assemblyBuilder.DefineDynamicModule("Proxy.Emit", "DynamicProxies.dll");
            return builder;
        }

        private TypeBuilder GetProxyTypeBuilder(ModuleBuilder module, string typeName)
        {
            var builder = module.DefineType(typeName,
                TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit, typeof(BaseProxy));

            return builder;
        }
    }
}
