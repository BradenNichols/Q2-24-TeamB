using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct ChatMessage
{
    public string username;
    public string message;
}
public class StreamChats : MonoBehaviour
{
    public List<ChatMessage> messages;
}
