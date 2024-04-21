# EP Calipers for Microsoft Windows

## IMPORTANT!!
The epcalipers-3 branch is now the default branch for this repo.

However, epcalipers-3 is still in beta.  The
epcalipers-2 branch contains the current production code.  The
epcalipers-2 branch will continue to be maintained to support current
users with older versions of Windows.

This epcalipers-3 branch is a rewrite nearly from scratch of EP
Calipers.  It is still under development, and we are still adding
functionality to get it to the same level of functionality as the
epcalipers-2 branch.

The goals of this new branch are to recreate the app with the
following improvements:
- Target Windows 10/11.
- Use the most modern Windows desktop system, WinUI 3.
- Improve the archecture of the app (XAML views, MVVM).
- Eliminate PDF processing via Ghostscript, and use Nuget packages instead.
- Support dark mode
- MSIX installation via the Microsoft Store.

## Description 
This is the Microsoft Windows™ version of EP Calipers.
Versions for Android, iOS and macOS are also available.

The WinUI 3 version of EP Calipers is called EP Calipers 3, to
distinguish it from the older version that used Windows Forms and WPF,
which will just continued to be called EP Calipers.

EP Calipers 3 provides electronic calipers for making measurements on
images of ECGs or other recordings.  Calculations such as mean
heart rate and corrected QT intervals can be performed directly in the
app.

EP Calipers 3 is open source, and licensed under the
[GNU GPL v3 license.](http://www.gnu.org/licenses/gpl.html).

## Dependencies
The app uses .NET 8 and WinUI 3, but will be published as a
self-contained package, meaning it should not be necessary to install
either of these on your computer.  The target OS is Windows 11 or
later, though it should run on later versions of Windows 10
(10.0.22621.0 or later).

## Development
All branches of the GitHub repository except for the epcalipers-3
and epcalipers-2 branches are now closed branches.  At present all
new development and releases will be on the epcalipers-3 branch, so
this branch is effectively the master or main branch.  However the
epcalipers-2 branch will continue to be maintained.

## Acknowledgments
The Brugadometer is based on the work of Dr. Adrian Baranchuk and his colleagues at Queen's University Kingston, Ontario, Canada. It is used with Dr. Baranchuk's permission and encouragement. Please see [this article](http://europace.oxfordjournals.org/content/16/11/1639) for more information.

Thanks to Dr. Michael Katz for the original concept, to Scott Krankkala for the idea behind marching calipers, and to Fred Cohen for helping to troubleshoot the application.

The screen capture code is from https://github.com/robmikh/WinUI3CaptureSample, covered under the MIT license.

## Copyright
Copyright © 2016-2024 [EP Studios, Inc.](https://www.epstudiossoftware.com)

## Author
David Mann, MD

Email: [mannd@epstudiossoftware.com](mailto:mannd@epstudiossoftware.com) 
Website: [epstudiossoftware.com](https://www.epstudiossoftware.com) 
