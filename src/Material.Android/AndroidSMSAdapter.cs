using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Provider;
using Android.Runtime;
using Android.Telephony;
using Android.App;
using Android.Database;
using Foundations.Extensions;
using Material.Contracts;
using Material.Framework;
using Material.Infrastructure.Requests;
using Org.BouncyCastle.Asn1;
using Uri = Android.Net.Uri;

namespace Material.Adapters
{
    using System.Threading.Tasks;

    //[BroadcastReceiver(Enabled = true)]
    //[Android.App.IntentFilter(new[] {"android.provider.Telephony.SMS_RECEIVED"})]
    public class AndroidSMSAdapter : ISMSAdapter //BroadcastReceiver,
    {
        //public Action<IEnumerable<Tuple<DateTimeOffset, JObject>>> Handler { get; set; }

        public async Task<IEnumerable<SMSMessage>> GetAllSMS(
            DateTime filterDate)
        {
            long dateTimeInSeconds = 0;
            if (filterDate != default(DateTime))
            {
                dateTimeInSeconds = Convert.ToInt64(
                    filterDate.ToUnixTimeMilliseconds());
            }

            var inboxMessages = await GetMessages(
                    dateTimeInSeconds,
                    Uri.Parse("content://sms/inbox"),
                    Telephony.Sms.Inbox.InterfaceConsts.Date,
                    Telephony.Sms.Inbox.InterfaceConsts.Address,
                    Telephony.Sms.Inbox.InterfaceConsts.Subject,
                    Telephony.Sms.Inbox.InterfaceConsts.Body,
                    Telephony.Sms.Inbox.InterfaceConsts.DateSent,
                    Telephony.Sms.Inbox.InterfaceConsts.Creator)
                .ConfigureAwait(false);

            var sentMessages = await GetMessages(
                    dateTimeInSeconds,
                    Uri.Parse("content://sms/sent"),
                    Telephony.Sms.Sent.InterfaceConsts.Date,
                    Telephony.Sms.Sent.InterfaceConsts.Address,
                    Telephony.Sms.Sent.InterfaceConsts.Subject,
                    Telephony.Sms.Sent.InterfaceConsts.Body,
                    Telephony.Sms.Sent.InterfaceConsts.DateSent,
                    Telephony.Sms.Sent.InterfaceConsts.Creator)
                .ConfigureAwait(false);

            return inboxMessages.Concat(sentMessages);
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

        private Task<IEnumerable<SMSMessage>> GetMessages(
            long filterDate,
            Uri uri,
            string date,
            string address,
            string subject,
            string body,
            string dateSent,
            string creator)
        {
            var taskCompletionSource = 
                new TaskCompletionSource<IEnumerable<SMSMessage>>();
            var addresses = new Dictionary<long, string>();

            var filter = "date>=" + filterDate;

            Platform.Current.RunOnMainThread(() =>
            {
                var cursor = FetchCursor(uri, null, filter);

                var textMessages = new List<SMSMessage>();

                if (cursor.MoveToFirst())
                {
                    do
                    {
                        var messageDate = cursor.GetLong(cursor.GetColumnIndex(date));

                        var message = new SMSMessage
                        {
                            Date = messageDate,
                            Subject = cursor.GetString(cursor.GetColumnIndex(subject)),
                            Body = cursor.GetString(cursor.GetColumnIndex(body)),
                            DateSent = cursor.GetLong(cursor.GetColumnIndex(dateSent)),
                            Creator = cursor.GetString(cursor.GetColumnIndex(creator))
                        };

                        var messageAddress = cursor.GetLong(cursor.GetColumnIndex(address));

                        if (!addresses.ContainsKey(messageAddress))
                        {
                            addresses[messageAddress] = GetContactInfoFromAddress(messageAddress);
                        }

                        message.Address = addresses[messageAddress];
                        textMessages.Add(message);

                    } while (cursor.MoveToNext());
                }

                taskCompletionSource.SetResult(textMessages);
            });

            return taskCompletionSource.Task;
        }

        private string GetContactInfoFromAddress(long address)
        {
            var uri = Uri.WithAppendedPath(
                ContactsContract.PhoneLookup.ContentFilterUri,
                Uri.Encode(address.ToString()));

            string[] projection =
            {
                ContactsContract.Contacts.InterfaceConsts.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName
            };

            var contactCursor = FetchCursor(uri, projection, null);

            if (contactCursor.MoveToFirst())
            {
                string name;

                do
                {
                    var contactId = contactCursor.GetString(
                        contactCursor.GetColumnIndex(projection[0]));

                    var emailAddress = GetEmailFromContactId(contactId);

                    name = contactCursor.GetString(
                        contactCursor.GetColumnIndex(
                            projection[1]));

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

        private string GetEmailFromContactId(string contactId)
        {
            var uri = ContactsContract.CommonDataKinds.Email.ContentUri;
            var filter = ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId + " = " + contactId;
            var emailCursor = FetchCursor(uri, null, filter);

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

        private static ICursor FetchCursor(
            Uri uri, 
            string[] projection, 
            string filter)
        {
            return Application
                .Context
                .ApplicationContext
                .ContentResolver
                .Query(
                    uri,
                    projection,
                    filter,
                    null,
                    null);
        }
    }
}