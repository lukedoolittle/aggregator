using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Material.View;

namespace Material
{
    public delegate void RequestPermissionsResultEventHandler(
        object sender, 
        RequestPermissionsResultEventArgs e);

    public class RequestPermissionsResultEventArgs : EventArgs
    {
        public int RequestCode { get; }
        public string[] Permissions { get; }
        public Permission[] GrantResults { get; }

        public RequestPermissionsResultEventArgs(
            int requestCode, 
            string[] permissions, 
            Permission[] grantResults)
        {
            RequestCode = requestCode;
            Permissions = permissions;
            GrantResults = grantResults;
        }
    }

    [Activity(Label = "PermissionsActivity")]
    public class PermissionsActivity : Activity
    {
        internal const string Key = "Permissions";
        internal static readonly ActivityStateRepository<TaskCompletionSource<PermissionsActivity>> StateRepo =
            new ActivityStateRepository<TaskCompletionSource<PermissionsActivity>>();

        public event RequestPermissionsResultEventHandler Permissions;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(bundle);

            var completionSource = StateRepo.Remove(
                Intent.GetStringExtra(Key));

            completionSource.SetResult(this);
        }

        protected virtual void OnChanged(
            RequestPermissionsResultEventArgs e)
        {
            Permissions?.Invoke(this, e);
        }

        public async Task<bool> Grant(
            List<string> permissions, 
            int requestCode)
        {
            var completionSource = new TaskCompletionSource<bool>();

            if ((int)Build.VERSION.SdkInt < 23)
            {
                return true;
            }
            else if (permissions.All(p => CheckSelfPermission(p) == (int)Permission.Granted))
            {
                return true;
            }
            else
            {
                RequestPermissions(
                    permissions.ToArray(), 
                    requestCode);

                Permissions += (sender, args) =>
                {
                    if (args.RequestCode == requestCode)
                    {
                        completionSource.SetResult(args.GrantResults[0] == Permission.Granted);
                    }
                };
            }

            return await completionSource.Task.ConfigureAwait(false);
        }

        public override void OnRequestPermissionsResult(
            int requestCode, 
            string[] permissions,
            Permission[] grantResults)
        {
            OnChanged(new RequestPermissionsResultEventArgs(
                requestCode, 
                permissions, 
                grantResults));
        }
    }
}