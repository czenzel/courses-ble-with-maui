using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyBluetooth.Interfaces;
using Microsoft.Maui;
using MauiPermissions = Microsoft.Maui.ApplicationModel.Permissions;

namespace UdemyBluetooth.Services
{
    public class Permissions : IPermissions
    {
        public bool RequestBluetooth()
        {
            return RequestLocation();
        }

        public bool RequestLocation()
        {
            var status = MauiPermissions.RequestAsync<MauiPermissions.LocationWhenInUse>().GetAwaiter().GetResult();
            // Put status check here on the permission status. 
            return status == PermissionStatus.Granted;
        }
    }
}
