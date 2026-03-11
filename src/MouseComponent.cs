using System;

namespace RefClient
{
  public class MouseComponent : UIComponent
  {
    private bool _isVisible;

    public event EventHandler OnVisibilityChanged;
#pragma warning disable CS0067
    public event EventHandler OnFocusChanged;
#pragma warning restore CS0067

    public bool FocusRequiresMouse { get; set; }
    public bool VisibilityRequiresMouse { get; set; }
    public bool AlwaysVisible { get; set; }

    public bool IsVisible => _isVisible;

    public bool IsFocused { get; set; }

    public void Show()
    {
      if (_isVisible)
        return;

      _isVisible = true;
      OnVisibilityChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Hide(bool ignoreAlwaysVisible = false)
    {
      if (!_isVisible)
        return;

      _isVisible = false;
      OnVisibilityChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Toggle()
    {
      if (_isVisible)
      {
        Hide();
        return;
      }

      Show();
    }
  }
}
