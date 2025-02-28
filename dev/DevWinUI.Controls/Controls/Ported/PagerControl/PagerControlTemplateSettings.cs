﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace DevWinUI;

public partial class PagerControlTemplateSettings : DependencyObject
{
    public IList<object> Pages
    {
        get { return (IList<object>)GetValue(PagesProperty); }
        set { SetValue(PagesProperty, value); }
    }
    public static readonly DependencyProperty PagesProperty =
        DependencyProperty.Register("Pages", typeof(IList<object>), typeof(PagerControlTemplateSettings), new PropertyMetadata(new List<object>()));

    public IList<object> NumberPanelItems
    {
        get { return (IList<object>)GetValue(NumberPanelItemsProperty); }
        set { SetValue(NumberPanelItemsProperty, value); }
    }
    public static readonly DependencyProperty NumberPanelItemsProperty =
        DependencyProperty.Register("NumberPanelItems", typeof(IList<object>), typeof(PagerControlTemplateSettings), new PropertyMetadata(new List<object>()));
}
