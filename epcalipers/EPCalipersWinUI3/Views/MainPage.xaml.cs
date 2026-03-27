using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using EPCalipersWinUI3.Models;
using EPCalipersWinUI3.Models.Calipers;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.Win32.Foundation;
using WinRT.Interop;
using System.Collections.Generic;
using Microsoft.UI;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Text;


namespace EPCalipersWinUI3.Views
{
	public sealed partial class MainPage : Page
	{
		#region fields
		public MainPageViewModel ViewModel { get; set; }

		private const int _dragMargin = 5;
		private Point _rightClickPosition;

		private static readonly string _saveFileDialogTitle = "FileSavedTitle".GetLocalized();
		private static readonly string _fileSavedMessage = "FileSavedMessage".GetLocalized();
		private static readonly string _fileSavedAndRenamedMessage = "FileSavedAndRenamedMessage".GetLocalized();
		private static readonly string _fileCouldntBeSavedMessage = "FileCouldntBeSavedMessage".GetLocalized();
		private static readonly string _fileSaveCancelledMessage = "FileSaveCancelledMessage".GetLocalized();

		private readonly Windows.Win32.Graphics.Direct3D11.ID3D11Device _d3dDevice;
		private readonly IDirect3DDevice _device;
		#endregion
		#region constructor, navigation
		public MainPage()
		{
			InitializeComponent();
			Debug.Print("MainPage constructor");
			Loaded += MainPage_Loaded;
			ViewModel = new MainPageViewModel(SetZoom, CaliperView, ScrollView);

			// Used for screenshot features
			_d3dDevice = Direct3D11Helper.CreateD3DDevice();
			_device = Direct3D11Helper.CreateDirect3DDeviceFromD3D11Device(_d3dDevice);

			// Don't bother showing screenshot capture if it isn't supported at all on device.
			if (!GraphicsCaptureSession.IsSupported())
			{
				OpenFromScreenShotMenuFlyoutItem.Visibility = Visibility.Collapsed;
			}

			ZoomInMenuItem.KeyboardAccelerators.Add(new KeyboardAccelerator()
			{
				// Plus key on keyboard
				Key = (Windows.System.VirtualKey)0xBB,
				Modifiers = Windows.System.VirtualKeyModifiers.Control
			});

			ZoomOutMenuItem.KeyboardAccelerators.Add(new KeyboardAccelerator()
			{
				// Minus key on keyboard
				Key = (Windows.System.VirtualKey)0xBD,
				Modifiers = Windows.System.VirtualKeyModifiers.Control
			});

			ScrollView.RegisterPropertyChangedCallback(ScrollViewer.ZoomFactorProperty, (s, e) =>
			{
				ViewModel.ZoomFactor = ScrollView.ZoomFactor;
				UpdateNoteFrames();
			});

			EcgImage.RegisterPropertyChangedCallback(Image.SourceProperty, (s, e) =>
			{
				ViewModel.Bounds = CaliperView.Bounds;

				if (ViewModel.ClearCalipersBetweenPdfPages)
				{
					ViewModel.DeleteAllCalipersCommand.Execute(null);
				}
				if (ViewModel.ResetZoomWithNewPdfPage)
				{
					ViewModel.ResetZoomCommand.Execute(null);
				}
				if (ViewModel.ResetRotationWithNewPdfPage)
				{
					RotateImageWithoutAnimation(0);
				}
				else
				{
					var originalRotation = _imageRotation;
					RotateImageWithoutAnimation(originalRotation);
				}
				UpdateNotesCanvasSize();
				UpdateNoteFrames();
			});
		}

