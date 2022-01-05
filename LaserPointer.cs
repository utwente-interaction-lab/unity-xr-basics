using UnityEngine;
using Valve.VR;

public class LaserPointer : MonoBehaviour
{
    [SerializeField] SteamVR_Input_Sources handType;
    [SerializeField] SteamVR_Behaviour_Pose controllerPose;
    [SerializeField] SteamVR_Action_Boolean teleportAction;

    // Laser variables
    [SerializeField] GameObject laserPrefab;
    private GameObject laser;
    private Vector3 hitPoint;

    // Reticle variables
    [SerializeField] Transform cameraRigTransform;
    [SerializeField] GameObject teleportReticlePrefab;
    private GameObject reticle;
    [SerializeField] Transform headTransform;
    [SerializeField] Vector3 teleportReticleOffset;
    [SerializeField] LayerMask teleportMask;
    private bool shouldTeleport;

    private void Start()
    {
        laser = Instantiate(laserPrefab);
        reticle = Instantiate(teleportReticlePrefab);
    }
    
    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laser.transform.position = Vector3.Lerp(controllerPose.transform.position, hitPoint,0.5f);
        laser.transform.LookAt(hitPoint);
        Vector3 laserScale = laser.transform.localScale;
        laser.transform.localScale = new Vector3(laserScale.x, laserScale.y, hit.distance);
    }

    private void Teleport()
    {
        shouldTeleport = false;
        reticle.SetActive(false);
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0;
        cameraRigTransform.position = hitPoint + difference;
    }
    
    private void Update()
    {
        if (teleportAction.GetState(handType))
        {
            RaycastHit hit;
            Vector3 controllerPos = controllerPose.transform.position;
            if (Physics.Raycast(controllerPos, transform.forward, out hit, 100))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
                if (((1<<hit.collider.gameObject.layer) & teleportMask) != 0)
                {
                    //print( "Teleport hit!" );
                    reticle.SetActive(true);
                    reticle.transform.position = hitPoint + teleportReticleOffset;
                    shouldTeleport = true;
                } else {
                    reticle.SetActive(false);
                    shouldTeleport = false;
                }
            }
            else
            {
                laser.SetActive(false);
                reticle.SetActive(false);
            }
        }
        else
        {
            laser.SetActive(false);
            reticle.SetActive(false);
        }

        if (teleportAction.GetStateUp(handType) && shouldTeleport)
        {
            Teleport();
        }
    }
}