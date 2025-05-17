using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadramiShop.Services;

public class AppState:INotifyPropertyChanged
{
    private bool _isBusy;
    public bool IsBusy 
    {
        get => _isBusy;
        set 
        { 
          if (_isBusy != value)
            {
                _isBusy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
            }
             
        } 
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void ShowLoader() => IsBusy = true;

    public void HideLoader() => IsBusy = false;
}