		// DEFER: Investigate further.  It appears necessary to refresh calipers TWICE,
		// first in OnNavigatedTo() and
		// then in MainPage_Loaded() to get changes to update.  Why??
		private void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			Debug.Print("MainPage_Loaded()");
			ViewModel.LoadSampleImage();
			ViewModel.RefreshCalipers();
			ViewModel.RestoreTitleBarName();
			UpdateNotesCanvasSize();
		}

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			Debug.Print("MainPage OnNavigatorTo");
			base.OnNavigatedTo(e);
			if (AppHelper.StartupFile != null)
			{
				await ViewModel.OpenImageFile(AppHelper.StartupFile);
				AppHelper.StartupFile = null;
			}
			ViewModel.RefreshCalipers();
			await ViewModel.RefreshImageIfPdfResolutionChanged();  
			SetCaliperViewOrientation();
		}
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			ViewModel.CloseWindows();
		}

		private void SetCaliperViewOrientation()
		{
			ISettings settings = Settings.Instance;
			switch (settings.CaliperViewAlignment)
			{
				case CaliperViewAlignment.TopLeft:
					CaliperView.HorizontalAlignment = HorizontalAlignment.Left;
					CaliperView.VerticalAlignment = VerticalAlignment.Top;
					break;
				case CaliperViewAlignment.Center:
					CaliperView.HorizontalAlignment = HorizontalAlignment.Center;
					CaliperView.VerticalAlignment = VerticalAlignment.Center;
					break;
			}
		}
		#endregion
		#region touches
		private bool pointerDown = false;
		private Point pointerPosition;

		private void ScrollViewer_Tapped(object sender, TappedRoutedEventArgs e)
		{
			var position = e.GetPosition(CaliperView);
			ViewModel.ToggleCaliperSelection(position);
		}

		private void ScrollViewer_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
		{
			var position = e.GetPosition(CaliperView);
			ViewModel.RemoveAtPoint(position);
		}

		private void ScrollView_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			var position = e.GetPosition(CaliperView);
			_rightClickPosition = position;

			Debug.WriteLine($"Right click position: {_rightClickPosition.X}, {_rightClickPosition.Y}");
			var caliper = ViewModel.GetCaliperAt(position);
			ViewModel.IsNearCaliperAllowDuringCalibration = caliper != null;
			ViewModel.IsNearCaliper = ViewModel.IsNearCaliperAllowDuringCalibration && !ViewModel.IsCalibrating;
			if (caliper != null && caliper is TimeCaliper timeCaliper)
			{
				ViewModel.CaliperIsMarching = timeCaliper.IsMarching;
			}
			else
			{
				ViewModel.CaliperIsMarching = false;
			}

			bool noteTargeted = NoteIndexForContextMenu(position) >= 0;
			ViewModel.IsNearNote = noteTargeted;
			ViewModel.CanAddNote = !noteTargeted;

			// Clear prior context note highlight
			if (_contextMenuNote != null)
			{
				_contextMenuNote.IsSelected = false;
				UpdateNoteBorderVisibility(_contextMenuNote);
				_contextMenuNote = null;
			}

			// Highlight note under context click
			_contextMenuNote = GetNoteForContextMenu(position);
			if (_contextMenuNote != null)
			{
				_contextMenuNote.IsSelected = true;
				UpdateNoteBorderVisibility(_contextMenuNote);
			}
		}

		private NoteEntry GetNoteForContextMenu(Point p)
		{
			var index = NoteIndexForContextMenu(p);
			return index >= 0 ? _noteEntries[index] : null;
		}

		private void SelectComponent_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ToggleComponentSelection(_rightClickPosition);
		}
		private void SelectCaliper_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ToggleCaliperSelection(_rightClickPosition);
		}
		private void DeleteCaliper_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.DeleteCaliperAt(_rightClickPosition);
		}
		private void MarchingCaliper_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ToggleMarchingCaliper(_rightClickPosition);
		}
		private void ColorCaliper_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ShowColorDialog(_rightClickPosition);
		}

		private void ScrollView_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var point = e.GetCurrentPoint(CaliperView);
			var p = point.Position;

			int noteIndex = NoteIndexForDrag(p);
			bool onDraggableNote = noteIndex >= 0;

			bool insideEditingNote = PointInsideAnyEditingNote(p);
			//bool insideNote = PointInsideAnyNote(p);
			//bool insideNoteDragRegion = PointInsideAnyNoteDragRegion(p);

			// Any click outside the actual textbox ends note editing.
			if (!insideEditingNote)
			{
				EndAllNoteEditing();
			}

			// If the user is interacting with a note or its drag region,
			// do not start caliper interaction.
			if (onDraggableNote)
			{
				_draggingNote = _noteEntries[noteIndex];

				_noteDragStartPointer = p;

				_noteDragStartPosition = new Point(
					Canvas.GetLeft(_draggingNote.Container),
					Canvas.GetTop(_draggingNote.Container));

				CaliperView.CapturePointer(e.Pointer);

				ViewModel.ReleaseGrabbedCaliper();
				pointerDown = false;

				return;
			}

			CaliperView.CapturePointer(e.Pointer);
			pointerPosition = p;
			pointerDown = true;
			ViewModel.GrabCaliper(pointerPosition);
		}

		private void ScrollViewer_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if (_draggingNote != null)
			{
				// Bring dragging note to front.
				Canvas.SetZIndex(_draggingNote.Container, 1000);
				Canvas.SetZIndex(_draggingNote.DragHandle, 1000);

				var position = e.GetCurrentPoint(CaliperView).Position;

				var dx = position.X - _noteDragStartPointer.X;
				var dy = position.Y - _noteDragStartPointer.Y;

				var newX = _noteDragStartPosition.X + dx;
				var newY = _noteDragStartPosition.Y + dy;

				SetNotePosition(_draggingNote, newX, newY);
				return;
			}

			if (pointerDown) // && dragging caliper...
			{
				var position = e.GetCurrentPoint(CaliperView);
				if (position.Position.X < EcgImage.ActualWidth - _dragMargin
					&& position.Position.Y < EcgImage.ActualHeight - 5
					&& position.Position.Y > 5
					&& position.Position.X > 5)
				{
					ViewModel.DragCaliperComponent(position.Position);
				}
			}
		}

		private void ScrollView_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			if (_draggingNote != null)
			{
				_draggingNote = null;
				CaliperView.ReleasePointerCapture(e.Pointer);
				return;
			}

			ViewModel.ReleaseGrabbedCaliper();
			CaliperView.ReleasePointerCapture(e.Pointer);
			pointerDown = false;
		}
		#endregion
		#region zoom
		/// <summary>
		/// Delegate method that view model uses to set zoom on scroll view
		/// </summary>
		/// <param name="zoom"></param>
		private void SetZoom(float zoom)
		{
			ScrollView?.ChangeView(0, 0, zoom);
		}
		#endregion
		#region drag and drop
		private async void EcgImage_Drop(object sender, DragEventArgs e)
		{
			if (e.DataView.Contains(StandardDataFormats.StorageItems))
			{
				var items = await e.DataView.GetStorageItemsAsync();
				if (items.Count > 0)
				{
					var storageFile = items[0] as StorageFile;
					// check file types first???
					// The commands below are needed to avoid retaining calipers when dropping a new file.
					// Otherwise, when going from a multipage PDF to dropping a file, calipers can be retained inadvertently.
					// Also need to avoid retaining zoom, rotation.
					ViewModel.DeleteAllCalipersCommand.Execute(null);
					ViewModel.ClearCalibrationCommand.Execute(null);
					ViewModel.ResetZoomCommand.Execute(null);
					RotateImageWithoutAnimation(0);
					await ViewModel.OpenImageFile(storageFile);
				}
			}
		}

		private void EcgImage_DragOver(object sender, DragEventArgs e)
		{
			e.AcceptedOperation = DataPackageOperation.Link;
			e.Handled = true;
		}
		#endregion
		#region rotation
		// Note rotation is handled in the code behind file because it is purely a
		// manipulation of the view, with no affect on the view model.

		private readonly static TimeSpan _rotationDuration = TimeSpan.FromSeconds(0.4);
		private double _imageRotation = 0;
		private double _rotatedImageScale = 1.0;

		private void Rotate90R_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(90);
		}

		private void Rotate90L_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(-90);
		}

		private void Rotate1R_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(1);
		}

		private void Rotate1L_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(-1);
		}

		private void Rotate01R_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(0.1);
		}

		private void Rotate01L_Click(object sender, RoutedEventArgs e)
		{
			RotateImageByAngle(-0.1);
		}

		private void ResetRotation_Click(object sender, RoutedEventArgs e)
		{
			RotateImageToAngle(0);
		}

		private void RotateImageByAngle(double angle)
		{
			var originalRotation = _imageRotation;
			_imageRotation += angle;
			RotateImage(originalRotation, _imageRotation);
		}

		private void RotateImageToAngle(double angle)
		{
			var originalRotation = _imageRotation;
			_imageRotation = angle;
			RotateImage(originalRotation, _imageRotation);
		}

		private void RotateImage(double startAngle, double endAngle)
		{
			EcgImage.RenderTransformOrigin = new Point(0.5, 0.5);
			Storyboard storyboard = new()
			{
				Duration = new Duration(_rotationDuration)
			};
			DoubleAnimation rotateAnimation = new()
			{
				From = startAngle,
				To = endAngle,
				Duration = storyboard.Duration
			};

			var scaledWidth = _rotatedImageScale * EcgImage.ActualWidth;
			var scaledHeight = _rotatedImageScale * EcgImage.ActualHeight; ;
			_rotatedImageScale = MathHelper.ScaleToFit(scaledWidth, scaledHeight, _imageRotation);

			DoubleAnimation scaleAnimation = new()
			{
				Duration = storyboard.Duration,
				To = _rotatedImageScale
			};
			DoubleAnimation scaleYAnimation = new()
			{
				Duration = storyboard.Duration,
				To = _rotatedImageScale
			};
			Storyboard.SetTarget(scaleAnimation, EcgImage);
			Storyboard.SetTarget(scaleYAnimation, EcgImage);
			Storyboard.SetTarget(rotateAnimation, EcgImage);
			Storyboard.SetTargetProperty(scaleAnimation,
				"(UIElement.RenderTransform).(CompositeTransform.ScaleX)");
			Storyboard.SetTargetProperty(scaleYAnimation,
				"(UIElement.RenderTransform).(CompositeTransform.ScaleY)");
			Storyboard.SetTargetProperty(rotateAnimation,
				"(UIElement.RenderTransform).(CompositeTransform.Rotation)");
			storyboard.Children.Add(scaleAnimation);
			storyboard.Children.Add(scaleYAnimation);
			storyboard.Children.Add(rotateAnimation);
			storyboard.Begin();
		}

		private void RotateImageWithoutAnimation(double angle)
		{
			if (EcgImage?.Source == null) { return; }
			_imageRotation = angle;
			EcgImage.RenderTransformOrigin = new Point(0.5, 0.5);
			CompositeTransform rotateTransform = new()
			{
				CenterX = EcgImage.Width / 2,
				CenterY = EcgImage.Height / 2,
				Rotation = _imageRotation,
			};
			EcgImage.RenderTransform = rotateTransform;
		}
		#endregion
		#region event handlers
		// Events that open dialogs are handled in the code behind file.
		private async void About_Click(object sender, RoutedEventArgs e) => await CommandHelper.About(XamlRoot);

		private async void GotoPdfPage_Click(object sender, RoutedEventArgs e)
		{
			var gotoPdfPageDialog = new GotoPdfPageDialog(ViewModel)
			{
				XamlRoot = XamlRoot
			};
			var result = await gotoPdfPageDialog.ShowAsync();
			Debug.WriteLine(result);
			if (result == ContentDialogResult.Primary)
			{
				var page = gotoPdfPageDialog.PageNumber;
				await ViewModel.GotoPdfPage(page);
			}
		}

		private async void OpenFile_Click(object sender, RoutedEventArgs e)
		{
			if (await ViewModel.WarnIfLocked(XamlRoot))
			{
				return;
			}

			// Create a file picker
			var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

			// Retrieve the window handle (HWND) of the current WinUI 3 window.
			var mainWindow = AppHelper.AppMainWindow;
			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(mainWindow);

			// Initialize the file picker with the window handle (HWND).
			WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

			// Set options for your file picker
			openPicker.ViewMode = PickerViewMode.Thumbnail;
			openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
			openPicker.FileTypeFilter.Add(".jpg");
			openPicker.FileTypeFilter.Add(".jpeg");
			openPicker.FileTypeFilter.Add(".png");
			openPicker.FileTypeFilter.Add(".bmp");
			openPicker.FileTypeFilter.Add(".pdf");
			// Open the picker for the user to pick a file
			var file = await openPicker.PickSingleFileAsync();
			// Change the cursor to a wait icon
			CaliperView.InputCursor = InputSystemCursor.Create(InputSystemCursorShape.Wait);
			await ViewModel.OpenImageFile(file);
			CaliperView.InputCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
		}

		private async Task StartPickerCaptureAsync()
		{
			var hwnd = new HWND(WindowNative.GetWindowHandle(AppHelper.AppMainWindow));
			var picker = new GraphicsCapturePicker();
			InitializeWithWindow.Initialize(picker, hwnd);
			var item = await picker.PickSingleItemAsync();

			if (item != null)
			{
				var source = await StartCaptureFromItemAsync(item);
				ViewModel.MainImageSource = source;
				ViewModel.IsMultipagePdf = false;
				ViewModel.SetTitleBarName("Screenshot".GetLocalized());
			}
		}

		private async Task<SoftwareBitmapSource> StartCaptureFromItemAsync(GraphicsCaptureItem item)
		{
			var softwareBitmap = await GetSoftwareBitmapFromItemAsync(item);
			var source = new SoftwareBitmapSource();
			await source.SetBitmapAsync(softwareBitmap);
			return source;
		}

		private async Task<SoftwareBitmap> GetSoftwareBitmapFromItemAsync(GraphicsCaptureItem item)
		{
			var surface = await CaptureSnapshot.CaptureAsync(_device, item);
			var softwareBitmap = await SoftwareBitmap.CreateCopyFromSurfaceAsync(surface, BitmapAlphaMode.Premultiplied);
			return softwareBitmap;
		}

		private async void OpenFromScreenshot_Click(object sender, RoutedEventArgs e)
		{
			if (!GraphicsCaptureSession.IsSupported()) return;
			await StartPickerCaptureAsync();
		}

		private async void SaveScreenshot_Click(object sender, RoutedEventArgs e)
		{
			var softwareBitmap = await RenderCaliperView();
			SaveScreenshotToFile(softwareBitmap);
		}



		// Alternative screenshot methods, inferior to SaveScreenshot_Click because it screenshots
		// the whole app window, including menus etc.  Currently unused.
		//private async Task SaveAppWindowScreenshot_Click(object sender, RoutedEventArgs e)
		//{
		//	if (!GraphicsCaptureSession.IsSupported()) return;
		//	await Task.Yield(); // Updates UI, ensuring menu closes before screenshot.
		//	await StartHwndCapture();
		//}
		//
		//private async Task StartHwndCapture()
		//{
		//	var hwnd = new HWND(WindowNative.GetWindowHandle(AppHelper.AppMainWindow));
		//	var item = CaptureSnapshot.CreateItemForWindow(hwnd);
		//	if (item != null)
		//	{
		//		var source = await GetSoftwareBitmapFromItemAsync(item);
		//		// save to file
		//		SaveScreenshotToFile(source);
		//	}
		//}

		// See https://learn.microsoft.com/en-us/uwp/api/windows.graphics.imaging.bitmapencoder?view=winrt-22621&devlangs=csharp&f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(Windows.Graphics.Imaging.BitmapEncoder)%3Bk(DevLang-csharp)%26rd%3Dtrue
		private async void SaveScreenshotToFile(SoftwareBitmap bitmap)
		{
			try
			{
				FileSavePicker savePicker = new();
				var hWnd = WindowNative.GetWindowHandle(AppHelper.AppMainWindow);
				InitializeWithWindow.Initialize(savePicker, hWnd);
				savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
				savePicker.FileTypeChoices.Add("JPG image", [".jpg"]);
				savePicker.FileTypeChoices.Add("PNG image", [".png"]);
				savePicker.FileTypeChoices.Add("BMP image", [".bmp"]);
				savePicker.SuggestedFileName = "EPCalipersScreenshot";
				StorageFile file = await savePicker.PickSaveFileAsync();
				ContentDialog dialog;
				if (file != null)
				{
					// Prevent updates to the remote version of the file
					// until we finish making changes and call CompleteUpdatesAsync.
					CachedFileManager.DeferUpdates(file);
					SaveSoftwareBitmapToFile(bitmap, file);
					FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
					if (status == FileUpdateStatus.Complete)
					{
						Debug.Print(_fileSavedMessage);
						dialog = MessageHelper.CreateMessageDialog(_saveFileDialogTitle, _fileSavedMessage);
					}
					else if (status == FileUpdateStatus.CompleteAndRenamed)
					{
						Debug.Print(_fileSavedAndRenamedMessage);
						dialog = MessageHelper.CreateMessageDialog(_saveFileDialogTitle, _fileSavedAndRenamedMessage);
					}
					else
					{
						Debug.Print(_fileCouldntBeSavedMessage);
						dialog = MessageHelper.CreateErrorDialog(_fileCouldntBeSavedMessage);
					}
				}
				else
				{
					Debug.Print(_fileSaveCancelledMessage);
					dialog = MessageHelper.CreateMessageDialog(_saveFileDialogTitle, _fileSaveCancelledMessage);
				}
				dialog.XamlRoot = XamlRoot;
				await dialog.ShowAsync();
			}
			catch (Exception ex)
			{
				Debug.Print(ex.ToString());
				var dialog = MessageHelper.CreateErrorDialog(ex.ToString());
				dialog.XamlRoot = XamlRoot;
				await dialog.ShowAsync();
			}
		}

		private static async void SaveSoftwareBitmapToFile(SoftwareBitmap softwareBitmap, StorageFile outputFile)
		{
			using IRandomAccessStream stream = await outputFile.OpenAsync(FileAccessMode.ReadWrite);
			// Create an encoder with the desired format
			var ext = outputFile.FileType;
			ext = ext.ToLower();
			Debug.Print(ext);
			var encoderID = ext switch
			{
				".png" => BitmapEncoder.PngEncoderId,
				".bmp" => BitmapEncoder.BmpEncoderId,
				_ => BitmapEncoder.JpegEncoderId,
			};
			BitmapEncoder encoder = await BitmapEncoder.CreateAsync(encoderID, stream);

			// Set the software bitmap
			encoder.SetSoftwareBitmap(softwareBitmap);
			encoder.IsThumbnailGenerated = true;

			try
			{
				await encoder.FlushAsync();
			}
			catch (Exception err)
			{
				const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
				switch (err.HResult)
				{
					case WINCODEC_ERR_UNSUPPORTEDOPERATION:
						// If the encoder does not support writing a thumbnail, then try again
						// but disable thumbnail generation.
						encoder.IsThumbnailGenerated = false;
						break;
					default:
						throw;
				}
			}

			if (encoder.IsThumbnailGenerated == false)
			{
				await encoder.FlushAsync();
			}
		}

		private async Task<SoftwareBitmap> RenderCaliperView()
		{
			var renderTargetBitmap = new RenderTargetBitmap();
			await renderTargetBitmap.RenderAsync(CaliperView);
			var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
			var softwareBitmap = SoftwareBitmap.CreateCopyFromBuffer(
				pixelBuffer,
				BitmapPixelFormat.Bgra8,
				renderTargetBitmap.PixelWidth,
				renderTargetBitmap.PixelHeight
				);
			return softwareBitmap;
		}
		#endregion

		#region notes
		private sealed class NoteEntry
		{
			public Border Container { get; set; }
			public RichEditBox Editor { get; set; }
			public Border DragHandle { get; set; } // Transparent border around the note
												   // that serves as a larger hit target for dragging the note.
			public Point AbsoluteAnchor { get; set; }
			public bool IsHovering { get; set; }
			public bool IsEditing { get; set; }
			public bool IsSelected { get; set; }
			public int HoverCount { get; set; }
		}

		private readonly List<NoteEntry> _noteEntries = [];
		private readonly Size _defaultNoteSize = new(180, 80);
		private const double _noteHitSlop = 10.0;
		private const double _defaultNoteFontSize = 14.0;
		private const double _minimumFontSize = 10.0;
		private const double _maximumFontSize = 36.0;
		private NoteEntry _contextMenuNote;

		private NoteEntry _draggedNote;
		private Point _lastDragPoint;
		private bool _isDraggingNote;
		private NoteEntry _draggingNote;
		private Point _noteDragStartPointer;
		private Point _noteDragStartPosition;

		private bool HasNotes => _noteEntries.Count > 0;

		// TODO: Delete note if it is empty after adding.  Also consider delete note
		// if it is empty after editing and losing focus.
		private void AddNote()
		{
			var absoluteAnchor = ResolveNoteAbsoluteAnchor();
			var scaledAnchor = NoteAnchorInViewFromAbsoluteAnchor(absoluteAnchor);
			var scaledOrigin = NoteOriginInViewFromAnchor(scaledAnchor);
			ISettings settings = Settings.Instance;

			var noteWidth = settings.DefaultNoteWidth;
			var noteHeight = settings.DefaultNoteHeight;

			var editor = new RichEditBox
			{
				Width = noteWidth,
				Height = noteHeight,
				Background = new SolidColorBrush(Colors.Transparent),
				BorderThickness = new Thickness(0),
				TextWrapping = TextWrapping.Wrap,
				AcceptsReturn = true,
				IsSpellCheckEnabled = false,
				//FontSize = _defaultNoteFontSize,
				FontSize = settings.DefaultNoteFontSize,
				//Foreground = new SolidColorBrush(Colors.Black),
				Foreground = new SolidColorBrush(settings.DefaultNoteForegroundColor),
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch,
				IsReadOnly = false,
				IsTabStop = true,
				AllowFocusOnInteraction = true,
				IsHitTestVisible = true
			};
			editor.Document.SetText(TextSetOptions.None, "");

			var container = new Border
			{
				Width = noteWidth,
				Height = noteHeight,
				Background = new SolidColorBrush(Colors.Transparent),
				BorderBrush = new SolidColorBrush(Colors.Black),
				BorderThickness = new Thickness(0),
				Child = editor
			};

			var dragHandle = new Border
			{
				Width = noteWidth + (_noteHitSlop * 2),
				Height = noteHeight + (_noteHitSlop * 2),
				Background = new SolidColorBrush(Colors.Transparent)
			};

			var entry = new NoteEntry
			{
				Container = container,
				Editor = editor,
				DragHandle = dragHandle,
				AbsoluteAnchor = absoluteAnchor
			};

			Canvas.SetLeft(container, scaledOrigin.X);
			Canvas.SetTop(container, scaledOrigin.Y);
			Canvas.SetLeft(dragHandle, scaledOrigin.X - _noteHitSlop);
			Canvas.SetTop(dragHandle, scaledOrigin.Y - _noteHitSlop);

			WireNoteEvents(entry);

			Debug.WriteLine($"Note placed at: {scaledOrigin.X}, {scaledOrigin.Y}");

			NotesCanvas.Children.Add(dragHandle);
			NotesCanvas.Children.Add(container);

			_noteEntries.Add(entry);
			UpdateNoteBorderVisibility(entry);

			BeginEditingNote(entry);
		}

		private Rect GetNoteRect(NoteEntry entry)
		{
			var x = Canvas.GetLeft(entry.Container);
			var y = Canvas.GetTop(entry.Container);
			return new Rect(x, y, entry.Container.Width, entry.Container.Height);
		}

		private Rect GetExpandedNoteRect(NoteEntry entry)
		{
			var rect = GetNoteRect(entry);
			return new Rect(
				rect.X - _noteHitSlop,
				rect.Y - _noteHitSlop,
				rect.Width + (_noteHitSlop * 2),
				rect.Height + (_noteHitSlop * 2));
		}

		private int NoteIndexForContextMenu(Point p)
		{
			for (int i = _noteEntries.Count - 1; i >= 0; i--)
			{
				var entry = _noteEntries[i];

				// Do not allow delete targeting while actively editing.
				if (entry.IsEditing)
				{
					continue;
				}

				var noteRect = GetNoteRect(entry);
				var expandedRect = GetExpandedNoteRect(entry);

				if (noteRect.Contains(p) || expandedRect.Contains(p))
				{
					return i;
				}
			}

			return -1;
		}

		private void SetNotePosition(NoteEntry entry, double x, double y)
		{
			entry.AbsoluteAnchor = new Point(x, y);

			Canvas.SetLeft(entry.Container, x);
			Canvas.SetTop(entry.Container, y);

			Canvas.SetLeft(entry.DragHandle, x - _noteHitSlop);
			Canvas.SetTop(entry.DragHandle, y - _noteHitSlop);
		}

		private bool PointInsideAnyEditingNote(Point p)
		{
			foreach (var entry in _noteEntries)
			{
				if (!entry.IsEditing)
				{
					continue;
				}

				var x = Canvas.GetLeft(entry.Container);
				var y = Canvas.GetTop(entry.Container);

				var rect = new Rect(
					x,
					y,
					entry.Container.Width,
					entry.Container.Height);

				if (rect.Contains(p))
				{
					return true;
				}
			}

			return false;
		}

		private bool PointIsInNonEditingNoteDragTarget(NoteEntry entry, Point p)
		{
			if (entry.IsEditing)
			{
				return false;
			}

			var noteRect = GetNoteRect(entry);
			var expandedRect = GetExpandedNoteRect(entry);

			return noteRect.Contains(p) || expandedRect.Contains(p);
		}

		private int NoteIndexForDrag(Point p)
		{
			for (int i = _noteEntries.Count - 1; i >= 0; i--)
			{
				if (PointIsInNonEditingNoteDragTarget(_noteEntries[i], p))
				{
					return i;
				}
			}
			return -1;
		}

		private NoteEntry GetNoteNear(Point p)
		{
			var index = NoteIndexNear(p);
			return index >= 0 ? _noteEntries[index] : null;
		}

		private bool PointInsideAnyNoteDragRegion(Point p)
		{
			foreach (var entry in _noteEntries)
			{
				var x = Canvas.GetLeft(entry.DragHandle);
				var y = Canvas.GetTop(entry.DragHandle);
				var rect = new Rect(x, y, entry.DragHandle.Width, entry.DragHandle.Height);
				if (rect.Contains(p))
				{
					return true;
				}
			}
			return false;
		}

		private bool PointInsideAnyNote(Point p)
		{
			foreach (var entry in _noteEntries)
			{
				// Get the note's position
				var x = Canvas.GetLeft(entry.Container);
				var y = Canvas.GetTop(entry.Container);

				// Build the rectangle representing the actual textbox area
				var rect = new Rect(x, y, entry.Container.Width, entry.Container.Height);

				if (rect.Contains(p))
				{
					return true;
				}
			}
			return false;
		}

		private void WireNoteEvents(NoteEntry entry)
		{
			entry.Container.PointerEntered += (_, __) =>
			{
				entry.HoverCount++;
				entry.IsHovering = entry.HoverCount > 0;
				UpdateNoteBorderVisibility(entry);
			};

			entry.Container.PointerExited += (_, __) =>
			{
				entry.HoverCount = Math.Max(0, entry.HoverCount - 1);
				entry.IsHovering = entry.HoverCount > 0;
				if (!_isDraggingNote || _draggedNote != entry)
				{
					UpdateNoteBorderVisibility(entry);
				}
			};

			//entry.Editor.GotFocus += (_, __) =>
			//{
			//	UpdateNoteBorderVisibility(entry);
			//};

			//entry.Editor.LostFocus += (_, __) =>
			//{
			//	if (!entry.IsEditing)
			//	{
			//		UpdateNoteBorderVisibility(entry);
			//	}
			//};

			//entry.Container.PointerPressed += (_, e) =>
			//{
			//	var p = e.GetCurrentPoint(entry.Container);
			//	if (p.Properties.IsLeftButtonPressed)
			//	{
			//		BeginEditingNote(entry);
			//		e.Handled = true;
			//	}
			//};

			entry.DragHandle.PointerEntered += (_, __) =>
			{
				entry.HoverCount++;
				entry.IsHovering = entry.HoverCount > 0;
				UpdateNoteBorderVisibility(entry);
			};

			entry.DragHandle.PointerExited += (_, __) =>
			{
				entry.HoverCount = Math.Max(0, entry.HoverCount - 1);
				entry.IsHovering = entry.HoverCount > 0;
				UpdateNoteBorderVisibility(entry);
			};

			entry.Container.Tapped += (_, e) =>
			{
				if (!entry.IsEditing)
				{
					BeginEditingNote(entry);
					e.Handled = true;
				}
			};

			entry.DragHandle.PointerPressed += NoteDragHandle_PointerPressed;
			entry.DragHandle.PointerMoved += NoteDragHandle_PointerMoved;
			entry.DragHandle.PointerReleased += NoteDragHandle_PointerReleased;
			entry.DragHandle.PointerCanceled += NoteDragHandle_PointerReleased;
		}

		private void NoteDragHandle_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var handle = sender as Border;
			var entry = FindNoteByHandle(handle);
			if (entry == null)
			{
				return;
			}

			if (!IsBorderVisible(entry))
			{
				return;
			}

			var pointInHandle = e.GetCurrentPoint(handle).Position;
			var innerRect = new Rect(_noteHitSlop, _noteHitSlop, _defaultNoteSize.Width, _defaultNoteSize.Height);

			if (innerRect.Contains(pointInHandle))
			{
				return;
			}

			_draggedNote = entry;
			_isDraggingNote = true;
			_lastDragPoint = e.GetCurrentPoint(CaliperView).Position;
			handle.CapturePointer(e.Pointer);

			EndAllNoteEditing();
			Focus(FocusState.Programmatic);

			e.Handled = true;
		}

		private void NoteDragHandle_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if (!_isDraggingNote || _draggedNote == null)
			{
				return;
			}

			var currentPoint = e.GetCurrentPoint(CaliperView).Position;
			var dx = currentPoint.X - _lastDragPoint.X;
			var dy = currentPoint.Y - _lastDragPoint.Y;

			MoveNote(_draggedNote, dx, dy);

			_lastDragPoint = currentPoint;
			e.Handled = true;
		}

		private void NoteDragHandle_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			if (sender is Border handle)
			{
				handle.ReleasePointerCaptures();
			}

			_isDraggingNote = false;
			_draggedNote = null;
			e.Handled = true;
		}

		private void MoveNote(NoteEntry entry, double dx, double dy)
		{
			entry.AbsoluteAnchor = new Point(
				entry.AbsoluteAnchor.X + dx,
				entry.AbsoluteAnchor.Y + dy);

			UpdateNoteFrames();
		}

		private void DeleteNoteAt(Point position)
		{
			var index = NoteIndexForContextMenu(position);
			if (index < 0)
			{
				return;
			}

			var entry = _noteEntries[index];
			NotesCanvas.Children.Remove(entry.Container);
			NotesCanvas.Children.Remove(entry.DragHandle);
			_noteEntries.RemoveAt(index);
		}

		private void DeleteAllNotes()
		{
			foreach (var entry in _noteEntries)
			{
				NotesCanvas.Children.Remove(entry.Container);
				NotesCanvas.Children.Remove(entry.DragHandle);
			}
			_noteEntries.Clear();
		}

		private void UpdateNoteBorderVisibility(NoteEntry entry)
		{
			bool showBorder = entry.IsHovering || entry.IsEditing || entry.IsSelected;
			entry.Container.BorderThickness = showBorder ? new Thickness(1) : new Thickness(0);

			entry.Container.Background = entry.IsEditing
				? new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(40, 255, 255, 255))
				: new SolidColorBrush(Colors.Transparent);
		}

		private bool IsBorderVisible(NoteEntry entry)
		{
			return entry.IsHovering || entry.IsEditing || entry.IsSelected;
		}

		private NoteEntry FindNoteByHandle(Border handle)
		{
			foreach (var entry in _noteEntries)
			{
				if (entry.DragHandle == handle)
				{
					return entry;
				}
			}
			return null;
		}

		private int NoteIndexNear(Point p)
		{
			for (int i = 0; i < _noteEntries.Count; i++)
			{
				var entry = _noteEntries[i];
				var x = Canvas.GetLeft(entry.Container);
				var y = Canvas.GetTop(entry.Container);
				var frame = new Rect(x, y, entry.Container.Width, entry.Container.Height);
				var expanded = new Rect(
					frame.X - _noteHitSlop,
					frame.Y - _noteHitSlop,
					frame.Width + (_noteHitSlop * 2),
					frame.Height + (_noteHitSlop * 2));

				if (expanded.Contains(p) && !frame.Contains(p))
				{
					return i;
				}
			}
			return -1;
		}

		private Point ResolveNoteAbsoluteAnchor()
		{
			var position = _rightClickPosition;
			return position;
		}

		private Point NoteAnchorInViewFromAbsoluteAnchor(Point absoluteAnchor)
		{
			return absoluteAnchor;
		}

		private Point NoteOriginInViewFromAnchor(Point anchor)
		{
			return anchor;
		}

		private void UpdateNoteFrames()
		{
			if (_noteEntries.Count == 0)
			{
				return;
			}

			foreach (var entry in _noteEntries)
			{
				var scaledAnchor = NoteAnchorInViewFromAbsoluteAnchor(entry.AbsoluteAnchor);
				var scaledOrigin = NoteOriginInViewFromAnchor(scaledAnchor);

				Canvas.SetLeft(entry.Container, scaledOrigin.X);
				Canvas.SetTop(entry.Container, scaledOrigin.Y);

				Canvas.SetLeft(entry.DragHandle, scaledOrigin.X - _noteHitSlop);
				Canvas.SetTop(entry.DragHandle, scaledOrigin.Y - _noteHitSlop);

				entry.Editor.FontSize = NoteFontSizeForCurrentZoom();


		var frame = new Rect(scaledOrigin.X, scaledOrigin.Y, _defaultNoteSize.Width, _defaultNoteSize.Height);
				var viewport = new Rect(0, 0, EcgImage.ActualWidth, EcgImage.ActualHeight);
				var isVisible = RectsIntersect(frame, viewport);
				entry.Container.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
				entry.DragHandle.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
			}
		}

		private void UpdateNotesCanvasSize()
		{
			NotesCanvas.Width = EcgImage.ActualWidth;
			NotesCanvas.Height = EcgImage.ActualHeight;
		}

		private static bool RectsIntersect(Rect a, Rect b)
		{
			return a.X < b.X + b.Width &&
				   a.X + a.Width > b.X &&
				   a.Y < b.Y + b.Height &&
				   a.Y + a.Height > b.Y;
		}
		private double NoteFontSizeForCurrentZoom()
		{
			return _defaultNoteFontSize;
		}

		private void BeginEditingNote(NoteEntry entry)
		{
			entry.IsEditing = true;
			entry.IsSelected = true;

			entry.Editor.IsReadOnly = false;
			entry.Editor.IsTabStop = true;
			entry.Editor.AllowFocusOnInteraction = true;
			entry.Editor.IsHitTestVisible = true;

			UpdateNoteBorderVisibility(entry);

			DispatcherQueue.TryEnqueue(() =>
			{
				entry.Editor.Focus(FocusState.Programmatic);
			});
		}

		private void EndEditingNote(NoteEntry entry)
		{
			entry.IsEditing = false;
			entry.IsSelected = false;

			entry.Editor.IsReadOnly = true;
			entry.Editor.IsTabStop = false;
			entry.Editor.AllowFocusOnInteraction = false;
			entry.Editor.IsHitTestVisible = false;

			UpdateNoteBorderVisibility(entry);
		}

		private void EndAllNoteEditing()
		{
			foreach (var entry in _noteEntries)
			{
				EndEditingNote(entry);
			}

			// Move focus away from any RichEditBox.
			Focus(FocusState.Programmatic);
		}

		private void AddNote_Click(object sender, RoutedEventArgs e)
		{
			AddNote();

		}

		private void DeleteNote_Click(object sender, RoutedEventArgs e)
		{
			DeleteNoteAt(_rightClickPosition);
		}
		#endregion

		private void ContextMenuFlyout_Closed(object sender, object e)
		{
			if (_contextMenuNote != null && !_contextMenuNote.IsEditing)
			{
				_contextMenuNote.IsSelected = false;
				UpdateNoteBorderVisibility(_contextMenuNote);
				_contextMenuNote = null;
			}
		}
	}
}

