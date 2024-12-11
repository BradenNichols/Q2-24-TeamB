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
            CreateMessage(0); // testing keybind
    }

    public GameObject CreateMessage(int messageIndex)
    {
        // Old Message

        //GameObject oldestMessage;
        /*
        float oldYPos = 0;

        if (chatQueue.Count > 0)
        {
            //oldestMessage = (GameObject)chatQueue.Peek();
            oldYPos = ((GameObject)chatQueue.Peek()).GetComponent<RectTransform>().rect.y;
        }*/

        // Create

        ChatMessage messageData = messages[messageIndex];

        GameObject messageObject = Instantiate(exampleMessage, content.transform);
        TMP_Text textMeshPro = messageObject.GetComponent<TMP_Text>();

        textMeshPro.text = $"{messageData.username}: {messageData.message}";

        messageObject.name = messageIndex.ToString();
        messageObject.SetActive(true);

        // After

        //Debug.Log($"Create message (index {messageIndex.ToString()})");
        /*chatQueue.Enqueue(messageObject);

        Debug.Log(oldYPos);
        Debug.Log(((GameObject)chatQueue.Peek()).GetComponent<RectTransform>().rect.y);

        if (chatQueue.Count > 1 && oldYPos != ((GameObject)chatQueue.Peek()).GetComponent<RectTransform>().rect.y)
        {
            Destroy((GameObject)chatQueue.Dequeue());

            Debug.Log("DESTROYED!");
        }*/

        return messageObject;
    }
}
