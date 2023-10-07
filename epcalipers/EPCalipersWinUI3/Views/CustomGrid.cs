using EPCalipersWinUI3.Calipers;
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

namespace EPCalipersWinUI3.Views
{
    public class CustomGrid : Grid
	{

		public InputCursor InputCursor
		{
			get => ProtectedCursor;
			set => ProtectedCursor = value;
		}
	}
}
