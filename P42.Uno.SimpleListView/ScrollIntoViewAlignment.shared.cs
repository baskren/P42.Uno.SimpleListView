using P42.Utils.Uno;
using System;
using System.Collections.Generic;
using System.Text;

namespace P42.Uno.SimpleListView
{
    public enum ScrollIntoViewAlignment
    {
        Default,
        Leading,
        Center,
        Trailing
    }

    static class ScrollIntoViewAlignmentExtensions
    {
        public static ScrollToPosition AsScrollToPosition(this ScrollIntoViewAlignment alignment)
        {
            switch (alignment)
            {
                case ScrollIntoViewAlignment.Leading:
                    return ScrollToPosition.Start;
                case ScrollIntoViewAlignment.Center:
                    return ScrollToPosition.Center;
                case ScrollIntoViewAlignment.Trailing:
                    return ScrollToPosition.End;
                default:
                    return ScrollToPosition.MakeVisible;
            }
        }
    }
}
