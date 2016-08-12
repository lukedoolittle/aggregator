using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class WithingsMeasure
    {
        [DataMember(Name = "value")]
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }
        [DataMember(Name = "unit")]
        [JsonProperty(PropertyName = "unit")]
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

    [DataContract]
    public class Measuregrp
    {
        [DataMember(Name = "grpid")]
        [JsonProperty(PropertyName = "grpid")]
        public int Grpid { get; set; }
        [DataMember(Name = "attrib")]
        [JsonProperty(PropertyName = "attrib")]
        public int Attrib { get; set; }
        [DataMember(Name = "date")]
        [JsonProperty(PropertyName = "date")]
        public int Date { get; set; }
        [DataMember(Name = "category")]
        [JsonProperty(PropertyName = "category")]
        public int Category { get; set; }
        [DataMember(Name = "measures")]
        [JsonProperty(PropertyName = "measures")]
        public IList<WithingsMeasure> Measures { get; set; }
    }

    [DataContract]
    public class WithingsBody
    {
        [DataMember(Name = "updatetime")]
        [JsonProperty(PropertyName = "updatetime")]
        public int Updatetime { get; set; }
        [DataMember(Name = "measuregrps")]
        [JsonProperty(PropertyName = "measuregrps")]
        public IList<Measuregrp> Measuregrps { get; set; }
        [DataMember(Name = "timezone")]
        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }
    }

    [DataContract]
    public class WithingsWeighInResponse
    {
        [DataMember(Name = "status")]
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        [DataMember(Name = "body")]
        [JsonProperty(PropertyName = "body")]
        public WithingsBody Body { get; set; }
    }


}
