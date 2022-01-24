using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [Header("Walking")]
    [SerializeField] float WalkingSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float EdgeCheckTracingDistance = 0.8f;
    [SerializeField] float EdgeCheckTracingDepth = 1f;

    [Header("Ground Check")]
    [SerializeField] Transform GroundCheck;
    [SerializeField] float GroundCheckRadius = 0.1f;
    [SerializeField] LayerMask GroundLayerMask;


    Vector2 MoveInput;
    Vector2 AimInput;
    Vector3 Velocity;
    float Gravity = -9.8f;
    CharacterController characterController;
    bool LookAtCursor = true;
    Vector2 CursorPos;

    #region Floor Follow
    Transform currentFloor;
    Vector3 PreviousWorldPos;
    Vector3 PreviousFloorLocalPos;
    Quaternion PreviousWorldRot;
    Quaternion PreviousFloorLocalRot;
    #endregion

    bool isClimbing;
    Vector3 ClimbingLadderDir;

    public void SetCursorPos(Vector2 cursorPos)
    {
        CursorPos = cursorPos;
    }

    public void SetAimInput(Vector2 input)
    {
        AimInput = input;
    }

    Vector3 GetCursorDir()
    {
        Ray ray = Camera.main.ScreenPointToRay(CursorPos);
        float CameraHeight = Camera.main.transform.position.y - (transform.position.y + characterController.height);
        float CosAngle = Vector3.Dot(Vector3.down, ray.direction);
        float traceDistance = CameraHeight / CosAngle;

        Vector3 testPos = ray.GetPoint(traceDistance);
        Vector3 DirToTestPosition = testPos - transform.position;
        Vector3 flatDirToTestPos = new Vector3(DirToTestPosition.x, 0, DirToTestPosition.z);
        Debug.DrawLine(transform.position, transform.position + flatDirToTestPos);
        return flatDirToTestPos.normalized;
    }

    public void SetClimbingStatus(bool climbing, Vector3 climbingLadderDir)
    {
        isClimbing = climbing;
        ClimbingLadderDir = climbingLadderDir;
    }
    public void SetMovementInput(Vector2 newInput)
    {
        MoveInput = newInput;
    }
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void ClearVetricalVelocity()
    {
        Velocity.y = 0;
    }

    private void Update()
    {
        if (isClimbing)
        {
            CalculateClimbingVelocity();
        }
        else
        {
            CaculateWalkingVelocity();
        }

        CheckFloor();
        FollowFloor();
        characterController.Move(Velocity * Time.deltaTime);
        UpdateRotation();
        SnapShotPostionAndRotation();
    }

    private void CaculateWalkingVelocity()
    {
        if (IsOnGround())
        {
            Velocity.y = -0.2f;
        }

        Velocity = GetPlayerDesiredMoveDir() * WalkingSpeed;
        Velocity.y += Gravity * Time.deltaTime;

        Vector3 PosXTracePos = transform.position + new Vector3(EdgeCheckTracingDistance, 0.5f, 0f);
        Vector3 NegXTracePos = transform.position + new Vector3(-EdgeCheckTracingDistance, 0.5f, 0f);
        Vector3 PosZTracePos = transform.position + new Vector3(0f, 0.5f, EdgeCheckTracingDistance);
        Vector3 NegZTracePos = transform.position + new Vector3(0f, 0.5f, -EdgeCheckTracingDistance);

        bool CanGoPosX = Physics.Raycast(PosXTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoNegX = Physics.Raycast(NegXTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoPosZ = Physics.Raycast(PosZTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoNegZ = Physics.Raycast(NegZTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);

        float xMin = CanGoNegX ? float.MinValue : 0f;
        float xMax = CanGoPosX ? float.MaxValue : 0f;
        float zMin = CanGoNegZ ? float.MinValue : 0f;
        float zMax = CanGoPosZ ? float.MaxValue : 0f;

        Velocity.x = Mathf.Clamp(Velocity.x, xMin, xMax);
        Velocity.z = Mathf.Clamp(Velocity.z, zMin, zMax);
    }

    public Vector3 GetPlayerDesiredMoveDir()
    {
        return InputAxisToWorldDir(MoveInput);
    }


    private Vector3 InputAxisToWorldDir(Vector2 input)
    {
        //base on the camera's right
        Vector3 CameraRight = Camera.main.transform.right;//?

        //you need to use cross product to find the frame up
        Vector3 FrameUp = Vector3.Cross(CameraRight, transform.up);


        return CameraRight * input.x + FrameUp * input.y;
    }

    public Vector3 GetPlayerDesiredLookDir()
    {
        if (AimInput.magnitude > 0)
        {
            return InputAxisToWorldDir(AimInput);
        } else
        {
            return GetPlayerDesiredMoveDir();
        }
    }

    void UpdateRotation()
    {
        if (isClimbing)
        {
            return;
        }
        Vector3 PlayerDesiredDir = GetPlayerDesiredLookDir();
        if (PlayerDesiredDir.magnitude == 0)
        {
            PlayerDesiredDir = transform.forward;
        }

        Quaternion DesiredRotation = Quaternion.LookRotation(PlayerDesiredDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, DesiredRotation, Time.deltaTime * rotationSpeed);
    }

    void CalculateClimbingVelocity()
    {
        if (MoveInput.magnitude == 0)
        {
            Velocity = Vector3.zero;
            return;
        }

        Vector3 PlayerDesiredMoveDir = GetPlayerDesiredMoveDir();

        float Dot = Vector3.Dot(ClimbingLadderDir, PlayerDesiredMoveDir);

        Velocity = Vector3.zero;
        if (Dot < 0)
        {
            Velocity = GetPlayerDesiredMoveDir() * WalkingSpeed;
            Velocity.y = WalkingSpeed;
        }
        else
        {
            if (IsOnGround())
            {
                Velocity = GetPlayerDesiredMoveDir() * WalkingSpeed;
            }
            Velocity.y = -WalkingSpeed;
        }
    }
    bool IsOnGround()
    {
        return Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
    }

    void CheckFloor()
    {
        Collider[] cols = Physics.OverlapSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
        if (cols.Length != 0)
        {
            if (currentFloor != cols[0].transform)
            {
                currentFloor = cols[0].transform;
                SnapShotPostionAndRotation();
            }
        }
    }
    void FollowFloor()
    {
        if (currentFloor)
        {
            Vector3 DeltaMove = currentFloor.TransformPoint(PreviousFloorLocalPos) - PreviousWorldPos;
            Velocity += DeltaMove / Time.deltaTime;

            Quaternion DestinationRot = currentFloor.rotation * PreviousFloorLocalRot;
            Quaternion DeltaRot = Quaternion.Inverse(PreviousWorldRot) * DestinationRot;
            transform.rotation = transform.rotation * DeltaRot;
        }
    }
    void SnapShotPostionAndRotation()
    {
        PreviousWorldPos = transform.position;
        PreviousWorldRot = transform.rotation;
        if (currentFloor != null)
        {
            PreviousFloorLocalPos = currentFloor.InverseTransformPoint(transform.position);
            PreviousFloorLocalRot = Quaternion.Inverse(currentFloor.rotation) * transform.rotation;
            //to add 2 rotation you do QuaternionA * QuaternionB
            //to subtract you do Quaternion.Inverse(QuaternionA) * QuaternionB
        }
    }

    public IEnumerator MoveToTransform(Transform Destination, float transformTime)
    {
        Vector3 StartPos = transform.position;
        Vector3 EndPos = Destination.position;
        Quaternion StartRot = transform.rotation;
        Quaternion EndRot = Destination.rotation;

        float timmer = 0f;
        while (timmer < transformTime)
        {
            timmer += Time.deltaTime;
            Vector3 DeltaMove = Vector3.Lerp(StartPos, EndPos, timmer / transformTime) - transform.position;
            characterController.Move(DeltaMove);
            transform.rotation = Quaternion.Lerp(StartRot, EndRot, timmer / transformTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
