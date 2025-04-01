﻿using Microsoft.UI.Xaml.Automation;

namespace DevWinUI;
public partial class JsonNavigationService
{
    private void AddNavigationMenuItems()
    {
        AddNavigationMenuItems(OrderItemsType.None);
    }
    private void AddNavigationMenuItems(OrderItemsType orderItems)
    {
        foreach (var group in GetOrderedDataGroups(orderItems).Where(i => !i.IsSpecialSection && !i.HideGroup))
        {
            var itemGroup = new NavigationViewItem()
            {
                Content = group.Title,
                IsExpanded = group.IsExpanded,
                Tag = group.UniqueId,
                DataContext = group,
                InfoBadge = GetInfoBadge(group)
            };
            var icon = GetIcon(group.ImagePath, group.IconGlyph);
            if (icon != null)
            {
                itemGroup.Icon = icon;
            }

            AutomationProperties.SetName(itemGroup, group.Title);
            AutomationProperties.SetAutomationId(itemGroup, group.UniqueId);

            foreach (var item in GetOrderedDataItems(group.Items, orderItems).Where(i => !i.HideNavigationViewItem))
            {
                var itemInGroup = new NavigationViewItem()
                {
                    IsEnabled = item.IncludedInBuild,
                    Content = item.Title,
                    Tag = item.UniqueId,
                    DataContext = item,
                    InfoBadge = GetInfoBadge(item)
                };

                var iconInGroup = GetIcon(item.ImagePath, item.IconGlyph);
                if (iconInGroup != null)
                {
                    itemInGroup.Icon = iconInGroup;
                }

                AutomationProperties.SetName(itemInGroup, item.Title);
                AutomationProperties.SetAutomationId(itemInGroup, item.UniqueId);

                if (group.ShowItemsWithoutGroup)
                {
                    if (group.IsFooterNavigationViewItem)
                    {
                        _navigationView.FooterMenuItems.Add(itemInGroup);
                    }
                    else
                    {
                        if (group.IsNavigationViewItemHeader)
                        {
                            _navigationView.MenuItems.Add(new NavigationViewItemHeader { Content = itemInGroup.Content });
                        }
                        else
                        {
                            _navigationView.MenuItems.Add(itemInGroup);
                        }
                    }
                }
                else
                {
                    itemGroup.MenuItems.Add(itemInGroup);
                }
            }

            if (!group.ShowItemsWithoutGroup)
            {
                if (group.IsFooterNavigationViewItem)
                {
                    _navigationView.FooterMenuItems.Add(itemGroup);
                }
                else
                {
                    if (group.IsNavigationViewItemHeader)
                    {
                        _navigationView.MenuItems.Add(new NavigationViewItemHeader { Content = itemGroup.Content });
                    }
                    else
                    {
                        _navigationView.MenuItems.Add(itemGroup);
                    }
                }
            }
        }

        EnsureNavigationSelection(_defaultPage?.ToString());
    }

