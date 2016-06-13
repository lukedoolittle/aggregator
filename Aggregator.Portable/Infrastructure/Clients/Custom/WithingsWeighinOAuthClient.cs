using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;
using Aggregator.Infrastructure.Requests;

namespace Aggregator.Infrastructure.Clients
{
    public class WithingsWeighinOAuthClient : OAuthClient<WithingsWeighin>
    {
        public WithingsWeighinOAuthClient(IOAuth oauthRequest) :
            base(oauthRequest)
        {
        }

        protected override IEnumerable<Tuple<DateTimeOffset, JObject>> ExtractDataPoints(
            JToken payload,
            TimestampOptions options)
        {
            var datapoints = new List<Tuple<DateTimeOffset, JObject>>();

            foreach (var group in payload)
            {
                var timestamp = group.ExtractTimestamp(
                    options.TimestampProperty,
                    options.TimestampFormat,
                    options.TimestampOffsetProperty,
                    options.TimestampOffset);

                var item = new JObject
                {
                    [options.TimestampProperty] = timestamp
                };

                foreach (var measure in group["measures"])
                {
                    var rawValue = Convert.ToInt32(measure["value"].ToString());
                    var exponent = Convert.ToInt32(measure["unit"].ToString());
                    var value = rawValue^exponent;
                    string key = string.Empty;

                    switch (measure["type"].ToString())
                    {
                        case "1":
                            key = "Weight(kg)";
                            break;
                        case "4":
                            key = "Height(meter)";
                            break;
                        case "5":
                            key = "Fat Free Mass(kg)";
                            break;
                        case "6":
                            key = "Fat Ratio(%)";
                            break;
                        case "8":
                            key = "Fat Mass Weight(kg)";
                            break;
                        case "9":
                            key = "Diastolic Blood Pressure(mmHg)";
                            break;
                        case "10":
                            key = "Systolic Blood Pressure(mmHg)";
                            break;
                        case "11":
                            key = "Heart Pulse(bpm)";
                            break;
                        case "54":
                            key = "SP02(%)";
                            break;
                        default:
                            key = "Unknown Measure";
                            break;
                    }

                    item[key] = value;
                }

                datapoints.Add(new Tuple<DateTimeOffset, JObject>(timestamp, item));
            }

            return datapoints;
        }
        //unit - Power of ten the "value" parameter should be multiplied to to get the real value. Eg : value = 20 and unit=-1 means the value really is 2.0
        //type - Measure type filter.Value is a number, which corresponds to : 
        //1 : Weight(kg)
        //4 : Height(meter)
        //5 : Fat Free Mass(kg)
        //6 : Fat Ratio(%)
        //8 : Fat Mass Weight(kg)
        //9 : Diastolic Blood Pressure(mmHg)
        //10 : Systolic Blood Pressure(mmHg)
        //11 : Heart Pulse(bpm)
        //54 : SP02(%)
        //
        //
        //EXAMPLE
        //
        //     {
        //         "status": 0,
        //    "body": {
        //             "updatetime": 1249409679,
        //        "timezone": "Europe/Paris",
        //        "measuregrps": [
        //            {
        //                "grpid": 2909,
        //                "attrib": 0,
        //                "date": 1222930968,
        //                "category": 1,
        //                "measures": [
        //                    {
        //                        "value": 79300,
        //                        "type": 1,
        //                        "unit": -3
        //                    },
        //                    {
        //                        "value": 652,
        //                        "type": 5,
        //                        "unit": -1
        //                    },
        //                    {
        //                        "value": 178,
        //                        "type": 6,
        //                        "unit": -1
        //                    },
        //                    {
        //                        "value": 14125,
        //                        "type": 8,
        //                        "unit": -3
        //                    }
        //                ]
        //            },
        //            {
        //                "grpid": 2908,
        //                "attrib": 0,
        //                "date": 1222930968,
        //                "category": 1,
        //                "measures": [
        //                    {
        //                        "value": 173,
        //                        "type": 4,
        //                        "unit": -2
        //                    }
        //                ]
        //            }
        //        ]
        //    }
        //}
    }
}
