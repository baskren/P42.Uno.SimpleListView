using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using P42.Utils.Uno;
using ScrollIntoViewAlignment = Microsoft.UI.Xaml.Controls.ScrollIntoViewAlignment;
using Windows.UI;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace P42.Uno.SimpleListView
{
    public partial class SimpleListView
    {
        ListView _listView = new ListView
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            IsMultiSelectCheckBoxEnabled = false,
        };

        public void PlatformBuild()
        {
            SelectedItems = _listView.SelectedItems;

            var containerStyle = new Style
            {
                TargetType = typeof(ListViewItem),
                Setters = {
                    new Setter(ListViewItem.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch),
                    new Setter(ListViewItem.VerticalContentAlignmentProperty, VerticalAlignment.Stretch),
                    new Setter(ListViewItem.MarginProperty, new Thickness(0)),
                    new Setter(ListViewItem.PaddingProperty, new Thickness(0)),
                    //new Setter(ListViewItemPresenter.PointerOverBackgroundProperty, Colors.Transparent.ToBrush())
                }
            };
            _listView.ItemContainerStyle = containerStyle;

            //_listView.Bind(ListView.IsItemClickEnabledProperty, this, nameof(IsItemClickEnabled));
            //_listView.Bind(ListView.SelectionModeProperty, this, nameof(SelectionMode));
            //_listView.Bind(ListView.SelectedIndexProperty, this, nameof(SelectedIndex));
            //_listView.Bind(ListView.SelectedItemProperty, this, nameof(SelectedItem));
            _listView.ItemClick += OnListView_ItemClick;
            _listView.SelectionChanged += OnListView_SelectionChanged;

            Content = _listView;
        }

        private static void OnIsItemClickEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView._listView.IsItemClickEnabled = listView.IsItemClickEnabled;
        }

        private static void OnFooterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView._listView.Footer = listView.Footer;
        }

        private static void OnFooterTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView._listView.FooterTemplate = listView.FooterTemplate;
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView._listView.Header = listView.Header;
        }

        private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView._listView.HeaderTemplate = listView.HeaderTemplate;
        }


        private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView._listView.SelectionMode = listView.SelectionMode;
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView._listView.SelectedItem = listView.SelectedItem;
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView._listView.SelectedIndex = listView.SelectedIndex;
        }


        private void OnListView_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            if (SelectionMode == ListViewSelectionMode.None)
            {
                SelectedItem = null;
                return;
            }

            if (SelectionMode == ListViewSelectionMode.Single)
            {
                if (e.AddedItems?.FirstOrDefault() is object addedItem && addedItem != null)
                {
                    if (!addedItem.Equals(SelectedItem))
                        SelectedItem = addedItem;
                }
                else if (e.RemovedItems?.FirstOrDefault() is object removedItem && removedItem != null)
                    _listView.SelectedItem = null;
            }

            SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(this, e.RemovedItems?.ToList(), e.AddedItems?.ToList()));
        }

        private void OnListView_ItemClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            var item = e.ClickedItem;
            var container = (ListViewItem)_listView.ContainerFromItem(item);
            var cellElement = (FrameworkElement)container.ContentTemplateRoot;

            ItemClick?.Invoke(this, new ItemClickEventArgs(this, e.ClickedItem, cellElement));
        }

        private static void OnItemsSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is SimpleListView simpleListView)
                simpleListView._listView.ItemsSource = simpleListView.ItemsSource;
        }

        private static void OnItemTemplateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is SimpleListView simpleListView)
                simpleListView._listView.ItemTemplate = simpleListView.ItemTemplate;
        }

        private static void OnItemTemplateSelectorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is SimpleListView simpleListView)
                simpleListView._listView.ItemTemplateSelector = simpleListView.ItemTemplateSelector;
        }


        #region ListViewBase Methods


/* removed because it might work better with ScrollViewer.ChangeView(X, Y, Zoom, true);
public async Task ScrollIntoView(object item, ScrollIntoViewAlignment alignment)
{
    await _listView.ScrollToAsync(item, alignment.AsScrollToPosition(), true);
}
*/

public void SelectAll()
{
    _listView.SelectAll();
}

#endregion

}
}
