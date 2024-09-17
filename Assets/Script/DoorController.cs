using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public Transform doorTransform;
    public float openAngle = 90f;
    public float closeAngle = 0f;
    public float openSpeed = 2f;
    private bool isOpen = false;
    private bool isPlayerNear = false;

    void Start()
    {
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }

        if (isOpen)
        {
            doorTransform.localRotation = Quaternion.Slerp(doorTransform.localRotation, Quaternion.Euler(0f, 0f, openAngle), Time.deltaTime * openSpeed);
        }
        else
        {
            doorTransform.localRotation = Quaternion.Slerp(doorTransform.localRotation, Quaternion.Euler(0f, 0f, closeAngle), Time.deltaTime * openSpeed);
        }
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
