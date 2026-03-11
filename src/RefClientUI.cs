using UnityEngine;
using UnityEngine.InputSystem;

namespace RefClient
{
  public class RefClientUI : MonoBehaviour
  {
    private bool _panelVisible = false;
    private bool _cursorUnlocked = false;

    private TeamPlayerWindow _redWindow;
    private TeamPlayerWindow _blueWindow;
    private InfractionsWindow _infractionsWindow;

    private void Start()
    {
      float margin = 20f;

      _redWindow = new TeamPlayerWindow("Red", PlayerTeam.Red, 98234,
        new Rect(Screen.width - 520 - margin, Screen.height - 420, 520, 400));

      _blueWindow = new TeamPlayerWindow("Blue", PlayerTeam.Blue, 98235,
        new Rect(margin, Screen.height - 420, 520, 400));

      float infWidth = 700f;
      _infractionsWindow = new InfractionsWindow(98236,
        new Rect((Screen.width - infWidth) / 2f, Screen.height - 100, infWidth, 70));
    }

    private void Update()
    {
      if (Keyboard.current == null)
        return;

      bool shiftHeld = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
      bool ctrlHeld = Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed;

      if (ctrlHeld && shiftHeld && Keyboard.current[Key.H].wasPressedThisFrame)
      {
        _panelVisible = !_panelVisible;

        if (!_panelVisible)
          ReleaseCursor();
      }

      if (!_panelVisible)
        return;

      if (Mouse.current == null)
        return;

      if (Mouse.current.rightButton.wasPressedThisFrame)
        AcquireCursor();

      if (Mouse.current.rightButton.wasReleasedThisFrame)
        ReleaseCursor();
    }

    private void AcquireCursor()
    {
      if (_cursorUnlocked)
        return;

      _cursorUnlocked = true;
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;

      try
      {
        NetworkBehaviourSingleton<UIManager>.Instance.isMouseActive = true;
      }
      catch { }
    }

    private void ReleaseCursor()
    {
      if (!_cursorUnlocked)
        return;

      _cursorUnlocked = false;
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;

      try
      {
        NetworkBehaviourSingleton<UIManager>.Instance.isMouseActive = false;
      }
      catch { }
    }

    private void OnGUI()
    {
      if (!_panelVisible)
        return;

      RefClientStyles.EnsureInitialized();

      _redWindow.Draw();
      _blueWindow.Draw();
      _infractionsWindow.Draw();
    }
  }
}
