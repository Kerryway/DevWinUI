﻿using System.Diagnostics.CodeAnalysis;

namespace DevWinUI;
public partial class NavigationServiceEx
{
    public Frame? Frame
    {
        get
        {
            return _frame;
        }

        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => Frame != null && Frame.CanGoBack;

    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated += OnFrameNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated -= OnFrameNavigated;
        }
    }

    public bool GoBack()
    {
        if (CanGoBack)
        {
            var frameContentBeforeNavigationAOTSafe = _frame?.Content;

            _frame.GoBack();

            if (frameContentBeforeNavigationAOTSafe is Page page && page?.DataContext is INavigationAwareEx viewModel)
            {
                viewModel.OnNavigatedFrom();
            }

            return true;
        }

        return false;
    }

    public bool NavigateTo(Type pageType, object? parameter = null, bool clearNavigation = false, NavigationTransitionInfo transitionInfo = null)
    {
        return Navigate(pageType, parameter, clearNavigation, transitionInfo);
    }

    public bool Navigate(Type pageType, object? parameter = null, bool clearNavigation = false, NavigationTransitionInfo transitionInfo = null)
    {
        if (pageType == null)
        {
            return false;
        }

        if (_frame != null && (_frame.CurrentSourcePageType != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed))))
        {
            _frame.Tag = clearNavigation;

            if (_useBreadcrumbBar)
            {
                _mainBreadcrumb.AddNewItem(pageType, parameter);
            }

            var frameContentBeforeNavigationAOTSafe = _frame.Content;

            var navigated = _frame.Navigate(pageType, parameter, transitionInfo);
            if (navigated)
            {
                _lastParameterUsed = parameter;

                if (frameContentBeforeNavigationAOTSafe is Page page && page?.DataContext is INavigationAwareEx viewModel)
                {
                    viewModel.OnNavigatedFrom();
                }
            }

            return navigated;
        }

        return false;
    }
    private void OnFrameNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                frame.BackStack.Clear();
            }

            // This is AOT Safe
            if (_frame?.Content is Page page && page?.DataContext is INavigationAwareEx viewModel)
            {
                viewModel.OnNavigatedTo(e.Parameter);
            }

            FrameNavigated?.Invoke(sender, e);
        }
    }
}
