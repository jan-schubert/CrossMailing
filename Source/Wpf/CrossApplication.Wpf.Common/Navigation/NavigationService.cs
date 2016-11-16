using System;
using CrossApplication.Core.Contracts;
using CrossApplication.Core.Contracts.Common.Navigation;
using CrossApplication.Wpf.Contracts.Navigation;
using Prism.Regions;

namespace CrossApplication.Wpf.Common.Navigation
{
    public class NavigationService : INavigationService
    {
        public NavigationService(IRegionManager regionManager, IViewManager viewManager, IUserManager userManager)
        {
            _regionManager = regionManager;
            _viewManager = viewManager;
            _userManager = userManager;
        }

        public void NavigateTo(string navigationKey)
        {
            NavigateTo(_viewManager.GetViewItem(navigationKey));
        }

        private void NavigateTo(ViewItem viewItem)
        {
            if (viewItem.IsAuthorizationRequired && !_userManager.IsAuthorized)
            {
                NavigateToViewItem(_viewManager.LoginViewItem);
                return;
            }

            NavigateToViewItem(viewItem);
        }

        private void NavigateToViewItem(ViewItem viewItem)
        {
            _regionManager.RequestNavigate(viewItem.RegionName, new Uri(viewItem.ViewKey, UriKind.Relative));

            foreach (var subViewItem in viewItem.SubViewItems)
            {
                NavigateTo(subViewItem);
            }
        }

        private readonly IRegionManager _regionManager;
        private readonly IUserManager _userManager;
        private readonly IViewManager _viewManager;
    }
}