using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Roccat_Talk.TalkFX;
using Color = System.Windows.Media.Color;

namespace CosminLazar.VSKeyboardFeedback.Options
{
    public class OptionsControlViewModel : INotifyPropertyChanged
    {
        private readonly IOptionsStore _optionsStore;
        private IskuFxOptionsViewModel _iskuFxVm;

        public event PropertyChangedEventHandler PropertyChanged;

        public IskuFxOptionsViewModel IskuFxVM
        {
            get { return _iskuFxVm; }
            set
            {
                _iskuFxVm = value;
                OnPropertyChanged();
            }
        }

        public OptionsControlViewModel(IOptionsStore optionsStore)
        {
            _optionsStore = optionsStore;

            IskuFxVM = new IskuFxOptionsViewModel(optionsStore.IskuFxSettings);
        }

        public void Save()
        {
            IskuFxVM.Save();
            _optionsStore.Save();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class IskuFxOptionsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<string> Effects
        {
            get
            {
                yield return KeyEffect.On.ToString();
                yield return KeyEffect.Off.ToString();
                yield return KeyEffect.Blinking.ToString();
                yield return KeyEffect.Breathing.ToString();
                //Heartbeat doesn't seem to work
            }
        }

        public IskuFxFeedbackViewModel Errors { get; private set; }
        public IskuFxFeedbackViewModel NoErrors { get; private set; }

        public IskuFxOptionsViewModel(RoccatIskuFxSettings settings)
        {
            Errors = new IskuFxFeedbackViewModel(settings.Errors);
            NoErrors = new IskuFxFeedbackViewModel(settings.NoErrors);
        }

        public void Save()
        {
            Errors.Save();
            NoErrors.Save();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class IskuFxFeedbackViewModel : INotifyPropertyChanged
    {
        private readonly RoccatIskuFxFeedback _iskuFeedbackSettings;
        public event PropertyChangedEventHandler PropertyChanged;

        private string _effect;
        public string Effect
        {
            get { return _effect; }
            set
            {
                _effect = value;
                OnPropertyChanged();
            }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        public IskuFxFeedbackViewModel(RoccatIskuFxFeedback iskuFeedbackSettings)
        {
            _iskuFeedbackSettings = iskuFeedbackSettings;

            Effect = iskuFeedbackSettings.Effect.ToString();
            Color = _iskuFeedbackSettings.Color;
        }

        public void Save()
        {
            _iskuFeedbackSettings.Effect = (KeyEffect)Enum.Parse(typeof(KeyEffect), Effect);
            _iskuFeedbackSettings.Color = Color;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}