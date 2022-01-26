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
    [SerializeField] float MoveSpeed = 20f;



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

        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, cameraFollowTrans.position, (1-MoveDamping)*MoveSpeed*Time.deltaTime);
        MainCamera.transform.rotation = Quaternion.Lerp(MainCamera.transform.rotation, cameraFollowTrans.rotation, (1-RotateDamping) * RotateSpeed*Time.deltaTime);

    }
}
