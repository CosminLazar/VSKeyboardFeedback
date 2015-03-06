using System;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;

namespace CosminLazar.VSKeyboardFeedback.Options
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false)]
    [ComVisible(true)]
    [Guid(GuidList.guidVSKeyboardFeedbackOptionsDialogPage)]
    public class OptionsDialogPage : UIElementDialogPage
    {
        private OptionsControl _optionsControl;
        protected override UIElement Child
        {
            get { return (_optionsControl = _optionsControl ?? new OptionsControl(new OptionsControlViewModel(GetOptionsStore()))); }
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);

            if (e.ApplyBehavior == ApplyKind.Apply)
            {
                ((OptionsControlViewModel)_optionsControl.DataContext).Save();
            }
        }

        private IOptionsStore GetOptionsStore()
        {
            var compModel = GetService(typeof(SComponentModel)) as IComponentModel;
            return compModel.DefaultExportProvider.GetExportedValue<IOptionsStore>();
        }
    }
}
