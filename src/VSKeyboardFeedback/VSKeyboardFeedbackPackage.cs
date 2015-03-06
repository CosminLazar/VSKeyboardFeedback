using System;
using System.Runtime.InteropServices;
using CosminLazar.VSKeyboardFeedback.Options;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CosminLazar.VSKeyboardFeedback
{
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0")]
    [Guid(GuidList.guidVSKeyboardFeedbackPkgString)]
    [ProvideOptionPage(typeof(OptionsDialogPage), Constants.ApplicationName, "Settings", 0, 0, supportsAutomation: true)]
    public sealed class VSKeyboardFeedbackPackage : Package
    {
        private IskuFxKeyboardCommunicator _iskuFxKeyboardCommunicator;
        private ErrorMonitor _errorMonitor;

        static VSKeyboardFeedbackPackage()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => AssemblyCaretaker.GetByName(args.Name);
        }

        protected override void Initialize()
        {
            base.Initialize();

            _iskuFxKeyboardCommunicator = new IskuFxKeyboardCommunicator(GetOptionsStore().IskuFxSettings);

            var taskList = GetGlobalService(typeof(SVsErrorList)) as IVsTaskList;
            _errorMonitor = new ErrorMonitor(taskList);
            _errorMonitor.ErrorCheckFinished += ErrorCheckFinished;
            _errorMonitor.BeginMonitoring();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _errorMonitor.EndMonitoring();
                _errorMonitor.ErrorCheckFinished -= ErrorCheckFinished;
                _errorMonitor.Dispose();
                _iskuFxKeyboardCommunicator.Dispose();
            }

            base.Dispose(disposing);
        }

        private void ErrorCheckFinished(bool hasErrors)
        {
            if (hasErrors)
            {
                _iskuFxKeyboardCommunicator.ReportErrors();
            }
            else
            {
                _iskuFxKeyboardCommunicator.ReportNoErrors();
            }
        }

        private IOptionsStore GetOptionsStore()
        {
            var compModel = GetService(typeof(SComponentModel)) as IComponentModel;
            return compModel.DefaultExportProvider.GetExportedValue<IOptionsStore>();
        }
    }
}
