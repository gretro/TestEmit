using System;

namespace TestEmit
{
    public interface ITestMetadata
    {
        int PropInt { get; set; }

        int? PropIntNullable { get; set; }

        float PropFloat { get; set; }

        float? PropFloatNullable { get; set; }

        double PropDouble { get; set; }

        double? PropDoubleNullable { get; set; }

        bool PropBool { get; set; }

        bool? PropBoolNullable { get; set; }

        Guid PropGuid { get; set; }

        Guid? PropGuidNullable { get; set; }

        string PropString { get; set; }

        Test PropObj { get; set; }
    }
}
