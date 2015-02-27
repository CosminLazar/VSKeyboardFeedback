using System;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.Shell;

namespace CosminLazar.VSKeyboardFeedback.Options
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false)]
    [ComVisible(true)]
    [Guid("F516F904-AB33-4B51-98BF-0E3998AA7E4F")]
    public class OptionsDialogPage : UIElementDialogPage
    {
        protected override UIElement Child
        {
            get
            {
                return new OptionsControl();
            }
        }
    }
}
