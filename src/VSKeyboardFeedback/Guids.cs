// Guids.cs
// MUST match guids.h
using System;

namespace CosminLazar.VSKeyboardFeedback
{
    static class GuidList
    {
        public const string guidVSKeyboardFeedbackPkgString = "37ac7b0c-2e9a-40b9-9d52-31f41fd42bfa";
        public const string guidVSKeyboardFeedbackCmdSetString = "68739aa0-6017-4f38-a3b9-37aad4befc6c";

        public static readonly Guid guidVSKeyboardFeedbackCmdSet = new Guid(guidVSKeyboardFeedbackCmdSetString);
    };
}