using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class Interaction : MonoBehaviour
{
    [Header("Stats")]
    public bool isEnabled = true;
    public bool hasInteracted = false;
    public float interactDistance = 10;
    public string interactString = "Interact";

    [Header("References")]
    public InputActionReference inputAction;
    public UnityEvent events;

    private GameObject player;
    private GeneralStats playerStats;
    private Transform myCamera;
    private PlayerMovement playerMovement;
    private TMP_Text interactLabel;

    void Start()
    {
        inputAction.action.started += InteractEvent;

        player = GameObject.Find("Player");
        playerStats = player.GetComponent<GeneralStats>();
        myCamera = GameObject.Find("PlayerCam").transform;
        playerMovement = player.GetComponent<PlayerMovement>();

        TMP_Text baseLabel = GameObject.Find("InteractLabel").GetComponent<TMP_Text>();

        interactLabel = Instantiate(baseLabel, GameObject.Find("TopRenderHUD").transform);
        interactLabel.text = $"Press E/X to {interactString}";
    }

    void Update()
    {
        interactLabel.enabled = canInteract();
    }

    void OnDestroy()
    {
        Destroy(interactLabel);
    }

    // Input
    public void InteractEvent(InputAction.CallbackContext context)
    {
        Interact();
    }

    // Main Functions
    public bool canInteract()
    {
        if (player == null || hasInteracted || !isEnabled || !playerMovement.canInteract || !playerStats.isAlive) 
            return false;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= interactDistance)
        {
            Vector3 vector = (transform.position - myCamera.position).normalized;
            float dot = Vector3.Dot(myCamera.forward, vector);

            return dot >= 0.3f;
        }

        return false;
    }

    public void Interact()
    {
        if (canInteract())
        {
            hasInteracted = true;
            interactLabel.enabled = false;

            events.Invoke();
        }
    }
}
