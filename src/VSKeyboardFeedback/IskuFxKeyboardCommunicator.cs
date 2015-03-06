using System;
using System.Windows.Media;
using CosminLazar.VSKeyboardFeedback.Options;
using Roccat_Talk.TalkFX;
using Color = Roccat_Talk.TalkFX.Color;

namespace CosminLazar.VSKeyboardFeedback
{
    class IskuFxKeyboardCommunicator : IDisposable
    {
        private readonly RoccatIskuFxSettings _settings;
        private readonly TalkFxConnection _keyboardConnection;
        private KeyboardState _currentKeyboardFeedback;

        public IskuFxKeyboardCommunicator(RoccatIskuFxSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");

            _settings = settings;
            _keyboardConnection = new TalkFxConnection();
        }

        public void ReportNoErrors()
        {
            SendFeedback(KeyboardState.FromFeedback(_settings.NoErrors));
        }

        public void ReportErrors()
        {
            SendFeedback(KeyboardState.FromFeedback(_settings.Errors));
        }

        public void Dispose()
        {
            _keyboardConnection.RestoreLedRgb();
            _keyboardConnection.Dispose();
        }

        private void SendFeedback(KeyboardState state)
        {
            if (state.Equals(_currentKeyboardFeedback))
                return;

            _keyboardConnection.SetLedRgb(KeyboardState.Zone, state.Effect, KeyboardState.Speed, state.Color);
            _currentKeyboardFeedback = state;
        }

        struct KeyboardState
        {
            public static Zone Zone { get { return Zone.Event; } }
            public static Speed Speed { get { return Speed.Normal; } }
            public KeyEffect Effect { get; private set; }
            public Color Color { get; private set; }

            public static KeyboardState FromFeedback(RoccatIskuFxFeedback feedback)
            {
                if (feedback.Effect == KeyEffect.Off)
                    return KeyboardOff();

                return new KeyboardState
                {
                    Color = ConvertColor(feedback.Color),
                    Effect = feedback.Effect
                };
            }

            private static KeyboardState KeyboardOff()
            {
                return new KeyboardState
                {
                    Color = ConvertColor(Colors.Black),
                    Effect = KeyEffect.On
                };
            }

            private static Color ConvertColor(System.Windows.Media.Color feedbackColor)
            {
                return new Color(feedbackColor.R, feedbackColor.G, feedbackColor.B);
            }
        }
    }
}