using UnityEngine;

namespace RefClient
{
  public static class ChatService
  {
    public static void Send(string message)
    {
      try
      {
        UIChat chat = Object.FindFirstObjectByType<UIChat>(FindObjectsInactive.Include);
        if (chat == null)
        {
          Debug.Log($"[RefClient] UIChat not found. Message: {message}");
          return;
        }

        chat.Client_SendClientChatMessage(message, false);
      }
      catch
      {
        Debug.Log($"[RefClient] Chat unavailable. Message: {message}");
      }
    }
  }
}
