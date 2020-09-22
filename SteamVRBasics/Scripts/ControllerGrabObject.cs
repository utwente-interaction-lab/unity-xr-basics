using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerGrabObject : MonoBehaviour
{
    [SerializeField] SteamVR_Input_Sources handType = default;
    [SerializeField] SteamVR_Action_Boolean grabAction = default;
    [SerializeField] SteamVR_Behaviour_Pose controllerPose = default;

    GameObject collidingObject;
    GameObject objectInHand;

    void SetCollidingObject(Collider col)
    {
        if(!collidingObject && col.GetComponent<Rigidbody>())
        {
            collidingObject = col.gameObject;
            Debug.Log("Colliding Object");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (collidingObject)
        {
            collidingObject = null;
            Debug.Log("No Coliding Object");
        }
    }

    void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        }
        objectInHand = null;
        Debug.Log("Released Object");
    }

    FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObject) {
                GrabObject();
            }
        }
        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
    }
}
