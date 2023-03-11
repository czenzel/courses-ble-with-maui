using Shiny.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyBluetooth.Interfaces;
using UdemyBluetooth.Structures;
using SystemTimer = System.Timers.Timer;

namespace UdemyBluetooth.Services
{
    public class BluetoothClient : IBluetoothClient
    {
        private readonly IBleManager _bleManager;
        private readonly ObservableList<BluetoothDevice> _devices = new ObservableList<BluetoothDevice>();

        private BluetoothDevice _connectedDevice;

        public BluetoothClient(IMauiInterface mauiInterface)
        {
            _bleManager = mauiInterface.Resolve(typeof(IBleManager)) as IBleManager;
        }

        public void Connect(BluetoothDevice device)
        {
            IPeripheral peripheral = (IPeripheral)device;

            peripheral.WhenConnected()
                .Subscribe(_device =>
                {
                    _connectedDevice = device;
                });

            peripheral.Connect(new ConnectionConfig(false));
        }

        public void Disconnect()
        {
            IPeripheral? device = null;

            if (_connectedDevice != null)
                device = (IPeripheral)_connectedDevice.Device;

            if (!(device!.Status == ConnectionState.Connected))
                return;

            device.WhenDisconnected()
                .Subscribe(_device =>
                {
                    _connectedDevice = null;
                });

            device.CancelConnection();
        }

        public void StartScan()
        {
            if (_bleManager.IsScanning)
            {
                StopScan();
            }

            _devices.Clear();

            _bleManager.Scan()
                .Subscribe(a =>
                {
                    if (!_devices.Any(b => b.Uuid.Equals(a.Peripheral.Uuid)))
                    {
                        _devices.Add(new BluetoothDevice()
                        {
                            Device = a.Peripheral,
                            LocalName = a.AdvertisementData.LocalName,
                            Uuid = a.Peripheral.Uuid
                        });
                    }
                });
        }

        public void StopScan()
        {
            _bleManager.StopScan();
        }

        public ObservableList<BluetoothDevice> ScanResults => _devices;

        public BluetoothDevice ConnectedDevice => _connectedDevice;
    }
}
