using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace P42.Uno.SimpleListView
{
    public partial class SimpleListView : UserControl
    {

        #region Properties

        #region ListViewBase Properties

        #region IsItemClickEnabled Property
        public static readonly DependencyProperty IsItemClickEnabledProperty = DependencyProperty.Register(
            nameof(IsItemClickEnabled),
            typeof(bool),
            typeof(SimpleListView),
            new PropertyMetadata(default(bool), OnIsItemClickEnabledChanged)
        );

        public bool IsItemClickEnabled
        {
            get => (bool)GetValue(IsItemClickEnabledProperty);
            set => SetValue(IsItemClickEnabledProperty, value);
        }
        #endregion IsItemClickEnabled Property

        #region Footer Property
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
            nameof(Footer),
            typeof(object),
            typeof(SimpleListView),
            new PropertyMetadata(default(object), OnFooterChanged)
        );

        public object Footer
        {
            get => (object)GetValue(FooterProperty);
            set => SetValue(FooterProperty, value);
        }
        #endregion Footer Property

        #region FooterTemplate Property
        public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(
            nameof(FooterTemplate),
            typeof(DataTemplate),
            typeof(SimpleListView),
            new PropertyMetadata(default(DataTemplate), OnFooterTemplateChanged)
        );

        public DataTemplate FooterTemplate
        {
            get => (DataTemplate)GetValue(FooterTemplateProperty);
            set => SetValue(FooterTemplateProperty, value);
        }
        #endregion FooterTemplate Property


        #region Header Property
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(object),
            typeof(SimpleListView),
            new PropertyMetadata(default(object), OnHeaderChanged)
        );

        public object Header
        {
            get => (object)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        #endregion Header Property

        #region HeaderTemplate Property
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            nameof(HeaderTemplate),
            typeof(DataTemplate),
            typeof(SimpleListView),
            new PropertyMetadata(default(DataTemplate), OnHeaderTemplateChanged)
        );

        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }
        #endregion HeaderTemplate Property


        #region SelectedItems Property
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            nameof(SelectedItems),
            typeof(IList<object>),
            typeof(SimpleListView),
            new PropertyMetadata(default(IList<object>))
        );
        public IList<object> SelectedItems
        {
            get => (IList<object>)GetValue(SelectedItemsProperty);
            private set => SetValue(SelectedItemsProperty, value);
        }
        #endregion SelectedItems Property

        #region SelectionMode Property
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            nameof(SelectionMode),
            typeof(ListViewSelectionMode),
            typeof(SimpleListView),
            new PropertyMetadata(default(ListViewSelectionMode), OnSelectionModeChanged)
        );

        public ListViewSelectionMode SelectionMode
        {
            get => (ListViewSelectionMode)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }
        #endregion SelectionMode Property

        #region Seletor Properties

        #region SelectedIndex Property
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            nameof(SelectedIndex),
            typeof(int),
            typeof(SimpleListView),
            new PropertyMetadata(-1, OnSelectedIndexChanged)
        );

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }
        #endregion SelectedIndex Property

        #region SelectedItem Property
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(SimpleListView),
            new PropertyMetadata(default(object), OnSelectedItemChanged)
        );

        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        #endregion SelectedItem Property

        #region ItemsControl Properties


        #region ItemsSource Property
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(SimpleListView),
            new PropertyMetadata(default(IEnumerable), OnItemsSourceChanged)
        );

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        #endregion ItemsSource Property

        #region ItemTemplate Property
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(SimpleListView),
            new PropertyMetadata(default(DataTemplate), OnItemTemplateChanged)
        );

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }
        #endregion ItemTemplate Property

        #region ItemTemplateSelector Property
        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register(
            nameof(ItemTemplateSelector),
            typeof(P42.Utils.Uno.DataTemplateSetSelector),
            typeof(SimpleListView),
            new PropertyMetadata(default(P42.Utils.Uno.DataTemplateSetSelector), OnItemTemplateSelectorChanged)
        );

        public P42.Utils.Uno.DataTemplateSetSelector ItemTemplateSelector
        {
            get => (P42.Utils.Uno.DataTemplateSetSelector)GetValue(ItemTemplateSelectorProperty);
            set => SetValue(ItemTemplateSelectorProperty, value);
        }
        #endregion ItemTemplateSelector Property


        #endregion

        #endregion Selector Properties

        #endregion ListViewBase Properties

        #endregion


        #region Events

        #region ListViewBase Events

        public event ItemClickEventHandler ItemClick;

        #region Selector Events

        public event SelectionChangedEventHandler SelectionChanged;

        #endregion

        #endregion

        #endregion


        #region Constructors
        public SimpleListView()
        {
            PlatformBuild();
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
        }
        #endregion


        #region Methods


        #endregion
    }
}
