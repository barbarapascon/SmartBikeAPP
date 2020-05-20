using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBikeApp.Interfaces
{
    public interface INotification
    {
        void CreateNotification(String title, String message);
    }
}
