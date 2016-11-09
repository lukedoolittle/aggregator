using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;

namespace Material.Permissions
{
    public class DeviceAuthorizationFacade
    {
        public async Task<bool> Authorize(
            List<string> permissions, 
            int requestCode,
            Activity currentActivity)
        {
            var activityCompletionSource = new TaskCompletionSource<PermissionsActivity>();
            var context = currentActivity;

            var intent = new Intent(context, typeof(PermissionsActivity));
            intent.PutExtra(
                PermissionsActivity.Key,
                PermissionsActivity.StateRepo.Add(activityCompletionSource));
            context.StartActivity(intent);

            var activity = await activityCompletionSource
                .Task
                .ConfigureAwait(false);

            var result = await activity
                .Grant(permissions, requestCode)
                .ConfigureAwait(false);

            activity.Finish();

            return result;
        }
    }
}