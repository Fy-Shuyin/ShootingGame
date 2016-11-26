using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    Vector3  cam_rot;
    Transform cameraTransform;

    void Start ()
    {
        cameraTransform = this.transform;
        cam_rot = cameraTransform.eulerAngles;
        Screen.lockCursor = true;
    }
	
	void Update ()
    {
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");

        cam_rot.x -= rv;
        cam_rot.y += rh;

        if (cam_rot.x <= -90f)
        { cam_rot.x = -90f; }
        if (cam_rot.x >= 60f)
        { cam_rot.x = 60f; }
        cameraTransform.eulerAngles = cam_rot;

        Vector3 camTempRot = cameraTransform.eulerAngles;
        camTempRot.x = 0;
        camTempRot.z = 0;
        PlayerController.selfTransform.eulerAngles = camTempRot;
        cameraTransform.eulerAngles = new Vector3(cameraTransform.eulerAngles.x, PlayerController.selfTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
    }
}
