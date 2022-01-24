using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform cameraFollowTrans;
    [SerializeField] float RotateSpeed = 20.0f;
    private Camera MainCamera;
    [SerializeField] [Range(0,1)]float RotateDamping = 0.5f;
    [SerializeField] [Range(0,1)]float MoveDamping = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
    }

    public void UpdateCamera(Vector3 playerPos, Vector2 moveInput, bool LockCamera)
    {
        transform.position = playerPos;
        if(!LockCamera)
        {
            transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime * moveInput.x);
        }
        //make the actual camera follow


        //use lerping instead of a hard set to achieve damping
        MainCamera.transform.position = cameraFollowTrans.position;
        MainCamera.transform.rotation = cameraFollowTrans.rotation;
        //Vector3 OriginalPosition = MainCamera.transform.position;
        //Quaternion OriginalRotation = MainCamera.transform.rotation;


    }
}
