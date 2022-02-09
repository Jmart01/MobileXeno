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

    [SerializeField] JoyStick moveStick;
    [SerializeField] JoyStick aimStick;
    CameraManager cameraManager;

    [SerializeField] Weapon[] StartWeaponPrefabs;
    [SerializeField] Transform GunSocket;
    List<Weapon> Weapons = new List<Weapon>();
    Weapon CurrentWeapon;
    int currentWeaponIndex = 0;
    bool PlayerAlive = true;

    AbilityComponent abilityComp;
    AbilityWheel abilityWheel;
    CreditSystem creditSystem;

    private void Awake()
    {
        inputActions = new InputActions();
        abilityComp = GetComponent<AbilityComponent>();
        creditSystem = GetComponent<CreditSystem>();
        abilityWheel = FindObjectOfType<AbilityWheel>();
        if(abilityComp != null)
        {
            abilityComp.onNewAbilityInitialized += NewAbilityAdded;
            abilityComp.onStaminaUpdated += StaminaUpdated;
        }
    }

    internal void AquireNewWeapon(Weapon weapon, bool Equip = false)
    {
        Weapon newWeapon = Instantiate(weapon, GunSocket);
        newWeapon.Owner = gameObject;
        newWeapon.UnEquip();
        Weapons.Add(newWeapon);
        if(Equip)
        {
            EquipWeapon(Weapons.Count-1);
        }
    }

    private void StaminaUpdated(float newValue)
    {
        abilityWheel.UpdateStamina(newValue);
    }

    private void NewAbilityAdded(AbilityBase newAbility)
    {
        AbilityWheel abilityWheel = FindObjectOfType<AbilityWheel>();
        abilityWheel.AddNewAbility(newAbility);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void DisableInputs()
    {
        OnDisable();
    }

    void InitializeWeapons()
    {
        foreach (Weapon weapon in StartWeaponPrefabs)
        {
            AquireNewWeapon(weapon);
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
            animator.SetFloat("FiringSpeed", CurrentWeapon.GetShootingSpeed());
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
        cameraManager = FindObjectOfType<CameraManager>();

        abilityWheel.UpdateStamina(abilityComp.GetStaminaLevel());
    }

    private void NextWeapon(InputAction.CallbackContext obj)
    {
        SwapWeapon();
    }

    public void SwapWeapon()
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
        StopFire();
    }

    private void Fire()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1);
        
    }

    private void StopFire()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0);
    }

    private void MainActionButtonDown(InputAction.CallbackContext obj)
    {
        Fire();
    }

    private void CursorPosUpdated(InputAction.CallbackContext obj)
    {
        movementComp.SetCursorPos(obj.ReadValue<Vector2>());
    }

    private void MoveInputUpdated(InputAction.CallbackContext ctx)
    {

        Vector2 input = ctx.ReadValue<Vector2>().normalized;
        UpdateMovement(input);
    }

    private void UpdateMovement(Vector2 input)
    {
        movementComp.SetMovementInput(input);
        if (input.magnitude == 0)
        {
            BackToIdleCoroutine = StartCoroutine(DelayedBackToIdle());
        }
        else
        {
            if (BackToIdleCoroutine != null)
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
        if(PlayerAlive == true)
        {
            UpdateAnimation();
            UpdateMovestickInput();
            UpdateCamera();
            UpdateAimStickInput();
        }
    }

    private void UpdateCamera()
    {

        cameraManager.UpdateCamera(transform.position, moveStick.Input, aimStick.Input.magnitude > 0);
    }

    public void FireTimePoint()
    {
        if(CurrentWeapon!=null)
        {
            CurrentWeapon.Fire();
        }
    }

    public void UpdateMovestickInput()
    {
        if(PlayerAlive == true)
        {
            if (moveStick != null)
            {
                UpdateMovement(moveStick.Input);
            }
        }
    }
    public void UpdateAimStickInput()
    {
        if(PlayerAlive == true)
        {
            if (aimStick != null)
            {
                movementComp.SetAimInput(aimStick.Input);
                if (aimStick.Input.magnitude > 0)
                {
                    Fire();
                }
                else
                {
                    StopFire();
                }
            }
        }
    }
    public override void NoHealthLeft(GameObject killer)
    {
        base.NoHealthLeft();
        PlayerAlive = false;
        OnDisable();
    }
}
