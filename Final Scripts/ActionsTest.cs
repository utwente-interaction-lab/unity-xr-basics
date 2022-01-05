using UnityEngine;
using Valve.VR;

public class ActionsTest : MonoBehaviour
{
    [SerializeField] SteamVR_Input_Sources handType;
    [SerializeField] SteamVR_Action_Boolean teleportAction;
    [SerializeField] SteamVR_Action_Boolean grabAction;

    void Update() 
    {
        if (grabAction.GetState(handType))
        {
        print("Grab: " + handType);
        }

        if (teleportAction.GetStateDown(handType))
        {
        print("Teleport: " + handType);
        }
    }
}

