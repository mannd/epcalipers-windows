using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPCalipersWinUI3.Helpers;
using Microsoft.UI.Xaml.Media.Imaging;

namespace EPCalipersWinUI3.Models
{
	public class PdfImagePage
	{
		public SoftwareBitmapSource PageSource { get; set; } = null;
		public double RotationAngle { get; set; } = 0;
		// TODO: ? Need ZoomFactor field, bound to View, i.e. need view model to handle zoom, not view
		/* This is because we may need to maintain zoom between pdf pages, and view resets zoom and rotation
		 * whenever EcgImage source changes.....
		 */
	}
}
