using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Aggregator.Framework.Contracts;

namespace Aggregator.View.BluetoothAuthorization
{
    [Activity(Label = "DeviceListActivity")]
    public class DeviceListActivity : ListActivity
    {
        internal static readonly ActivityStateRepository<TaskCompletionSource<DeviceListActivity>> StateRepo =
            new ActivityStateRepository<TaskCompletionSource<DeviceListActivity>>();

        private DeviceListAdapter _adapter;

        public Action<BluetoothDevice> DeviceSelected { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _adapter = new DeviceListAdapter(this);
            ListAdapter = _adapter;

            //TODO: remove magic strings
            var stateKey = Intent.GetStringExtra("Authorizer");
            var completionSource = StateRepo.Remove(stateKey);

            completionSource.SetResult(this);
        }

        public void OnDeviceFound(BluetoothDevice device)
        {
            if (!_adapter.Contains(device))
            {
                _adapter.Add(device);
                _adapter.NotifyDataSetChanged();
            }
        }

        protected override void OnListItemClick(
            ListView l, 
            Android.Views.View v, 
            int position, 
            long id)
        {
            DeviceSelected?.Invoke(_adapter[position]);
        }
    }

    //TODO: use the listview option with the double row to display
    //https://developer.xamarin.com/guides/android/user_interface/working_with_listviews_and_adapters/part_3_-_customizing_a_listview's_appearance/     
    public class DeviceListAdapter : BaseAdapter<BluetoothDevice>
    {
        private readonly List<BluetoothDevice> _items;
        private readonly Activity _context;

        public DeviceListAdapter(Activity context)
        {
            _context = context;
            _items = new List<BluetoothDevice>();
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override BluetoothDevice this[int position] => _items[position];
        public override int Count => _items.Count;

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            Android.Views.View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = _items[position].Name;
            return view;
        }

        public void Add(BluetoothDevice item)
        {
            _items.Add(item);
        }

        public bool Contains(BluetoothDevice item)
        {
            return _items.Any(b => b.Address == item.Address);
        }
    }
}