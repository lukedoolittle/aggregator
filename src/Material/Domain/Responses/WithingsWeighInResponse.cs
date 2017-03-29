using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class WithingsMeasure
    {
        [DataMember(Name = "value")]
        public int Value { get; set; }
        [DataMember(Name = "type")]
        public int Type { get; set; }
        [DataMember(Name = "unit")]
        public int Unit { get; set; }

        public double ReadingValue => Value * Math.Pow(10, Unit);

        public string ReadingType
        {
            get
            {
                switch (Type)
                {
                    case 1:
                        return "Weight(kg)";
                    case 4:
                        return "Height(meter)";
                    case 5:
                        return  "Fat Free Mass(kg)";
                    case 6:
                        return "Fat Ratio(%)";
                    case 8:
                        return "Fat Mass Weight(kg)";
                    case 9:
                        return "Diastolic Blood Pressure(mmHg)";
                    case 10:
                        return "Systolic Blood Pressure(mmHg)";
                    case 11:
                        return "Heart Pulse(bpm)";
                    case 54:
                        return "SP02(%)";
                    default:
                        return "Unknown Measure";
                }
            }
        }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class WithingsMeasureGroup
    {
        [DataMember(Name = "grpid")]
        public int Grpid { get; set; }
        [DataMember(Name = "attrib")]
        public int Attrib { get; set; }
        [DataMember(Name = "date")]
        public int Date { get; set; }
        [DataMember(Name = "category")]
        public int Category { get; set; }
        [DataMember(Name = "measures")]
        public IList<WithingsMeasure> Measures { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class WithingsBody
    {
        [DataMember(Name = "updatetime")]
        public int Updatetime { get; set; }
        [DataMember(Name = "measuregrps")]
        public IList<WithingsMeasureGroup> Measuregrps { get; set; }
        [DataMember(Name = "timezone")]
        public string Timezone { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class WithingsWeighInResponse
    {
        [DataMember(Name = "status")]
        public int Status { get; set; }
        [DataMember(Name = "body")]
        public WithingsBody Body { get; set; }
    }
}
