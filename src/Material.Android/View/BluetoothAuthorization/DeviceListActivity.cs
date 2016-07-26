using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Material.Contracts;

namespace Material.View.BluetoothAuthorization
{
    [Activity(Label = "DeviceListActivity")]
    public class DeviceListActivity : ListActivity
    {
        internal const string Authorizer = "Authorizer";
        internal static readonly ActivityStateRepository<TaskCompletionSource<DeviceListActivity>> StateRepo =
            new ActivityStateRepository<TaskCompletionSource<DeviceListActivity>>();

        private DeviceListAdapter _adapter;

        public Action<BluetoothDevice> DeviceSelected { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _adapter = new DeviceListAdapter(this);
            ListAdapter = _adapter;

            var stateKey = Intent.GetStringExtra(Authorizer);
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
            ListView listView, 
            Android.Views.View view, 
            int position, 
            long id)
        {
            DeviceSelected?.Invoke(_adapter[position]);
        }
    }

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

        public override Android.Views.View GetView(
            int position, 
            Android.Views.View convertView, 
            ViewGroup parent)
        {
            var view = convertView ?? 
                _context.LayoutInflater.Inflate(
                    Android.Resource.Layout.SimpleListItem2, 
                    null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = 
                _items[position].Name;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text =
                _items[position].Address.ToString();

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