using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using Roccat_Talk.TalkFX;
using Color = System.Windows.Media.Color;

namespace CosminLazar.VSKeyboardFeedback.Options
{
    [Export(typeof(IOptionsStore))]
    public class OptionsStore : IOptionsStore
    {
        private const string IskuFxNoErrorsCollection = Constants.ApplicationName + "\\IskuFxSettings\\NoErrors";
        private const string IskuFxErrorsCollection = Constants.ApplicationName + "\\IskuFxSettings\\Errors";

        private readonly WritableSettingsStore _writableSettingsStore;

        [ImportingConstructor]
        public OptionsStore(SVsServiceProvider vsServiceProvider)
        {
            var shellSettingsManager = new ShellSettingsManager(vsServiceProvider);
            _writableSettingsStore = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            Load();
        }

        private RoccatIskuFxSettings _iskuFxSettings;
        public RoccatIskuFxSettings IskuFxSettings
        {
            get
            {
                if (_iskuFxSettings == null)
                {
                    Load();
                }

                return (_iskuFxSettings = _iskuFxSettings ?? DefaultIskuFxSettings());
            }
        }

        private void Load()
        {
            try
            {
                var noErrors = GetFeedbackSettings(IskuFxNoErrorsCollection);
                var errors = GetFeedbackSettings(IskuFxErrorsCollection);

                if (noErrors != null || errors != null)
                {
                    _iskuFxSettings = new RoccatIskuFxSettings(noErrors, errors);
                }
            }
            catch (Exception ex)
            {
                ActivityLog.LogWarning(Constants.ApplicationName, string.Format("Could not load user settings, error: {0}", ex));
            }
        }

        public void Save()
        {
            try
            {
                SaveFeedbackSettings(IskuFxNoErrorsCollection, IskuFxSettings.NoErrors);
                SaveFeedbackSettings(IskuFxErrorsCollection, IskuFxSettings.Errors);
            }
            catch (Exception ex)
            {
                ActivityLog.LogWarning(Constants.ApplicationName, string.Format("Could not save user settings, error: {0}", ex));
            }
        }

        private void SaveFeedbackSettings(string collection, RoccatIskuFxFeedback settings)
        {
            if (!_writableSettingsStore.CollectionExists(collection))
                _writableSettingsStore.CreateCollection(collection);

            _writableSettingsStore.SetString(collection, "Effect", settings.Effect.ToString());
            _writableSettingsStore.SetString(collection, "Color", string.Join(",", new[] { settings.Color.R, settings.Color.G, settings.Color.B }));
        }

        private RoccatIskuFxFeedback GetFeedbackSettings(string collection)
        {
            if (!_writableSettingsStore.CollectionExists(collection))
                return null;

            var effect = (KeyEffect)Enum.Parse(typeof(KeyEffect), _writableSettingsStore.GetString(collection, "Effect"));
            var rgbColors = _writableSettingsStore.GetString(collection, "Color").Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToByte(x)).ToArray();

            var color = Color.FromRgb(rgbColors[0], rgbColors[1], rgbColors[2]);

            return new RoccatIskuFxFeedback
            {
                Effect = effect,
                Color = color
            };
        }

        public static RoccatIskuFxSettings DefaultIskuFxSettings()
        {
            var noErrors = new RoccatIskuFxFeedback { Color = Colors.Green, Effect = KeyEffect.On };
            var errors = new RoccatIskuFxFeedback { Color = Colors.Red, Effect = KeyEffect.On };

            return new RoccatIskuFxSettings(noErrors, errors);
        }
    }
}