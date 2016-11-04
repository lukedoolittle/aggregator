using System;
using System.Collections.Generic;
using System.Globalization;
using CoreGraphics;
using Foundation;
using Material.Contracts;
using UIKit;

namespace Material.View.BluetoothAuthorization
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public partial class DeviceTableViewController : UIViewController
    {
        private readonly CGRect _bounds;
        private TableSource _tableSource;
        private UITableView _table;
        private LoadingOverlay _loadingOverlay;

        public DeviceTableViewController(CGRect bounds)
        {
            _bounds = bounds;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _tableSource = new TableSource();
            _table = new UITableView(_bounds)
            {
                Source = _tableSource
            };
            Add(_table);
        }

        public void OnDeviceFound(BluetoothDevice device)
        {
            if (!_tableSource.Contains(device))
            {
                _tableSource.Add(device);
                _table.ReloadData();
            }
        }

        public void DisplayLoading(string deviceName)
        {
            _loadingOverlay = new LoadingOverlay(
                _bounds, 
                string.Format(
                    CultureInfo.InvariantCulture,
                    StringResources.BluetoothDialogBody, 
                    deviceName));
            _loadingOverlay.Show();
            View.Add(_loadingOverlay);
        }

        public void HideLoading()
        {
            _loadingOverlay.Hide();
        }

        public void SetDeviceSelectedHandler(Action<BluetoothDevice> handler)
        {
            _tableSource.SetDeviceSelectedHandler(handler);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                // free managed resources
                if (_table != null)
                {
                    _table.Dispose();
                }
                if (_tableSource != null)
                {
                    _tableSource.Dispose();
                }
                if (_loadingOverlay != null)
                {
                    _loadingOverlay.Dispose();
                }
            }
        }
    }

    public class TableSource : UITableViewSource
    {
        private Action<BluetoothDevice> _deviceSelected;

        private readonly List<BluetoothDevice> _items;
        private const string CellIdentifier = "TableCell";

        public TableSource()
        {
            _items = new List<BluetoothDevice>();
        }

        public override nint RowsInSection(
            UITableView tableview, 
            nint section)
        {
            return _items.Count;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public override UITableViewCell GetCell(
            UITableView tableView, 
            NSIndexPath indexPath)
        {
            if (tableView == null) throw new ArgumentNullException(nameof(tableView));
            if (indexPath == null) throw new ArgumentNullException(nameof(indexPath));

            var cell = tableView.DequeueReusableCell(CellIdentifier);
            var item = _items[indexPath.Row];

            if (cell == null)
            {
                cell = new UITableViewCell(
                    UITableViewCellStyle.Subtitle, 
                    CellIdentifier);
            }

            cell.TextLabel.Text = item.Name;
            cell.DetailTextLabel.Text = item.Address.ToString();

            return cell;
        }

        public void Add(BluetoothDevice item)
        {
            _items.Add(item);
        }

        public bool Contains(BluetoothDevice item)
        {
            return _items.Contains(item);
        }

        public override void RowSelected(
            UITableView tableView, 
            NSIndexPath indexPath)
        {
            if (tableView == null) throw new ArgumentNullException(nameof(tableView));
            if (indexPath == null) throw new ArgumentNullException(nameof(indexPath));

            _deviceSelected?.Invoke(_items[indexPath.Row]);
            tableView.DeselectRow(indexPath, true);
        }

        public void SetDeviceSelectedHandler(Action<BluetoothDevice> handler)
        {
            _deviceSelected = handler;
        }
    }
}