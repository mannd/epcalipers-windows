using EPCalipersWinUI3.Calipers;
using EPCalipersWinUI3.Contracts;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;

namespace EPCalipersWinUI3.Views
{
    public partial class CaliperGrid : Grid, ICaliperView
	{
		public InputCursor InputCursor
		{
			get => ProtectedCursor;
			set => ProtectedCursor = value;
		}

		public Bounds Bounds => new Bounds(ActualWidth, ActualHeight);

		public void Add(Line barLine)
		{
			Children.Add(barLine);
		}
		public void Remove(Line barLine)
		{
			Children.Remove(barLine);
		}

	}
}
