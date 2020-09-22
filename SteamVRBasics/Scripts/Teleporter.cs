using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleporter : MonoBehaviour
{

    [SerializeField] SteamVR_Input_Sources handType = default;
    [SerializeField] SteamVR_Action_Boolean teleportAction = default;

    public LaserPointer laser;
    public Transform cameraRigTransform;
    public GameObject teleportReticlePrefab;
    GameObject reticle;
    Transform teleportReticleTransform;
    public Transform headTransform;
    public Vector3 teleportReticleOffset;

    void ShowReticle(bool isValid, RaycastHit hit)
    {
        reticle.SetActive(isValid);

        if (isValid)
        {
            teleportReticleTransform.position = hit.point + teleportReticleOffset;
        }
    }

    void Teleport(Vector3 location)
    {
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0;
        cameraRigTransform.position = location + difference;
    }

    // Start is called before the first frame update
    void Start()
    {
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //teleport when releasing the button, if location is valid
        if (laser.IsValidHit() && teleportAction.GetStateUp(handType))
        {
            Teleport(laser.GetHitPoint().point);
        }

        //show/hide the laser and reticle when pressing the teleport button
        laser.ShowLaser(teleportAction.GetState(handType));
        ShowReticle(laser.IsValidHit(), laser.GetHitPoint());

    }
}
