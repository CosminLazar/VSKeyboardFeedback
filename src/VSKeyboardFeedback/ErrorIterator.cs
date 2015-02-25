using System.Collections.Generic;
using Microsoft.VisualStudio.Shell.Interop;

namespace CosminLazar.VSKeyboardFeedback
{
    class ErrorIterator
    {
        private readonly IVsTaskList _vsTaskList;

        public ErrorIterator(IVsTaskList vsTaskList)
        {
            _vsTaskList = vsTaskList;
        }

        public IEnumerable<IVsTaskItem> EnumerateErrors()
        {
            IVsEnumTaskItems taskEnumerator;
            if (_vsTaskList.EnumTaskItems(out taskEnumerator).HasFailed())
                yield break;

            var vsTaskItem = new IVsTaskItem[1];

            while (taskEnumerator.Next(1, vsTaskItem, null).HasWorked())
            {
                var taskItem = vsTaskItem[0];

                if (IsError(taskItem))
                    yield return taskItem;
            }
        }

        private static bool IsError(IVsTaskItem taskItem)
        {
            var errorItem = taskItem as IVsErrorItem;

            if (errorItem == null)
                return false;

            uint errorCategory;
            if (errorItem.GetCategory(out errorCategory).HasFailed())
                return false;

            return errorCategory == (uint)__VSERRORCATEGORY.EC_ERROR;
        }
    }
}