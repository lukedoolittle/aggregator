using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Responses
{
    public class WithingsMeasure
    {
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }
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

    public class Measuregrp
    {
        [JsonProperty(PropertyName = "grpid")]
        public int Grpid { get; set; }
        [JsonProperty(PropertyName = "attrib")]
        public int Attrib { get; set; }
        [JsonProperty(PropertyName = "date")]
        public int Date { get; set; }
        [JsonProperty(PropertyName = "category")]
        public int Category { get; set; }
        [JsonProperty(PropertyName = "measures")]
        public IList<WithingsMeasure> Measures { get; set; }
    }

    public class WithingsBody
    {
        [JsonProperty(PropertyName = "updatetime")]
        public int Updatetime { get; set; }
        [JsonProperty(PropertyName = "measuregrps")]
        public IList<Measuregrp> Measuregrps { get; set; }
        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }
    }

    public class WithingsWeighInResponse
    {
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        [JsonProperty(PropertyName = "body")]
        public WithingsBody Body { get; set; }
    }


}
