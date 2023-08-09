using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private float mouseX, mouseY, cameraX, cameraY, sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        sensitivity = 600;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GUIManager.instance.GetToggle()){
            mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
            mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

            cameraY += mouseX;
            cameraX -= mouseY;

            cameraX = Mathf.Clamp(cameraX, -90f, 90f);
            transform.rotation = Quaternion.Euler(cameraX, cameraY, 0);
            cameraTransform.rotation = Quaternion.Euler(0, cameraY, 0);
        }
        /*Vector3 cameraEndPosition = PlayerManager.playerTransform.position + cameraOffset;
        Vector3 newCameraPosition = Vector3.Lerp(cameraTransform.position, cameraEndPosition, Time.deltaTime);

        cameraTransform.position = newCameraPosition;*/
    }
}
