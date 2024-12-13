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
    public GameObject content;
    public List<ChatMessage> messages;

    //private Queue chatQueue = new();

    private void Start()
    {
        for (int i = 0; i < messages.Count; i++)
            CreateMessage(i);

        // TODO later: when the first message Y pos changes, delete it (since we'd hit the max)
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            CreateMessage(1); // testing keybind
    }

    public GameObject CreateMessage(int messageIndex)
    {
        // Create

        ChatMessage messageData = messages[messageIndex];

        GameObject messageObject = Instantiate(exampleMessage, content.transform);
        TMP_Text textMeshPro = messageObject.GetComponent<TMP_Text>();

        textMeshPro.text = $"{messageData.username}: {messageData.message}";

        messageObject.name = messageIndex.ToString();
        messageObject.SetActive(true);

        // two problems: autoscrolling on new messages: https://discussions.unity.com/t/scrollview-auto-scroll-to-bottom/895083
        // and weird offsets with different length messages: different prefab for different line lengths?

        return messageObject;
    }
}
