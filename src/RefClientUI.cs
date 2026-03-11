using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RefClient
{
  public class RefClientUI : MonoBehaviour
  {
    private bool _panelVisible = false;
    private MouseComponent _mouseComponent;

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

      _mouseComponent = new MouseComponent
      {
        VisibilityRequiresMouse = true
      };

      RegisterMouseComponent();
    }

    private void Update()
    {
      if (Keyboard.current != null)
      {
        bool shiftHeld = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        bool ctrlHeld = Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed;

        if (ctrlHeld && shiftHeld && Keyboard.current[Key.H].wasPressedThisFrame)
          _panelVisible = !_panelVisible;
      }

      bool shouldAcquire = _panelVisible
        && Mouse.current != null
        && Mouse.current.rightButton.isPressed;

      if (shouldAcquire)
        _mouseComponent.Show();
      else
        _mouseComponent.Hide();
    }

    private void RegisterMouseComponent()
    {
      try
      {
        var uiManager = NetworkBehaviourSingleton<UIManager>.Instance;

        var componentsField = typeof(UIManager).GetField("components",
          BindingFlags.NonPublic | BindingFlags.Instance);

        var components = (List<UIComponent>)componentsField.GetValue(uiManager);
        components.Add(_mouseComponent);

        var onVisibility = typeof(UIManager).GetMethod("OnMouseRequiredComponentChangedVisibility",
          BindingFlags.NonPublic | BindingFlags.Instance);
  
        var onFocus = typeof(UIManager).GetMethod("OnMouseRequiredComponentChangedFocus",
          BindingFlags.NonPublic | BindingFlags.Instance);

        _mouseComponent.OnVisibilityChanged += (EventHandler)Delegate.CreateDelegate(
          typeof(EventHandler), uiManager, onVisibility);

        _mouseComponent.OnFocusChanged += (EventHandler)Delegate.CreateDelegate(
          typeof(EventHandler), uiManager, onFocus);

        Debug.Log("[RefClient] Registered mouse component with UIManager.");
      }
      catch (Exception ex)
      {
        Debug.LogWarning($"[RefClient] Failed to register mouse component: {ex.Message}");
      }
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
