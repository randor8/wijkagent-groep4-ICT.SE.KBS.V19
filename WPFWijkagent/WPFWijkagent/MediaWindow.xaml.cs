using System.Collections.Generic;
using System.Windows;
using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for MediaWindow.xaml
    /// </summary>
    public partial class MediaWindow : Window
    {
        public MediaWindow(List<SocialMediaImage> images)
        {
            InitializeComponent();
            wpfLVImages.ItemsSource = images;
        }
    }
}
