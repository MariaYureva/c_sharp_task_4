using System.Windows;
using ReflectionChessApp.ViewModels;

namespace ReflectionChessApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}