    private IEnumerable<DataGroup> GetOrderedDataGroups(OrderItemsType orderItems)
    {
        switch (orderItems)
        {
            case OrderItemsType.None:
                return DataSource.Instance.Groups;
            case OrderItemsType.AscendingBoth:
            case OrderItemsType.AscendingTopLevel:
                return DataSource.Instance.Groups.OrderBy(i => i.Title);
            case OrderItemsType.DescendingBoth:
            case OrderItemsType.DescendingTopLevel:
                return DataSource.Instance.Groups.OrderByDescending(i => i.Title);
            default:
                return DataSource.Instance.Groups;
        }
    }
    private IEnumerable<DataItem> GetOrderedDataItems(IEnumerable<DataItem> dataItems, OrderItemsType orderItems)
    {
        switch (orderItems)
        {
            case OrderItemsType.None:
                return dataItems;
            case OrderItemsType.AscendingBoth:
            case OrderItemsType.AscendingSubLevel:
                return dataItems.OrderBy(i => i.Title);
            case OrderItemsType.DescendingBoth:
            case OrderItemsType.DescendingSubLevel:
                return dataItems.OrderByDescending(i => i.Title);
            default:
                return dataItems;
        }
    }
    private InfoBadge GetInfoBadge(BaseDataInfo data)
    {
        var dataInfoBadge = data.DataInfoBadge;
        if (dataInfoBadge != null)
        {
            bool hideNavigationViewItemBadge = dataInfoBadge.HideNavigationViewItemBadge;
            string value = dataInfoBadge.BadgeValue;
            string style = dataInfoBadge.BadgeStyle;
            bool hasValue = !string.IsNullOrEmpty(value);
            if (style != null && (style.Contains("Dot", StringComparison.OrdinalIgnoreCase) || style.Contains("Icon", StringComparison.OrdinalIgnoreCase)))
            {
                hasValue = true;
            }
            if (!hideNavigationViewItemBadge && hasValue)
            {
                int badgeValue = Convert.ToInt32(dataInfoBadge.BadgeValue);
                int width = dataInfoBadge.BadgeWidth;
                int height = dataInfoBadge.BadgeHeight;

                InfoBadge infoBadge = new()
                {
                    Style = Application.Current.Resources[style] as Style
                };
                switch (style.ToLower())
                {
                    case string s when s.Contains("value"):
                        infoBadge.Value = badgeValue;
                        break;
                    case string s when s.Contains("icon"):
                        infoBadge.IconSource = GetIconSource(dataInfoBadge);
                        break;
                }

                if (width > 0 && height > 0)
                {
                    infoBadge.Width = width;
                    infoBadge.Height = height;
                }

                return infoBadge;
            }
        }
        return null;
    }

    private IconSource GetIconSource(DataInfoBadge infoBadge)
    {
        string symbol = infoBadge?.BadgeSymbolIcon;
        string image = infoBadge?.BadgeBitmapIcon;
        string glyph = infoBadge?.BadgeFontIconGlyph;
        string fontName = infoBadge?.BadgeFontIconFontName;

        if (!string.IsNullOrEmpty(symbol))
        {
            return new SymbolIconSource
            {
                Symbol = GeneralHelper.GetEnum<Symbol>(symbol),
                Foreground = Application.Current.Resources["SystemControlForegroundAltHighBrush"] as Brush,
            };
        }

        if (!string.IsNullOrEmpty(image))
        {
            return new BitmapIconSource
            {
                UriSource = new Uri(image),
                ShowAsMonochrome = false
            };
        }

        if (!string.IsNullOrEmpty(glyph))
        {
            var fontIcon = new FontIconSource
            {
                Glyph = GeneralHelper.GetGlyph(glyph),
                Foreground = Application.Current.Resources["SystemControlForegroundAltHighBrush"] as Brush,
            };
            if (!string.IsNullOrEmpty(fontName))
            {
                fontIcon.FontFamily = new FontFamily(fontName);
            }
            return fontIcon;
        }
        return null;
    }

    private IconElement GetIcon(string imagePath, string iconGlyph)
    {
        if (string.IsNullOrEmpty(imagePath) && string.IsNullOrEmpty(iconGlyph))
        {
            return null;
        }

        if (!string.IsNullOrEmpty(iconGlyph))
        {
            return GetFontIcon(iconGlyph);
        }

        if (!string.IsNullOrEmpty(imagePath))
        {
            return new BitmapIcon() { UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute), ShowAsMonochrome = false };
        }

        return null;
    }

    private FontIcon GetFontIcon(string glyph)
    {
        var fontIcon = new FontIcon();
        if (!string.IsNullOrEmpty(_fontFamilyForGlyph))
        {
            fontIcon.FontFamily = new FontFamily(_fontFamilyForGlyph);
        }
        var _glyph = GeneralHelper.GetGlyph(glyph);
        if (!string.IsNullOrEmpty(_glyph))
        {
            fontIcon.Glyph = _glyph; // Set the Glyph property
        }
        else
        {
            fontIcon.Glyph = glyph;
        }

        return fontIcon;
    }
}
