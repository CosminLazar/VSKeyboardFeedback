using Roccat_Talk.TalkFX;
using Color = System.Windows.Media.Color;

namespace CosminLazar.VSKeyboardFeedback.Options
{
    public class RoccatIskuFxSettings
    {
        public RoccatIskuFxFeedback NoErrors { get; private set; }
        public RoccatIskuFxFeedback Errors { get; private set; }

        public RoccatIskuFxSettings(RoccatIskuFxFeedback noErrors, RoccatIskuFxFeedback errors)
        {
            NoErrors = noErrors;
            Errors = errors;
        }
    }

    public class RoccatIskuFxFeedback
    {
        public KeyEffect Effect { get; set; }
        public Color Color { get; set; }
    }
}