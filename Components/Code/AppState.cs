using System;
using System.ComponentModel;
using System.Collections.Generic;

//this classed is used to keep track of the hosting status of the application and send out an alert if the hosting status changes.
public class AppState : INotifyPropertyChanged
//defines the public class called AppState
//INotifyPropertyChanged interface is implemented by AppState class
//INotifyPropertyChanged is used to notify when a value within AppState is changed
{
	private bool _hosting;
	private List<Report> _reports;

	//the method below, is used to determine if the status of _hosting changes. if this is the case, the NotifyPropertyChanged method is called
	public bool Hosting {
		get {
			//NotifyPropertyChanged(nameof(Hosting));
			return _hosting;
		}
		set {
			_hosting = value;
			NotifyPropertyChanged(nameof(Hosting));
		}
	}

	//the method below, is used to determine if the status of _reports changes. if this is the case, the NotifyPropertyChanged method is called
	public List<Report> Reports {
		get {
			//NotifyPropertyChanged(nameof(Reports));
			_reports = SessionData.Reports;
            return _reports;
		}
		set {
			_reports = value;
			NotifyPropertyChanged(nameof(Reports));
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;
	//the method below triggers the property changed event within the current instance of the class.
	//a notification is sent using this method, using the propertyName string, which refers to the property that has changed.
	private void NotifyPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
