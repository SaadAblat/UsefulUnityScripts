using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script that connect all the other script together
/// </summary>
public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D PlayerRigideBody;
    
    public static PlayerScript Instance { get; private set; }

    Collider2D Playercollider;

    //Player Scripts :
    [Header("Player Scripts")]
    [SerializeField] internal PlayerController playerController;
    [SerializeField] internal PlayerInputs playerInputs;
    [SerializeField] internal PlayerAnimation playerAnimation;

    internal bool PlayerIsDead = false;
    internal bool GotTheKey = false;
    internal bool OpenedTheDoor = false;

    public DeathType death;
    public Weapon weapon;


    [SerializeField] internal bool HoldingUmbrella;
    [SerializeField] internal bool HasMagicPower;
    [SerializeField] internal bool CanSlide;


    private void Awake()
    {
        Instance = this;

        weapon = Weapon.unarmed;

        Playercollider = GetComponent<CapsuleCollider2D>();

    }
    void Start()
    {
        GotTheKey = false;
        PlayerIsDead = false;
        OpenedTheDoor = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerIsDead)
        {
            PlayerDeath();
        }
    }
    void PlayerDeath()
    {

        switch (death)
        {
            case DeathType.spikes:
                PlayerRigideBody.isKinematic = true;
                PlayerRigideBody.velocity = Vector2.zero;
                PlayerRigideBody.freezeRotation = true;
                break;

            case DeathType.rollingSpikes:
                PlayerRigideBody.drag = 1;
                PlayerRigideBody.gravityScale = 1.5f;
                break;

            case DeathType.other:
                Destroy(gameObject);
                break;

        }
    }



    public enum Weapon
    {
        axe,
        spear,
        unarmed
    }
    public enum DeathType
    {
        spikes,
        rollingSpikes,
        other,
    }







}
