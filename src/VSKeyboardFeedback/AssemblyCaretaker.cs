using System;
using System.Reflection;
using Xceed.Wpf.Toolkit;

namespace CosminLazar.VSKeyboardFeedback
{
    public class AssemblyCaretaker
    {
        public static Assembly GetByName(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName)) throw new ArgumentNullException("assemblyName");

            if (assemblyName.IndexOf("Xceed.Wpf.Toolkit", StringComparison.OrdinalIgnoreCase) >= 0)
                return typeof(ColorPicker).Assembly;

            return null;
        }
    }
}