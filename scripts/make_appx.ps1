cd "C:\Program Files (x86)\Windows Kits\10\bin\x64"
.\makeappx.exe pack /d C:\Users\mannd\Documents\epcalipers-app\epcalipers\PackageFiles\ /p C:\Users\mannd\Documents\epcalipers-app\epcalipers\epcalipers
.\signtool.exe sign -f my.pfx -fd SHA256 -v C:\Users\mannd\Documents\epcalipers-app\epcalipers\epcalipers.appx
cd "C:\Users\mannd\git\epcalipers-windows\scripts"