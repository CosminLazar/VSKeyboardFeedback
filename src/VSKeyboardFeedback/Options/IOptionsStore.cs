namespace CosminLazar.VSKeyboardFeedback.Options
{
    public interface IOptionsStore
    {
        RoccatIskuFxSettings IskuFxSettings { get; }
        void Save();
    }    
}
