using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        [ObservableProperty]
        private string testText = "Test Text";
    }
}
