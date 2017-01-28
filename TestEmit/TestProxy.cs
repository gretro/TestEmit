using System;

namespace TestEmit
{
    public class TestProxy : BaseProxy, ITestMetadata
    {
        public TestProxy(object target)
            : base(target)
        {
        }

        public string PropString
        {
            get { return (string)GetValue("PropString"); }
            set { SetValue("PropString", value); }
        }

        public int PropInt
        {
            get { return (int)GetValue("PropInt"); }
            set { SetValue("PropInt", value); }
        }

        public int? PropIntNullable
        {
            get
            {
                return (int?)GetValue("PropIntNullable");
            }

            set
            {
                SetValue("PropIntNullable", value);
            }
        }

        public float PropFloat
        {
            get
            {
                return (float)GetValue("PropFloat");
            }

            set
            {
                SetValue("PropFloat", value);
            }
        }

        public float? PropFloatNullable
        {
            get
            {
                return (float?)GetValue("PropFloatNullable");
            }

            set
            {
                SetValue("PropFloatNullable", value);
            }
        }

        public double PropDouble
        {
            get
            {
                return (double)GetValue("PropDouble");
            }

            set
            {
                SetValue("PropDouble", value);
            }
        }

        public double? PropDoubleNullable
        {
            get
            {
                return (double?)GetValue("PropDoubleNullable");
            }

            set
            {
                SetValue("PropDoubleNullable", value);
            }
        }

        public bool PropBool
        {
            get
            {
                return (bool)GetValue("PropBool");
            }

            set
            {
                SetValue("PropBool", value);
            }
        }

        public bool? PropBoolNullable
        {
            get
            {
                return (bool?)GetValue("PropBoolNullable");
            }

            set
            {
                SetValue("PropBoolNullable", value);
            }
        }

        public Guid PropGuid
        {
            get
            {
                return (Guid)GetValue("PropGuid");
            }

            set
            {
                SetValue("PropGuid", value);
            }
        }

        public Guid? PropGuidNullable
        {
            get
            {
                return (Guid?)GetValue("PropGuidNullable");
            }

            set
            {
                SetValue("PropGuidNullable", value);
            }
        }

        public Test PropObj
        {
            get
            {
                return (Test)GetValue("PropObj");
            }

            set
            {
                SetValue("PropObj", value);
            }
        }
    }
}
