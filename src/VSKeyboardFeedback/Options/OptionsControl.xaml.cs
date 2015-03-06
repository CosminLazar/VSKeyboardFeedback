using System.Windows.Controls;

namespace CosminLazar.VSKeyboardFeedback.Options
{
    /// <summary>
    /// Interaction logic for OptionsControl.xaml
    /// </summary>
    public partial class OptionsControl : UserControl
    {
        public OptionsControl()
        {
            InitializeComponent();
        }

        public OptionsControl(OptionsControlViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}
