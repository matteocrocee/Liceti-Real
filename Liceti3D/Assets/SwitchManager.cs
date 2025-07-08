using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    public int totalSwitches = 4;
    public int activatedSwitches = 0;

    [Header("Porta da aprire")]
    public GameObject door;

    public void NotifySwitchActivated()
    {
        activatedSwitches++;
        if (activatedSwitches >= totalSwitches)
            OpenDoor();
    }

    void OpenDoor()
    {
        if (door != null)
        {
            door.SetActive(false);
            Debug.Log("Porta aperta!");
        }
    }
}
