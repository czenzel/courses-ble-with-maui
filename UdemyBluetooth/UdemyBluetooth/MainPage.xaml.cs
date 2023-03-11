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
        if (!IsBusy)
        {
            return;
        }

        await DisplayAlert("Server", "Server Clicked!", "Cancel");
    }

    private async void BLEScan_Clicked(object sender, EventArgs e)
    {
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

