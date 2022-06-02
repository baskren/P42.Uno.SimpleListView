using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ElementType = P42.Uno.SimpleListView.SimpleListView;

namespace P42.Uno.SimpleListView
{
    public static class SimpleListViewMarkupExtensions
    {
        public static TElement SelectionMode<TElement>(this TElement element, ListViewSelectionMode value) where TElement : ElementType
        { element.SelectionMode = value; return element; }

        //public static TElement SwipeEnabled<TElement>(this TElement element, bool value = true) where TElement : ElementType
        //{ element.IsSwipeEnabled = value; return element; }

        public static TElement ItemClickEnabled<TElement>(this TElement element, bool value = true) where TElement : ElementType
        { element.IsItemClickEnabled = value; return element; }

        /*
        public static TElement IncrementalLoadingTrigger<TElement>(this TElement element, IncrementalLoadingTrigger value) where TElement : ElementType
        { element.IncrementalLoadingTrigger = value; return element; }

        public static TElement IncrementalLoadingThreshold<TElement>(this TElement element, double value) where TElement : ElementType
        { element.IncrementalLoadingThreshold = value; return element; }

        public static TElement HeaderTransitions<TElement>(this TElement element, TransitionCollection value) where TElement : ElementType
        { element.HeaderTransitions = value; return element; }

        public static TElement HeaderTemplate<TElement>(this TElement element, DataTemplate value) where TElement : ElementType
        { element.HeaderTemplate = value; return element; }

        public static TElement Header<TElement>(this TElement element, object value) where TElement : ElementType
        { element.Header = value; return element; }

        public static TElement DataFetchSize<TElement>(this TElement element, double value) where TElement : ElementType
        { element.DataFetchSize = value; return element; }

        public static TElement CanReorderItems<TElement>(this TElement element, bool value = true) where TElement : ElementType
        { element.CanReorderItems = value; return element; }

        public static TElement CanDragItems<TElement>(this TElement element, bool value = true) where TElement : ElementType
        { element.CanDragItems = value; return element; }

        public static TElement ShowsScrollingPlaceholders<TElement>(this TElement element, bool value = true) where TElement : ElementType
        { element.ShowsScrollingPlaceholders = value; return element; }

        public static TElement FooterTransitions<TElement>(this TElement element, TransitionCollection value) where TElement : ElementType
        { element.FooterTransitions = value; return element; }

        public static TElement FooterTemplate<TElement>(this TElement element, DataTemplate value) where TElement : ElementType
        { element.FooterTemplate = value; return element; }

        public static TElement Footer<TElement>(this TElement element, object value) where TElement : ElementType
        { element.Footer = value; return element; }

        public static TElement ReorderMode<TElement>(this TElement element, ListViewReorderMode value) where TElement : ElementType
        { element.ReorderMode = value; return element; }

        public static TElement IsMultiSelectCheckBoxEnabled<TElement>(this TElement element, bool value = true) where TElement : ElementType
        { element.IsMultiSelectCheckBoxEnabled = value; return element; }

        public static TElement SingleSelectionFollowsFocus<TElement>(this TElement element, bool value = true) where TElement : ElementType
        { element.SingleSelectionFollowsFocus = value; return element; }

        public static TElement SemanticZoomOwner<TElement>(this TElement element, SemanticZoom value) where TElement : ElementType
        { element.SemanticZoomOwner = value; return element; }

        public static TElement IsZoomedInView<TElement>(this TElement element, bool value = true) where TElement : ElementType
        { element.IsZoomedInView = value; return element; }

        public static TElement IsActiveView<TElement>(this TElement element, bool value = true) where TElement : ElementType
        { element.IsActiveView = value; return element; }


        public static TElement ItemsPanel<TElement>(this TElement element, ItemsPanelTemplate brush) where TElement : ElementType
        { element.ItemsPanel = brush; return element; }
        */

        public static TElement ItemTemplateSetSelector<TElement>(this TElement element, P42.Utils.Uno.DataTemplateSetSelector selector) where TElement : ElementType
        { element.ItemTemplateSelector = selector; return element; }

        public static TElement ItemTemplate<TElement>(this TElement element, DataTemplate brush) where TElement : ElementType
        { element.ItemTemplate = brush; return element; }
        /*
        public static TElement ItemContainerTransitions<TElement>(this TElement element, TransitionCollection value) where TElement : ElementType
        { element.ItemContainerTransitions = value; return element; }

        public static TElement ItemContainerStyleSelector<TElement>(this TElement element, StyleSelector value) where TElement : ElementType
        { element.ItemContainerStyleSelector = value; return element; }

        public static TElement GroupStyleSelector<TElement>(this TElement element, GroupStyleSelector value) where TElement : ElementType
        { element.GroupStyleSelector = value; return element; }

        public static TElement DisplayMemberPath<TElement>(this TElement element, string value) where TElement : ElementType
        { element.DisplayMemberPath = value; return element; }
        */

        /*
        public static TElement SelectedValuePath<TElement>(this TElement element, string value) where TElement : ElementType
        { element.SelectedValuePath = value; return element; }

        public static TElement SelectedValue<TElement>(this TElement element, object value) where TElement : ElementType
        { element.SelectedValue = value; return element; }
        */
        public static TElement SelectedItem<TElement>(this TElement element, object value) where TElement : ElementType
        { element.SelectedItem = value; return element; }

        public static TElement SelectedIndex<TElement>(this TElement element, int value) where TElement : ElementType
        { element.SelectedIndex = value; return element; }

        //public static TElement IsSynchronizedWithCurrentItem<TElement>(this TElement element, bool? value) where TElement : ElementType
        //{ element.IsSynchronizedWithCurrentItem = value; return element; }

        #region Events
        public static TElement AddSelectionChanged<TElement>(this TElement element, SelectionChangedEventHandler handler) where TElement : ElementType
        { element.SelectionChanged += handler; return element; }
        #endregion

    }
}
