namespace CosminLazar.VSKeyboardFeedback
{
    class HResult
    {
        public const int S_OK = 0;
        public const int S_FALSE = 1;
    }

    static class HResultExtensions
    {
        public static bool HasFailed(this int self)
        {
            return self == HResult.S_FALSE;
        }

        public static bool HasWorked(this int self)
        {
            return self == HResult.S_OK;
        }
    }
}
