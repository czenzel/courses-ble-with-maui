using UdemyBluetooth.Core;
using UdemyBluetooth.Interfaces;

namespace UdemyBluetooth;

public partial class MainPage : ContentPage
{
    private bool IsBusy { get; set; } = false;

	public MainPage()
	{
		InitializeComponent();
	}

    private async void BLEServer_Clicked(object sender, EventArgs e)
    {
        if (IsBusy)
        {
            return;
        }

        await Task.Run(() =>
        {
            try
            {
                IBluetoothServer server = Resolver.Resolve<IBluetoothServer>();

                if (server.Started)
                {
                    server.Stop();
                    Dispatcher.Dispatch(() => btnServer.Text = "Start BLE Server");
                }
                else
                {
                    server.Start();
                    Dispatcher.Dispatch(() => btnServer.Text = "Stop BLE Server");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        });
    }

    private async void BLEScan_Clicked(object sender, EventArgs e)
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;

        await Task.Run(async () =>
        {
            try
            {
                IBluetoothClient client = Resolver.Resolve<IBluetoothClient>();

                client.ScanResults.CollectionChanged += (s, e) =>
                {
                    System.Diagnostics.Debug.WriteLine($"BLE Scan > Devices In List: {client.ScanResults.Count}");
                };

                client.StartScan();
                await Task.Delay(TimeSpan.FromSeconds(15));
                client.StopScan();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        });
    }
}

