using Buildforge.App.ViewModel.Root;
using Vanara.PInvoke;

namespace Buildforge.App.View;

public partial class RootView : Window
{
    private const int TrayMessageId = (int)User32.WindowMessage.WM_APP + 1;

    public RootView()
    {
        InitializeComponent();

        DataContext = App.Services.GetRequiredService<RootViewModel>();
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        InitializeTrayIcon();

        base.OnSourceInitialized(e);
    }

    private void InitializeTrayIcon()
    {
        var helper = new WindowInteropHelper(this);

        var windowHandle = helper.Handle;

        var source = HwndSource.FromHwnd(windowHandle);

        source.AddHook(WindowHandleSourceHook);

        string exePath = Environment.ProcessPath ?? throw new NullReferenceException(nameof(Environment.ProcessPath));

        var icon = System.Drawing.Icon.ExtractAssociatedIcon(exePath) ?? throw new NullReferenceException(nameof(System.Drawing.Icon.ExtractAssociatedIcon));

        var data = new Shell32.NOTIFYICONDATA()
        {
            cbSize = (uint)Marshal.SizeOf(new Shell32.NOTIFYICONDATA()),
            uCallbackMessage = TrayMessageId,
            uFlags = Shell32.NIF.NIF_ICON | Shell32.NIF.NIF_MESSAGE,
            uID = 0,
            hIcon = icon.Handle,
            hwnd = windowHandle,
        };

        Shell32.Shell_NotifyIcon(Shell32.NIM.NIM_ADD, in data);
    }

    private nint WindowHandleSourceHook(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
    {
        if (msg != TrayMessageId)
        {
            return IntPtr.Zero;
        }

        if (lParam is (int)User32.WindowMessage.WM_LBUTTONUP || lParam is (int)User32.WindowMessage.WM_RBUTTONUP)
        {
            ShowActivate();

            handled = true;
        }

        return IntPtr.Zero;
    }

    private void ShowActivate()
    {
        Show();

        WindowState = WindowState.Normal;

        Activate();
    }

    private void OnStateChanged(object sender, EventArgs e)
    {
        if (WindowState is WindowState.Minimized)
        {
            Hide();
        }
    }
}