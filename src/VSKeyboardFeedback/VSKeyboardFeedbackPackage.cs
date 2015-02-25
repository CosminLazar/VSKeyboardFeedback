using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CosminLazar.VSKeyboardFeedback
{
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0")]
    [Guid(GuidList.guidVSKeyboardFeedbackPkgString)]
    public sealed class VSKeyboardFeedbackPackage : Package
    {
        private readonly KeyboardCommunicator _keyboardCommunicator;
        private ErrorMonitor _errorMonitor;

        public VSKeyboardFeedbackPackage()
        {
            _keyboardCommunicator = new KeyboardCommunicator();
        }

        protected override void Initialize()
        {
            base.Initialize();

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
                _keyboardCommunicator.Dispose();
            }

            base.Dispose(disposing);
        }

        private void ErrorCheckFinished(bool hasErrors)
        {
            if (hasErrors)
            {
                _keyboardCommunicator.ReportErrors();
            }
            else
            {
                _keyboardCommunicator.ReportNoErrors();
            }
        }
    }
}
