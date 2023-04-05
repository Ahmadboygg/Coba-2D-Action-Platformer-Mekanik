using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player instance;
    public Animator animator;
    public Rigidbody2D rigidbody2D;
    private MainInput inputActions;
    [SerializeField] private Transform groundDetector = null;
    [SerializeField] private LayerMask groundLayer = 0;

    public bool isAttack = false;
    public bool isCombo = false;
    public bool isGrounded;
    public bool isRotate;
    private float comboTimer = 1f;
    private float currentComboTimer;
    private float attackTimer = 0.8f;
    private float currentAttackTimer = 0.8f;
    [SerializeField] private float jumpPower = 4f;
    [SerializeField] private float movementSpeed = 1f;
    private Vector2 _moveInput
    {
        get => moveInput;
        set
        {
            moveInput = value;

            if(moveInput.x < 0)
            {
                transform.rotation = Quaternion.Euler(0,180,0);
            }
            else if(moveInput.x > 0)
            {
                transform.rotation = Quaternion.Euler(0,0,0);
            }
        }
    }
    public Vector2 moveInput;
    void Awake()
    {
        instance = this;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inputActions = new MainInput();
        inputActions.PlayerInput.Enable();
        inputActions.PlayerInput.Attack.performed += AttackHandle;
        inputActions.PlayerInput.Jump.performed += JumpHandle;
    }

    void Update()
    {
        ComboTimerHandle();
        AttackTimerHandle();
        RaycastHit2D onGround = Physics2D.Raycast(groundDetector.position,Vector2.down,0.05f,groundLayer);
        isGrounded = onGround.collider != null ? isGrounded = true : isGrounded = false;
        _moveInput = inputActions.PlayerInput.Move.ReadValue<Vector2>();
        rigidbody2D.velocity = new Vector2(moveInput.x * movementSpeed, rigidbody2D.velocity.y);
        
    }

    private void AttackHandle(InputAction.CallbackContext context)
    {
        if(context.performed && !isAttack)
        {
            isAttack = true;
            isCombo = true;
            currentComboTimer = comboTimer;
            currentAttackTimer = attackTimer;
        }
    }

    private void ComboTimerHandle()
    {
        if(currentComboTimer > 0)
        {
            currentComboTimer -= Time.deltaTime;
        }
        else
        {
            isCombo = false;
        }
    }

    private void AttackTimerHandle()
    {
        if(currentAttackTimer > 0)
        {
            currentAttackTimer -= Time.deltaTime;
        }
        else
        {
            isAttack = false;
        }
    }

    private void MoveHandle()
    {
        rigidbody2D.velocity = new Vector2(moveInput.x * movementSpeed, rigidbody2D.velocity.y);
        if(moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        else if(moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0,0,0);

        }
    }

    private void JumpHandle(InputAction.CallbackContext context)
    {
        if(isGrounded && context.performed)
        {
            rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
}
