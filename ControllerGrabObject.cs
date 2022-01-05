using UnityEngine;
using Valve.VR;

public class ControllerGrabObject: MonoBehaviour
{
    [SerializeField] SteamVR_Input_Sources handType;
    [SerializeField] SteamVR_Action_Boolean grabAction;
    [SerializeField] SteamVR_Behaviour_Pose controllerPose;

    private GameObject collidingObject;
    private GameObject objectInHand;
   
    void SetCollidingObject(Collider col)
    {
        if (!collidingObject && col.GetComponent<Rigidbody>())
        {
            collidingObject = col.gameObject;
            //print("CollidingObject");
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
            //print("No Colliding Object");
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
        if(GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            Rigidbody rb = objectInHand.GetComponent<Rigidbody>();
            rb.velocity = controllerPose.GetVelocity();
            rb.angularVelocity = controllerPose.GetAngularVelocity();
        }
        objectInHand = null;
    }

    FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    void Update()
    {
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObject)
            {
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