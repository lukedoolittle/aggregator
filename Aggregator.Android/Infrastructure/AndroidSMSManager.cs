using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Provider;
using Android.Runtime;
using Android.Telephony;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;
using Object = Java.Lang.Object;

namespace Aggregator.Infrastructure.ComponentManagers
{
    using System.Threading.Tasks;

    //[BroadcastReceiver(Enabled = true)]
    //[Android.App.IntentFilter(new[] {"android.provider.Telephony.SMS_RECEIVED"})]
    public class AndroidSMSManager : ISMSManager //BroadcastReceiver,
    {
        //public Action<IEnumerable<Tuple<DateTimeOffset, JObject>>> Handler { get; set; }

        public async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetAllSMS(string filterDate)
        {
            var date = string.IsNullOrEmpty(filterDate) ? 0 : Convert.ToInt64(filterDate);

            var inboxMessages = await ContentManager.GetSMSInboxMessages(date).ConfigureAwait(false);
            var sentMessages = await ContentManager.GetSMSSentMessages(date).ConfigureAwait(false);

            return inboxMessages.Concat(sentMessages).Select(
                text => new Tuple<DateTimeOffset, JObject>(
                    text[Telephony.Sms.Inbox.InterfaceConsts.Date].ToDateTimeOffset(null, null),
                    text));
        }

        //public override void OnReceive(
        //    Context context, 
        //    Intent intent)
        //{
        //    var bundle = intent.Extras;
        //    if (bundle == null)
        //    {
        //        return;
        //    }

        //    var pdus = bundle.Get("pdus");
        //    var castedPdus = JNIEnv.GetArray<Java.Lang.Object>(pdus.Handle);

        //    var result = new List<Tuple<DateTimeOffset, JObject>>();

        //    foreach (Object pdu in castedPdus)
        //    {
        //        var bytes = new byte[JNIEnv.GetArrayLength(pdu.Handle)];
        //        JNIEnv.CopyArray(pdu.Handle, bytes);

        //        var message = SmsMessage.CreateFromPdu(bytes);

        //        var messageAsJson = new JObject
        //        {
        //            ["sender"] = message.OriginatingAddress,
        //            ["body"] = message.MessageBody,
        //            ["timestamp"] = message.TimestampMillis
        //        };
        //        var datetime = message.TimestampMillis.ToDateTime();
        //        result.Add(new Tuple<DateTimeOffset, JObject>(datetime, messageAsJson));
        //    }

        //    Handler?.Invoke(result);
        //}
    }
}