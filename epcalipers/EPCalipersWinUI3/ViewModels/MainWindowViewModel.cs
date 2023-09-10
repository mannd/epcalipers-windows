using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3
{
    public partial class MainWindowViewModel : ObservableObject
    {

        [RelayCommand]
        void Test()
        {
            Debug.Print("test command");
        }

        [RelayCommand]
        void ZoomIn()
        {

        }

        [ObservableProperty]
        private string testText = "Test Text";

        [ObservableProperty]
        private Image mainImage;

        [ObservableProperty]
        private string mainImageSource = "/Assets/Images/sampleECG.jpg";
    }
}
