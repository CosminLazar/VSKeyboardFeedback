using System;
using Roccat_Talk.TalkFX;

namespace CosminLazar.VSKeyboardFeedback
{
    class KeyboardCommunicator : IDisposable
    {
        private readonly TalkFxConnection _keyboardConnection;
        private bool? _isTheKeyboardReportingErrors;

        public KeyboardCommunicator()
        {
            _keyboardConnection = new TalkFxConnection();
        }

        public void ReportNoErrors()
        {
            if (KeyboardIsAlreadyReportingNoErrors())
                return;

            _keyboardConnection.SetLedRgb(Zone.Event, KeyEffect.On, Speed.Normal, new Color(0, 255, 0));
            _isTheKeyboardReportingErrors = false;
        }

        public void ReportErrors()
        {
            if (KeyboardIsAlreadyReportingErrors())
                return;

            _keyboardConnection.SetLedRgb(Zone.Event, KeyEffect.On, Speed.Normal, new Color(255, 0, 0));
            _isTheKeyboardReportingErrors = true;
        }

        public void Dispose()
        {
            _keyboardConnection.RestoreLedRgb();
            _keyboardConnection.Dispose();
        }

        private bool KeyboardIsAlreadyReportingNoErrors()
        {
            return _isTheKeyboardReportingErrors.HasValue && !_isTheKeyboardReportingErrors.Value;
        }

        private bool KeyboardIsAlreadyReportingErrors()
        {
            return _isTheKeyboardReportingErrors.HasValue && _isTheKeyboardReportingErrors.Value;
        }
    }
}