using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class WindowsManager : Singleton<WindowsManager>
{
    [SerializeField] private Transform _windowsCanvas;

    private List<BaseWindow> _openedWindows = new List<BaseWindow>();

    public void OpenWindow(BaseWindow window)
    {
        if (IsOpened(window.GetType()))
        {
            CloseWindow(window.GetType());
            return;
        }

        if (_openedWindows.Count != 0)
            return;

        BaseWindow newWindow = Instantiate(window, _windowsCanvas);
        newWindow.Init();
        _openedWindows.Add(newWindow);
    }

    public void CloseWindow(Type type)
    {
        var window = _openedWindows.FirstOrDefault(x => x.GetType() == type);

        if (window == null)
            return;

        _openedWindows.Remove(window);
        window.OnClose();
        Destroy(window.gameObject);
    }

    public bool IsOpened(Type type)
    {
        return _openedWindows.Any(x => x.GetType() == type);
    }

    public T FindWindow<T>() where T : BaseWindow
    {
        var window = _openedWindows.FirstOrDefault(x => x.GetType() == typeof(T));

        if (window == null || window == default)
            return null;

        return (T)window;
    }

    public void CloseCurrentWindow()
    {
        if (_openedWindows.Count > 0)
        {
            CloseWindow(_openedWindows[0].GetType());
        }
    }
}
