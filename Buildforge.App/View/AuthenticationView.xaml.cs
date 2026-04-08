using Buildforge.App.Domain.Token;
using Buildforge.App.ViewModel.Authentication;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace Buildforge.App.View;

public partial class AuthenticationView : UserControl
{
    public AuthenticationView()
    {
        InitializeComponent();

        DataContext = App.Services.GetRequiredService<AuthenticationViewModel>();
    }

    private async void WebViewInitialized(object sender, EventArgs e)
    {
        if (App.Services.GetRequiredService<TokenHandler>().HasValidToken())
        {
            return;
        }

        if (sender is not WebView2 webView)
        {
            return;
        }

        await webView.EnsureCoreWebView2Async();

        await webView.CoreWebView2.Profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.AllProfile);
    }

    private void WebViewNavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
    {
        if (sender is not WebView2 webView)
        {
            return;
        }

        if (DataContext is not AuthenticationViewModel authenticationViewModel)
        {
            return;
        }

        var uri = new Uri(e.Uri);

        if (uri.AbsoluteUri.StartsWith(AuthenticationViewModel.RedirectUri))
        {
            e.Cancel = true;

            Task.Run(async () =>
            {
                await authenticationViewModel.Authenticate(uri);
            });
        }
    }
}