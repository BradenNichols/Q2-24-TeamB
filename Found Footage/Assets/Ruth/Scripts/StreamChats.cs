using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public struct ChatMessage
{
    public string username;
    public string message;
}
public class StreamChats : MonoBehaviour
{
    public GameObject exampleMessage;
    public List<ChatMessage> messages;

    private void Start()
    {
        for (int i = 0; i < messages.Count; i++)
            CreateMessage(i);

        // TODO later: when the first message Y pos changes, delete it (since we'd hit the max)
    }

    public GameObject CreateMessage(int messageIndex)
    {
        ChatMessage messageData = messages[messageIndex];

        GameObject messageObject = Instantiate(exampleMessage, transform);
        TMP_Text textMeshPro = messageObject.GetComponent<TMP_Text>();

        textMeshPro.text = $"{messageData.username}: {messageData.message}";

        messageObject.name = messageIndex.ToString();
        messageObject.SetActive(true);

        Debug.Log($"Create message (index {messageIndex.ToString()})");

        return messageObject;
    }
}
