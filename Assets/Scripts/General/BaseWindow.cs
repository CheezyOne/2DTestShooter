using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWindow : MonoBehaviour
{
    [SerializeField] private Button _closeButton;

    public virtual void Init()
    {
        if(_closeButton != null)
            _closeButton.onClick.AddListener(Close);
    }

    public void Close()
    {
        WindowsManager.Instance?.CloseWindow(GetType());
    }

    public virtual void OnClose() { }
}
