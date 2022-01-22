using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    MovementComponent movementComp;
    InputActions inputActions;
    Animator animator;
    int speedHash = Animator.StringToHash("speed");
    Coroutine BackToIdleCoroutine;
    InGameUI inGameUI;

    [SerializeField] Weapon[] StartWeaponPrefabs;
    [SerializeField] Transform GunSocket;
    List<Weapon> Weapons;
    Weapon CurrentWeapon;
    int currentWeaponIndex = 0;
    private void Awake()
    {
        inputActions = new InputActions();
        //Black bitches be the finest of bitches! Punch my grandma in the throat, kick my daddy's shin, elbow slap my mama's face in
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    void InitializeWeapons()
    {
        Weapons = new List<Weapon>();
        foreach (Weapon weapon in StartWeaponPrefabs)
        {
            Weapon newWeapon = Instantiate(weapon, GunSocket);
            newWeapon.Owner = gameObject;
            newWeapon.UnEquip();
            Weapons.Add(newWeapon);
        }
        EquipWeapon(0);
    }

    void EquipWeapon(int weaponIndex)
    {
        if (Weapons.Count > weaponIndex)
        {
            if(CurrentWeapon!=null)
            {
                CurrentWeapon.UnEquip();
            }

            currentWeaponIndex = weaponIndex;
            Weapons[weaponIndex].Equip();
            CurrentWeapon = Weapons[weaponIndex];
            if(inGameUI!=null)
            {
                inGameUI.SwichedWeaponTo(CurrentWeapon);
            }
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        inGameUI = FindObjectOfType<InGameUI>();
        movementComp = GetComponent<MovementComponent>();
        animator = GetComponent<Animator>();
        inputActions.Gameplay.CursorPos.performed += CursorPosUpdated;
        inputActions.Gameplay.move.performed += MoveInputUpdated;
        inputActions.Gameplay.move.canceled += MoveInputUpdated;
        inputActions.Gameplay.MainAction.performed += MainActionButtonDown;
        inputActions.Gameplay.MainAction.canceled += MainActionReleased;
        inputActions.Gameplay.Space.performed += BigAction;
        inputActions.Gameplay.NextWeapon.performed += NextWeapon;
        animator.SetTrigger("BackToIdle");
        InitializeWeapons();


    }

    private void NextWeapon(InputAction.CallbackContext obj)
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % Weapons.Count;
        EquipWeapon(currentWeaponIndex);
    }

    private void BigAction(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("BigAction");
    }

    private void MainActionReleased(InputAction.CallbackContext obj)
    {
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0);
    }

    private void MainActionButtonDown(InputAction.CallbackContext obj)
    {
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1);
    }

    private void CursorPosUpdated(InputAction.CallbackContext obj)
    {
        movementComp.SetCursorPos(obj.ReadValue<Vector2>());
    }

    private void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        
        Vector2 input = ctx.ReadValue<Vector2>().normalized;
        movementComp.SetMovementInput(input);
        if(input.magnitude==0)
        {
            BackToIdleCoroutine = StartCoroutine(DelayedBackToIdle());
        }else
        {
            if(BackToIdleCoroutine!=null)
            {
                StopCoroutine(BackToIdleCoroutine);
                BackToIdleCoroutine = null;
            }
        }
    }

    IEnumerator DelayedBackToIdle()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("BackToIdle");
    }

    void UpdateAnimation()
    {
        animator.SetFloat(speedHash, GetComponent<CharacterController>().velocity.magnitude);
        Vector3 PlayerForward = movementComp.GetPlayerDesiredLookDir();
        Vector3 PlayerMoveDir = movementComp.GetPlayerDesiredMoveDir();
        Vector3 PlayerLeft = Vector3.Cross(PlayerForward, Vector3.up);
        float forwardAmt = Vector3.Dot(PlayerForward, PlayerMoveDir);
        float leftAmt = Vector3.Dot(PlayerLeft, PlayerMoveDir);

        animator.SetFloat("forwardSpeed", forwardAmt);
        animator.SetFloat("leftSpeed", leftAmt);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        UpdateAnimation();
    }
    
    public void FireTimePoint()
    {
        if(CurrentWeapon!=null)
        {
            CurrentWeapon.Fire();
        }
    }

    public void SetJoystickData(Vector2 data)
    {
        movementComp.SetSpeedMulti(data);
        data = Vector2.ClampMagnitude(data, 1);
        movementComp.SetMovementInput(data);
    }
}
