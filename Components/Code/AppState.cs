using System;
using System.ComponentModel;

public class AppState : INotifyPropertyChanged
{
    private bool _hosting;

    public bool Hosting
    {
        get => _hosting;
        set
        {
            if (_hosting != value)
            {
                _hosting = value;
                NotifyPropertyChanged(nameof(Hosting));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
