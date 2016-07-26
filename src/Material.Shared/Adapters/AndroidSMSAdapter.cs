#if __ANDROID__
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Provider;
using Android.Runtime;
using Android.Telephony;
using Newtonsoft.Json.Linq;
using Android.App;
using Material.Contracts;
using Object = Java.Lang.Object;
using Material.Framework;

namespace Material.Adapters
{
    using System.Threading.Tasks;

    //[BroadcastReceiver(Enabled = true)]
    //[Android.App.IntentFilter(new[] {"android.provider.Telephony.SMS_RECEIVED"})]
    public class AndroidSMSAdapter : ISMSAdapter //BroadcastReceiver,
    {
        //public Action<IEnumerable<Tuple<DateTimeOffset, JObject>>> Handler { get; set; }

        public async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetAllSMS(string filterDate)
        {
            var date = string.IsNullOrEmpty(filterDate) ? 0 : Convert.ToInt64(filterDate);

            var inboxMessages = await ContentManager.GetSMSInboxMessages(date).ConfigureAwait(false);
            var sentMessages = await ContentManager.GetSMSSentMessages(date).ConfigureAwait(false);

            //TODO: this needs to parse the datetime correctly
            return inboxMessages.Concat(sentMessages).Select(
                text => new Tuple<DateTimeOffset, JObject>(
                    DateTimeOffset.Parse(text[Telephony.Sms.Inbox.InterfaceConsts.Date].ToString()) ,
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

    public static class ContentManager
    {
        private static Android.Net.Uri SMS_INBOX_URI = Android.Net.Uri.Parse("content://sms/inbox");
        private static Android.Net.Uri SMS_SENT_URI = Android.Net.Uri.Parse("content://sms/sent");

        private static Task<IEnumerable<JObject>> GetMessages(
            long filterDate,
            Android.Net.Uri uri,
            string date,
            string address,
            string subject,
            string body,
            string dateSent,
            string creator)
        {
            var taskCompletionSource = new TaskCompletionSource<IEnumerable<JObject>>();
            var addresses = new Dictionary<long, string>();

            Platform.RunOnMainThread(() =>
            {
                var cursor = Application
                    .Context
                    .ApplicationContext
                    .ContentResolver
                    .Query(uri, null, null, null, null);

                var textMessages = new List<JObject>();

                if (cursor.MoveToFirst())
                {
                    do
                    {
                        var messageDate = cursor.GetLong(cursor.GetColumnIndex(date));

                        if (messageDate > filterDate)
                        {
                            var message = new JObject
                            {
                                [date] = messageDate,
                                [subject] = cursor.GetString(cursor.GetColumnIndex(subject)),
                                [body] = cursor.GetString(cursor.GetColumnIndex(body)),
                                [dateSent] = cursor.GetString(cursor.GetColumnIndex(dateSent)),
                                [creator] = cursor.GetString(cursor.GetColumnIndex(creator))
                            };

                            var messageAddress = cursor.GetLong(cursor.GetColumnIndex(address));

                            if (!addresses.ContainsKey(messageAddress))
                            {
                                addresses[messageAddress] = GetContactInfoFromAddress(messageAddress);
                            }

                            message[address] = addresses[messageAddress];
                            textMessages.Add(message);
                        }
                    } while (cursor.MoveToNext());
                }

                taskCompletionSource.SetResult(textMessages);
            });

            return taskCompletionSource.Task;
        }

        public static Task<IEnumerable<JObject>> GetSMSInboxMessages(long filterDate)
        {
            return GetMessages(
                filterDate,
                SMS_INBOX_URI,
                Telephony.Sms.Inbox.InterfaceConsts.Date,
                Telephony.Sms.Inbox.InterfaceConsts.Address,
                Telephony.Sms.Inbox.InterfaceConsts.Subject,
                Telephony.Sms.Inbox.InterfaceConsts.Body,
                Telephony.Sms.Inbox.InterfaceConsts.DateSent,
                Telephony.Sms.Inbox.InterfaceConsts.Creator);
        }

        public static Task<IEnumerable<JObject>> GetSMSSentMessages(long filterDate)
        {
            return GetMessages(
                filterDate,
                SMS_SENT_URI,
                Telephony.Sms.Sent.InterfaceConsts.Date,
                Telephony.Sms.Sent.InterfaceConsts.Address,
                Telephony.Sms.Sent.InterfaceConsts.Subject,
                Telephony.Sms.Sent.InterfaceConsts.Body,
                Telephony.Sms.Sent.InterfaceConsts.DateSent,
                Telephony.Sms.Sent.InterfaceConsts.Creator);
        }

        public static string GetContactInfoFromAddress(long address)
        {
            var contactCursor = Application
                .Context
                .ApplicationContext
                .ContentResolver
                .Query(
                    ContactsContract.CommonDataKinds.Phone.ContentUri,
                    null,
                    ContactsContract.CommonDataKinds.Phone.Number + "='" + address + "'",
                    null,
                    null);

            if (contactCursor.MoveToFirst())
            {
                string name;

                do
                {
                    var contactId = contactCursor.GetString(
                        contactCursor.GetColumnIndex(ContactsContract.RawContacts.InterfaceConsts.ContactId));

                    var emailAddress = GetEmailFromContactId(contactId);

                    name = contactCursor.GetString(
                        contactCursor.GetColumnIndex(
                            ContactsContract.Contacts.InterfaceConsts.DisplayName));

                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        return $"{name} <{emailAddress}>";
                    }
                } while (contactCursor.MoveToNext());

                return name;
            }
            else
            {
                return address.ToString();
            }
        }

        public static string GetEmailFromContactId(string contactId)
        {
            var emailCursor = Application
                .Context
                .ApplicationContext
                .ContentResolver
                .Query(
                    ContactsContract.CommonDataKinds.Email.ContentUri,
                    null,
                    ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId + "=" + contactId,
                    null,
                    null);

            var emailAddress = "";

            while (emailCursor.MoveToNext())
            {
                var email = emailCursor.GetString(
                    emailCursor.GetColumnIndex(
                        ContactsContract.CommonDataKinds.Email.Address));

                if (!string.IsNullOrEmpty(email))
                {
                    emailAddress = emailAddress + email + ", ";
                }
            }

            return emailAddress.Trim().Trim(',');
        }
    }
}
#endif