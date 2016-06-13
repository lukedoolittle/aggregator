using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Provider;
using Newtonsoft.Json.Linq;
using Aggregator.Framework;

namespace Aggregator.Infrastructure.ComponentManagers
{
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