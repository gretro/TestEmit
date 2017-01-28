using System;

namespace TestEmit
{
    public class Test : ITestMetadata
    {
        public string PropString { get; set; }

        public int PropInt { get; set; }

        public int? PropIntNullable
        {
            get; set;
        }

        public float PropFloat
        {
            get;

            set;
        }

        public float? PropFloatNullable
        {
            get;

            set;
        }

        public double PropDouble
        {
            get;

            set;
        }

        public double? PropDoubleNullable
        {
            get;

            set;
        }

        public bool PropBool
        {
            get;

            set;
        }

        public bool? PropBoolNullable
        {
            get;

            set;
        }

        public Guid PropGuid
        {
            get;

            set;
        }

        public Guid? PropGuidNullable
        {
            get;

            set;
        }

        public Test PropObj
        {
            get;

            set;
        }
    }
}
