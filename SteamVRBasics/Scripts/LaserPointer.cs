using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LaserPointer : MonoBehaviour
{
    [SerializeField] SteamVR_Behaviour_Pose controllerPose = default;

    public GameObject laserPrefab;
    public Material validMaterial;
    public Material invalidMaterial;

    bool showLaser = false;
    bool isValidHit = false;
    RaycastHit hitPoint;
    GameObject laser;
    Transform laserTransform;

    public int pointerDistance = 100;
    public LayerMask laserMask;

    public void ShowLaser(bool showLaser)
    {
        this.showLaser = showLaser;
        laser.SetActive(showLaser);

        if (!showLaser)
        {
            isValidHit = false;
        }
    }

    public RaycastHit GetHitPoint()
    {
        return hitPoint;
    }

    public bool IsValidHit()
    {
        return isValidHit; 
    }

    void DrawLaser(RaycastHit hit)
    {
        if (isValidHit)
        {
            laser.GetComponent<Renderer>().material = validMaterial;
        }
        else
        {
            laser.GetComponent<Renderer>().material = invalidMaterial;
        }
        laserTransform.position = Vector3.Lerp(controllerPose.transform.position, hit.point, 0.5f);
        laserTransform.LookAt(hit.point);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
    }

    // Start is called before the first frame update
    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (showLaser)
        {

            //for detecting a hit with a valid object
            RaycastHit hit;
            isValidHit = Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, pointerDistance, laserMask);

            if (isValidHit)
            {
                hitPoint = hit;
                DrawLaser(hit);
            }
            else
            {
                //for always showing the laser
                RaycastHit laserPoint;
                Physics.Raycast(controllerPose.transform.position, transform.forward, out laserPoint, pointerDistance);
                DrawLaser(laserPoint);
            }

        }
    }
}
