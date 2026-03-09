using UnityEngine;

namespace RefClient
{
  public class RefClientMod : IPuckMod
  {
    private GameObject _uiObject;

    public bool OnEnable()
    {
      Debug.Log("[RefClient] Enabled.");

      _uiObject = new GameObject("RefClient_UI");
      _uiObject.AddComponent<RefClientUI>();
      Object.DontDestroyOnLoad(_uiObject);

      return true;
    }

    public bool OnDisable()
    {
      Debug.Log("[RefClient] Disabled.");

      if (_uiObject == null)
        return true;

      Object.Destroy(_uiObject);
      _uiObject = null;

      return true;
    }
  }
}
