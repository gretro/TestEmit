using Newtonsoft.Json;
using System;
using System.Dynamic;

namespace TestEmit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var a = new Test()
            {
                PropString = "Test",
                PropInt = 1,
                PropIntNullable = null,
                PropBool = false,
                PropBoolNullable = null,
                PropDouble = 2.0,
                PropDoubleNullable = null,
                PropFloat = 3.0f,
                PropFloatNullable = null,
                PropGuid = Guid.Empty,
                PropGuidNullable = null,
                PropObj = new Test()
                {
                    PropString = "SubTest",
                    PropInt = 1
                }
            };

            var emitter = new ProxyEmitter();
            var proxyA = emitter.EmitProxy<ITestMetadata>(a);

            //var proxyA = new TestProxy(a);

            proxyA.PropString = "Allo";
            Console.WriteLine(proxyA.PropString);

            proxyA.PropInt = 1;
            Console.WriteLine(proxyA.PropInt);

            proxyA.PropIntNullable = null;
            Console.WriteLine(proxyA.PropIntNullable);
            proxyA.PropIntNullable = 2;
            Console.WriteLine(proxyA.PropIntNullable);

            proxyA.PropBool = true;
            Console.WriteLine(proxyA.PropBool);

            proxyA.PropBoolNullable = null;
            Console.WriteLine(proxyA.PropBoolNullable);
            proxyA.PropBoolNullable = true;
            Console.WriteLine(proxyA.PropBoolNullable);

            proxyA.PropDouble = 3.0;
            Console.WriteLine(proxyA.PropDouble);

            proxyA.PropDoubleNullable = null;
            Console.WriteLine(proxyA.PropDoubleNullable);
            proxyA.PropDoubleNullable = 4.0;
            Console.WriteLine(proxyA.PropDoubleNullable);

            proxyA.PropFloat = 5.0f;
            Console.WriteLine(proxyA.PropFloat);

            proxyA.PropFloatNullable = null;
            Console.WriteLine(proxyA.PropFloatNullable);
            proxyA.PropFloatNullable = 6.0f;
            Console.WriteLine(proxyA.PropFloatNullable);

            proxyA.PropGuid = Guid.NewGuid();
            Console.WriteLine(proxyA.PropGuid);

            proxyA.PropGuidNullable = null;
            Console.WriteLine(proxyA.PropGuidNullable);
            proxyA.PropGuidNullable = Guid.NewGuid();
            Console.WriteLine(proxyA.PropGuidNullable);

            Console.WriteLine(JsonConvert.SerializeObject(proxyA.PropObj));
            proxyA.PropObj = null;
            Console.WriteLine(JsonConvert.SerializeObject(proxyA.PropObj));
            proxyA.PropObj = new Test()
            {
                PropString = "Sub Test 2",
                PropInt = 7
            };
            Console.WriteLine(JsonConvert.SerializeObject(proxyA.PropObj));

            //dynamic expando = new ExpandoObject();
            //expando.PropertyA = "Test Dynamic";
            //expando.PropertyB = 42;

            //var proxyB = new TestProxy(expando);
            //Console.WriteLine(proxyB.PropertyA);
            //Console.WriteLine(proxyB.PropertyB);
            Console.ReadKey();
        }
    }
}
