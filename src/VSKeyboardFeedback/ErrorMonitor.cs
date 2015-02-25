using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;
using ActivityLog = Microsoft.VisualStudio.Shell.ActivityLog;

namespace CosminLazar.VSKeyboardFeedback
{
    class ErrorMonitor : IDisposable
    {
        private readonly ErrorIterator _errorIterator;
        private readonly TaskScheduler _taskScheduler;
        private CancellationTokenSource _stopMonitoringToken;

        public event Action<bool> ErrorCheckFinished;

        public ErrorMonitor(IVsTaskList vsTaskList)
        {
            _errorIterator = new ErrorIterator(vsTaskList);
            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public void BeginMonitoring(int sampleIntervalMs = 1000)
        {
            EndMonitoring();

            _stopMonitoringToken = new CancellationTokenSource();

            ScheduleErrorCheck(sampleIntervalMs);
        }

        public void EndMonitoring()
        {
            if (_stopMonitoringToken != null)
            {
                _stopMonitoringToken.Cancel();
            }
        }

        public void Dispose()
        {
            if (_stopMonitoringToken != null)
            {
                _stopMonitoringToken.Dispose();
            }
        }

        private void ScheduleErrorCheck(int sampleIntervalMs)
        {
            var errorCheckTask =
                Task.Delay(sampleIntervalMs)
                .ContinueWith(_ => CheckForErrors(), _stopMonitoringToken.Token, TaskContinuationOptions.None, _taskScheduler);

            errorCheckTask.ContinueWith(task =>
            {
                Debug.Assert(task.Exception != null, "A failed task should have an exception");
                task.Exception.Flatten().Handle(e =>
                {
                    ActivityLog.LogWarning("VSKeyboardFeedback", string.Format("Got {0} error while checking the error tasks", e.GetType()));
                    return true;
                });
            }, TaskContinuationOptions.OnlyOnFaulted);

            errorCheckTask.ContinueWith(_ => ScheduleErrorCheck(sampleIntervalMs), _stopMonitoringToken.Token);
        }

        private void CheckForErrors()
        {
            var hasErrors = _errorIterator.EnumerateErrors().Any();

            OnErrorCheckFinished(hasErrors);
        }

        protected virtual void OnErrorCheckFinished(bool hasErrors)
        {
            var handlers = ErrorCheckFinished;

            if (handlers != null)
                handlers(hasErrors);
        }
    }
}