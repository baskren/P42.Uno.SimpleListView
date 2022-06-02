using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using Uno.Extensions.Specialized;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using P42.Utils.Uno;
using Microsoft.UI;
using ScrollIntoViewAlignment = Microsoft.UI.Xaml.Controls.ScrollIntoViewAlignment;
using System.Collections.ObjectModel;
using Android.Views;
using Android.Runtime;
using Android.Util;
using System.Threading.Tasks;

namespace P42.Uno.SimpleListView
{
    public partial class SimpleListView
    {
        static double _scale = -1;
        internal static double DisplayScale
        {
            get
            {
                if (_scale > 0)
                    return _scale;
                using var displayMetrics = new DisplayMetrics();
                using var service = global::Uno.UI.ContextHelper.Current.GetSystemService(Android.Content.Context.WindowService);
                using var windowManager = service?.JavaCast<IWindowManager>();
                var display = windowManager?.DefaultDisplay;
                display?.GetRealMetrics(displayMetrics);
                _scale = (double)displayMetrics?.Density;
                return _scale;
            }
        }

        static int instances = 0;
        int instance;
        internal ObservableCollection<object> _selectedItems = new ObservableCollection<object>();
        internal ObservableCollection<int> NativeCellHeights = new ObservableCollection<int>();
        Android.Views.View _headerView;
        Android.Views.View _footerView;

        Android.Widget.ListView _nativeListView;

        SimpleAdapter _adapter;

        public void PlatformBuild()
        {
            P42.Utils.Profile.Enter();
            instance = instances++;
            SelectedItems = _selectedItems;
            _selectedItems.CollectionChanged += OnSelectedItems_CollectionChanged;
            NativeCellHeights.CollectionChanged += OnNativeCellHeights_CollectionChanged;

            //Loaded += SimpleListView_Loaded;
            InjectNativeListView();
            P42.Utils.Profile.Exit();
        }
        /*
        async void SimpleListView_Loaded(object sender, RoutedEventArgs e)
        {
            //await Task.Delay(3000);
            InjectNativeListView();
        }
        */
        void InjectNativeListView()
        {
            if (ItemsSource is null)
                return;

            P42.Utils.Profile.Enter();
            if (_nativeListView != null)
            {
                _adapter?.NotifyDataSetInvalidated();
                _nativeListView.Adapter = null;
                _adapter?.Dispose();
                _nativeListView.Dispose();
            }
            P42.Utils.Profile.Mark("A");

            _adapter = new SimpleAdapter(this);
            _nativeListView = new Android.Widget.ListView(global::Uno.UI.ContextHelper.Current)
            {
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent),
                Divider = null,
                Adapter = _adapter,
            };
            P42.Utils.Profile.Mark("B");

            Content = VisualTreeHelper.AdaptNative(_nativeListView);
            //((FrameworkElement)Content).InvalidateMeasure();
            P42.Utils.Profile.Exit();
        }

        #region Header / Footer Change Handlers
        void UpdateFooter()
        {
            if (_footerView != null)
                _nativeListView.RemoveFooterView(_footerView);
            _footerView?.Dispose();
            _footerView = null;
            if (Footer != null)
            {
                if (Footer is Android.Views.View view)
                {
                    _footerView = view;
                    _nativeListView.AddFooterView(view);
                }
                else if (FooterTemplate?.LoadContent() is FrameworkElement newFooter)
                {
                    _footerView = newFooter;
                    newFooter.DataContext = Footer;
                    _nativeListView.AddFooterView(newFooter);
                }
            }
        }

