using System;
using System.Windows.Forms;

namespace epcalipers
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
#pragma warning disable CA2000 // Dispose objects before losing scope
			Application.Run(new Form1());
#pragma warning restore CA2000 // Dispose objects before losing scope
		}
	}
}
