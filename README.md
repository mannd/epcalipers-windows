# EP Calipers for Microsoft Windows

## IMPORTANT!!
This is an experimental branch of the EP Calipers for Windows
repository.  The current main branch is the epcalipers-2 branch.  The
epcalipers-2 branch contains the current production code.  However in
the near future this epcalipers-3 branch will become the main branch
going forward, once the code reaches beta status.  At that point the
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
- Update the Help system to Modern Windows style.
- Support dark mode
- MSIX installation via side-loading or Microsoft Store.

## Beta testing
If you are interested in beta-testing this new version, email us at
mannd@epstudiossoftware.com.  When the first beta version is
available, we will contact you.

## Note
The rest of the README for now just duplicates the version 2 README.
It does NOT necessarily reflect the version 3 functionality.

## Description 
This is the Microsoft Windows™ version of EP Calipers.
Versions for Android, iOS and macOS are also available.

EP Calipers provides electronic calipers for making measurements on
images of ECGs or other recordings.  Calculations such as mean
heart rate and corrected QT intervals can be performed directly in the
app.

EP Calipers is open source, and licensed under the
[GNU GPL v3 license.](http://www.gnu.org/licenses/gpl.html).

## Dependencies
The program no longer needs installation of Ghostscript on your computer in order to load PDF files.  Ghostscript support files are included with the program.

Ghostscript is licensed under the [GNU Affero General Public License (AGPL) version 3](https://www.gnu.org/licenses/agpl.html).  Ghostscript source code and other information are available from https://www.ghostscript.com/.

EP Calipers also uses Magick.NET, with source code available at https://github.com/dlemstra/Magick.NET.  Magick.NET is licensed with the [Apache License V2.0](http://apache.org/licenses/LICENSE-2.0.html).

The Apache 2.0 license is [compatible](http://www.apache.org/licenses/GPL-compatibility.html) with the GNU GPL v3 license, as is the AGPL v3 license.  See https://www.gnu.org/licenses/license-list.html#GPLCompatibleLicenses for details.

## Development
All branches of the GitHub repository except for the epcalipers-2
branch are now closed branches and contain code for the first version
of EP Calipers for Windows, ending with version 1.10.  At present all
new development and releases will be on the epcalipers-2 branch, so
this branch is effectively the master or main branch.

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