        private static void OnFooterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView.UpdateFooter();
        }

        private static void OnFooterTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView.UpdateFooter();
        }

        void UpdateHeader()
        {
            if (_headerView != null)
                _nativeListView.RemoveHeaderView(_headerView);
            _headerView?.Dispose();
            _headerView = null;
            if (Header != null)
            {
                if (Header is Android.Views.View view)
                {
                    _headerView = view;
                    _nativeListView.AddHeaderView(view);
                }
                else if (HeaderTemplate?.LoadContent() is FrameworkElement newHeader)
                {
                    _headerView = newHeader;
                    newHeader.DataContext = Header;
                    _nativeListView.AddHeaderView(newHeader);
                }
            }
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView.UpdateHeader();
        }

        private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
                listView.UpdateHeader();
        }
        #endregion


        #region Click / Selection Change Handlers
        private static void OnIsItemClickEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimpleListView listView)
            {
                listView.SelectItem(listView.SelectedItem);
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        bool _repondingToSelectedItemsCollectionChanged;
        private void OnSelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _repondingToSelectedItemsCollectionChanged = true;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    if (e.NewItems?.Cast<object>().Any() ?? false)
                        SelectedItem = e.NewItems[0];
                    else if (SelectedItem != null && (e.OldItems?.Contains(SelectedItem) ?? false))
                        SelectedItem = null;
                    SelectionChanged?.Invoke(this, new P42.Uno.SimpleListView.SelectionChangedEventArgs(this, e.OldItems, e.NewItems));
                    break;
                case NotifyCollectionChangedAction.Move:
                default:
                    break;
            }
            _repondingToSelectedItemsCollectionChanged = false;
        }

        internal async Task OnWrapperClicked(CellWrapper wrapper)
        {
            System.Diagnostics.Debug.WriteLine("SimpleListView. CLICK");
            SelectItem(wrapper.DataContext);
            await Task.Delay(10);
            if (IsItemClickEnabled)
                ItemClick?.Invoke(this, new ItemClickEventArgs(this, wrapper.DataContext, wrapper.Child));
        }

        void SelectItem(object item)
        {
            if (_repondingToSelectedItemsCollectionChanged)
                return;
            if (SelectionMode == ListViewSelectionMode.Single)
            {
                if (!_selectedItems.Contains(item))
                {
                    _selectedItems.Insert(0, item);
                    if (_selectedItems.Last() is object last)
                        _selectedItems.Remove(last);
                    //_selectedItems.Clear();
                    //_selectedItems.Add(item);
                }
            }
            else if (SelectionMode == ListViewSelectionMode.Multiple)
            {
                if (_selectedItems.Contains(item))
                    _selectedItems.Remove(item);
                else
                    _selectedItems.Add(item);
            }
        }
        #endregion


        #region Source / Template Change Handlers
        private static void OnItemsSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is SimpleListView simpleListView)
            {
                //simpleListView._adapter.SetItems(simpleListView.ItemsSource);
                simpleListView.InjectNativeListView();
            }
        }

        private static void OnItemTemplateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is SimpleListView simpleListView)
                simpleListView.InjectNativeListView();
        }

        private static void OnItemTemplateSelectorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is SimpleListView simpleListView)
                simpleListView.InjectNativeListView();
        }
        #endregion


        #region ListViewBase Methods

        int _waitingForIndex = -1;
        ScrollIntoViewAlignment _waitingAlignment = ScrollIntoViewAlignment.Default;
        public async Task ScrollIntoView(object item, P42.Uno.SimpleListView.ScrollIntoViewAlignment alignment)
        {
            if (ItemsSource.Cast<object>().ToList().IndexOf(item) is int index && index > -1)
            {
                if (alignment == ScrollIntoViewAlignment.Default)
                {
                    _nativeListView.SmoothScrollToPosition(index);
                    return;
                }
                else if (alignment == ScrollIntoViewAlignment.Leading)
                {
                    _nativeListView.SmoothScrollToPositionFromTop(index, 0);
                    return;
                }
                //var viewHeight = _nativeListView.Height;
                if (index < NativeCellHeights.Count)
                {
                    var cellHeight = NativeCellHeights[index];
                    InternalScrollTo(index, alignment, cellHeight);
                    /*
                    if (alignment == ScrollIntoViewAlignment.Center)
                        _nativeListView.SmoothScrollToPositionFromTop(index, (viewHeight - cellHeight) / 2);
                    else
                        _nativeListView.SmoothScrollToPositionFromTop(index, viewHeight - cellHeight);
                    */
                }
                else
                {
                    var estCellHeight = (int)(NativeCellHeights.Average() + 0.5);
                    _waitingForIndex = index;
                    _waitingAlignment = alignment;
                    InternalScrollTo(index, alignment, estCellHeight);
                    /*
                    if (alignment == ScrollIntoViewAlignment.Center)
                        _nativeListView.SmoothScrollToPositionFromTop(index, (viewHeight - estCellHeight) / 2);
                    else
                        _nativeListView.SmoothScrollToPositionFromTop(index, viewHeight - estCellHeight);
                    */
                }
                await Task.Delay(10);
            }
        }

        private void OnNativeCellHeights_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_waitingForIndex >= e.NewStartingIndex && _waitingForIndex < e.NewStartingIndex + e.NewItems.Count)
            {
                var index = _waitingForIndex;
                _waitingForIndex = -1;
                var cellHeight = NativeCellHeights[index];
                /*
                var viewHeight = _nativeListView.Height;
                if (_waitingAlignment == ScrollIntoViewAlignment.Trailing)
                    _nativeListView.SmoothScrollToPositionFromTop(index, viewHeight - cellHeight);
                else if (_waitingAlignment == ScrollIntoViewAlignment.Center)
                    _nativeListView.SmoothScrollToPositionFromTop(index, (viewHeight - cellHeight) / 2);
                else if (_waitingAlignment == ScrollIntoViewAlignment.Default ||
                    _waitingAlignment == ScrollIntoViewAlignment.Leading)
                    _nativeListView.SmoothScrollToPosition(index);
                */
                InternalScrollTo(index, _waitingAlignment, cellHeight);
                _waitingAlignment = ScrollIntoViewAlignment.Default;
            }
        }

        void InternalScrollTo(int index, ScrollIntoViewAlignment alignment, int cellHeight)
        {
            var viewHeight = _nativeListView.Height;
            if (_waitingAlignment == ScrollIntoViewAlignment.Trailing)
                _nativeListView.SmoothScrollToPositionFromTop(index, viewHeight - cellHeight);
            else if (_waitingAlignment == ScrollIntoViewAlignment.Center)
                _nativeListView.SmoothScrollToPositionFromTop(index, (viewHeight - cellHeight) / 2);
            else if (_waitingAlignment == ScrollIntoViewAlignment.Default ||
                _waitingAlignment == ScrollIntoViewAlignment.Leading)
                _nativeListView.SmoothScrollToPosition(index);
        }

        public void SelectAll()
        {
            var items = ItemsSource.Cast<object>().ToArray();
            foreach (var item in items)
            {
                if (!SelectedItems.Contains(item))
                    SelectedItems.Add(item);
            }
        }

        #endregion



    }

    class SimpleAdapter : Android.Widget.BaseAdapter<object>
    {
        IEnumerable<object> Items;
        DataTemplate ItemTemplate => SimpleListView.ItemTemplate;
        P42.Utils.Uno.DataTemplateSetSelector TemplateSelector => SimpleListView.ItemTemplateSelector;
        SimpleListView SimpleListView;
        //List<DataTemplate> Templates = new List<DataTemplate>();

        public SimpleAdapter(SimpleListView simpleListView)
        {
            P42.Utils.Profile.Enter();
            SimpleListView = simpleListView;
            SetItems(SimpleListView.ItemsSource.Cast<object>());
            P42.Utils.Profile.Exit();
        }

        public void SetItems(IEnumerable<object> items)
        {
            P42.Utils.Profile.Enter();

            if (items != Items)
            {
                if (Items is INotifyCollectionChanged oldCollection)
                    oldCollection.CollectionChanged -= OnCollectionChaged;
                Items = items;
                if (Items is INotifyCollectionChanged newCollection)
                    newCollection.CollectionChanged += OnCollectionChaged;
                NotifyDataSetChanged();
            }
            P42.Utils.Profile.Exit();
        }

        private void OnCollectionChaged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged();
        }



        public override object this[int position]
        {
            get
            {
                if (position > -1 && position <= Items.Count())
                    return Items?.ElementAt(position);
                return null;
            }
        }

        public override int Count 
            => Items?.Count() ?? 0;

        public override long GetItemId(int position) => position;

        public override int ViewTypeCount
        {
            get
            {
                if (ItemTemplate != null || TemplateSelector is null)
                    return 1;
                return TemplateSelector.Count() + 1;
            }
        }

        public override int GetItemViewType(int position)
        {
            P42.Utils.Profile.Enter();

            if (TemplateSelector?.SelectDataTemplateSet(this[position]) is DataTemplateSet set)
            {
                P42.Utils.Profile.Exit("EXIT A");
                return TemplateSelector.IndexOf(set) + 1;
            }
            P42.Utils.Profile.Exit();
            return 0;
        }

        // ALWAYS SET INDEX BEFORE DATACONTEXT
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            P42.Utils.Profile.Enter();

            if (convertView is CellWrapper wrapper)
            {
                wrapper.Index = position;
                wrapper.DataContext = this[position];
                P42.Utils.Profile.Exit("A");
                return convertView;
            }
            P42.Utils.Profile.Mark("B");

            if (ItemTemplate?.LoadContent() is FrameworkElement newElement)
            {
                P42.Utils.Profile.Mark("B.1");

                var cw = new CellWrapper(SimpleListView)
                {
                    Child = newElement,
                    Index = position,
                };
                P42.Utils.Profile.Mark("B.2");

                cw.DataContext = this[position];

                P42.Utils.Profile.Exit("EXIT B");
                return cw;
            }
            P42.Utils.Profile.Mark("C");

            if (TemplateSelector?.SelectDataTemplateSet(this[position]) is DataTemplateSet set)
            {
                P42.Utils.Profile.Mark("C.1");

                if (set.Constructor.Invoke() is FrameworkElement newSelectedElement)
                {
                    P42.Utils.Profile.Mark("C.2");

                    var cw = new CellWrapper(SimpleListView)
                    {
                        Child = newSelectedElement,
                        Index = position,
                    };
                    P42.Utils.Profile.Mark("C.3");

                    cw.DataContext = this[position];

                    P42.Utils.Profile.Exit("EXIT C");
                    return cw;
                }
            }
            P42.Utils.Profile.Mark("D");

            var w = new CellWrapper(SimpleListView)
            {
                Child = new Cell(),
                Index = position,
            };
            P42.Utils.Profile.Mark("D.1");
            w.DataContext = this[position];

            P42.Utils.Profile.Exit("EXIT D");
            return w;
        }

    }

    partial class CellWrapper : Border
    {
        SimpleListView SimpleListView;

        public bool IsSelected => DataContext is null
                ? false
                : (SimpleListView?._selectedItems?.Contains(DataContext) ?? false);

        int _index;
        public int Index 
        { 
            get => _index; 
            set
            {
                if (_index != value)
                {
                    _index = value;
                    UpdateSelection();
                }
            }
        }

        public CellWrapper(SimpleListView simpleListView)
        {
            P42.Utils.Profile.Enter();
            SimpleListView = simpleListView;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Top;
            Tapped += OnCellWrapper_Tapped;
            SimpleListView._selectedItems.CollectionChanged += OnSelectedItems_CollectionChanged;
            SizeChanged += OnCellWrapper_SizeChanged;
            P42.Utils.Profile.Exit();
        }


        private void OnCellWrapper_SizeChanged(object sender, SizeChangedEventArgs args)
        {
            RecordCellHeight();
        }

        void RecordCellHeight()
        {
            P42.Utils.Profile.Enter();
            if (ActualHeight > -1 && DataContext != null && SimpleListView is SimpleListView parent)
            {
                var height = (int)(ActualHeight * SimpleListView.DisplayScale + 0.5);
                if (Index < parent.NativeCellHeights.Count)
                {
                    parent.NativeCellHeights[Index] = height;
                    P42.Utils.Profile.Exit("EXIT A");
                    return;
                }
                while (Index > parent.NativeCellHeights.Count)
                    parent.NativeCellHeights.Add(0);
                parent.NativeCellHeights.Add(height);
            }
            P42.Utils.Profile.Exit();
        }

        private void OnSelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateSelection();
        }

        async void OnCellWrapper_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await SimpleListView.OnWrapperClicked(this);
        }

        protected override void OnDataContextChanged(DependencyPropertyChangedEventArgs e)
        {
            P42.Utils.Profile.Enter();

            base.OnDataContextChanged(e);
            P42.Utils.Profile.Mark("A");

            UpdateSelection();
            P42.Utils.Profile.Mark("B");

            Child.DataContext = DataContext;
            P42.Utils.Profile.Mark("C");

            RecordCellHeight();
            P42.Utils.Profile.Exit();

            //InvalidateMeasure();
            //ForceLayout();
        }

        private void UpdateSelection()
        {
            Background = IsSelected
                ? SystemColors.ListLow.ToBrush()
                : Colors.Transparent.ToBrush();
        }

        bool _isArranging;
        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            if (_isArranging)
                return finalSize;

            P42.Utils.Profile.Enter();
            _isArranging = true;
            var result = base.ArrangeOverride(finalSize);
            _isArranging = false;
            P42.Utils.Profile.Exit();
            return result;
        }
    }

    partial class Cell : TextBlock
    {

        public Cell()
        {
            Foreground = P42.Utils.Uno.SystemColors.BaseMediumHigh.ToBrush();
            Margin = new Thickness(10, 5);
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Center;
        }

        protected override void OnDataContextChanged(DependencyPropertyChangedEventArgs e)
        {
            P42.Utils.Profile.Enter();
            base.OnDataContextChanged(e);
            Text = DataContext.ToString();
            P42.Utils.Profile.Exit();
        }
    }

    /*
    #region ScrollListener 
    class ScrollListener : Java.Lang.Object, Android.Widget.ListView.IOnScrollListener
    {
        public SimpleListView ListView;
        public bool IsBuildingLayOut;

        public void OnScroll(Android.Widget.AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            if (!IsBuildingLayOut)
                ListView?.OnScrolling(this, EventArgs.Empty);
        }

        public void OnScrollStateChanged(Android.Widget.AbsListView view, [GeneratedEnum] Android.Widget.ScrollState scrollState)
        {
            if (scrollState == ScrollState.Idle)
                ListView?.OnScrolled(this, EventArgs.Empty);
        }

    }
    #endregion
    */
}
