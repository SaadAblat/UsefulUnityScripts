using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] PlayerScript ps;

    public bool playerIsWaitingForTheAxeToReturn;
    public bool AxeEquiped;
    public bool PlayerHaveTheAxe;

    public bool PlayerHaveTheSword;
    public bool SwordEquiped;

    public bool holdingWeapon;

    List<GameObject> weaponsInHand = new List<GameObject>();
    List<GameObject> weaponsInBack = new List<GameObject>();

    [SerializeField] GameObject AxeInHand;
    [SerializeField] GameObject AxeInTheBack;

    [SerializeField] GameObject SwordInHand;
    [SerializeField] GameObject SwordInBack;

    float axeRegenerationTimer;
    [SerializeField] float timeToRegenerateAxe;
    [SerializeField] float axeMovingBackSpeed;
    bool axeIsInTheWay;
    // Start is called before the first frame update
    void Start()
    {
        axeRegenerationTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ps.Controller.IsAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayerWeaponName = PlayerWeaponList.axe;
                ps.Controller.IsAttacking = false;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayerWeaponName = PlayerWeaponList.sword;
                ps.Controller.IsAttacking = false;
            }
        }
       

        // Weapons gameObject both hand and 
        if (PlayerHaveTheAxe)
        {
            if (!weaponsInHand.Contains(AxeInHand))
            {
                weaponsInHand.Add(AxeInHand);
                weaponsInBack.Add(AxeInTheBack);
            }
        }
        else
        {
            AxeEquiped = false;
            AxeInHand.SetActive(false);
            AxeInTheBack.SetActive(false);
            if (weaponsInHand.Contains(AxeInHand))
            {

                weaponsInHand.Remove(AxeInHand);
                weaponsInBack.Remove(AxeInTheBack);
            }
        }
        if (PlayerHaveTheSword)
        {
            if (!weaponsInHand.Contains(SwordInHand))
            {
                weaponsInHand.Add(SwordInHand);
                weaponsInBack.Add(SwordInBack);
            }
        }
        else
        {
            SwordEquiped = false;
            SwordInHand.SetActive(false);
            SwordInBack.SetActive(false);
            if (weaponsInHand.Contains(SwordInHand))
            {

                weaponsInHand.Remove(SwordInHand);
                weaponsInBack.Remove(SwordInBack);
            }
        }

        // Holding Weapon boolean
        if (SwordEquiped || (AxeEquiped && !playerIsWaitingForTheAxeToReturn))
        {
            holdingWeapon = true;
        }
        else
        {
            holdingWeapon = false;
        }
        if (PlayerHaveTheAxe && playerIsWaitingForTheAxeToReturn)
        {
            axeRegenerationTimer += Time.deltaTime;
            if (axeRegenerationTimer > timeToRegenerateAxe)
            {
                axeRegenerationTimer = 0;
                axeIsInTheWay = true;

            }
        }
        if (axeIsInTheWay)
        {
            CallBackTheAxe();
            axeRegenerationTimer = 0;
        }


        ManageWeapons();

       

      
        
    }

    void CallBackTheAxe()
    {
       
        if (FindObjectOfType<Axe>() != null)
        {
            GameObject axe = FindObjectOfType<Axe>().gameObject;
            axe.GetComponent<Axe>().enabled = false;
            foreach( var collider in axe.GetComponentsInChildren<CapsuleCollider>())
            {
                collider.enabled = false;
            }
            axe.transform.position = Vector3.MoveTowards(axe.transform.position, transform.position, axeMovingBackSpeed * Time.deltaTime);

        }
        else
        {
            axeIsInTheWay = false;
        }
       
    }

    public enum PlayerWeaponList
    {
        axe,
        spear,
        sword,
        none
            
    }
    public PlayerWeaponList PlayerWeaponName;
    void ManageWeapons()
    {
        switch(PlayerWeaponName)
        {
            case PlayerWeaponList.axe:
                AxeEquiped = true;
                SwordEquiped = false;
                if (playerIsWaitingForTheAxeToReturn)
                {
                    AxeInHand.SetActive(false);
                    AxeInTheBack.SetActive(false);
                }
                else
                {
                    WeaponPlacement(AxeInHand, AxeInTheBack);
                }

                foreach(var weaponH in weaponsInHand)
                {
                    if (weaponH != AxeInHand)
                    {
                        weaponH.SetActive(false);
                    }
                }
                foreach (var weaponB in weaponsInBack)
                {
                    if (weaponB != AxeInTheBack)
                    {
                        weaponB.SetActive(true);
                    }
                }
                break;
            case PlayerWeaponList.sword:
                SwordEquiped = true;
                AxeEquiped = false;
                foreach (var weaponH in weaponsInHand)
                {
                    if (weaponH != SwordInHand)
                    {
                        weaponH.SetActive(false);
                    }
                }
                foreach (var weaponB in weaponsInBack)
                {
                    if (weaponB != SwordInBack)
                    {
                        if (!(weaponB == AxeInTheBack && playerIsWaitingForTheAxeToReturn))
                        {
                            weaponB.SetActive(true);
                        }
                     
                    }
                }
                WeaponPlacement(SwordInHand, SwordInBack); break;
            case PlayerWeaponList.none:
                EmptyHandsAndBack(); break;

        }
    }

    void WeaponPlacement(GameObject WeaponInHand, GameObject WeaponInTheBack)
    {
        if (ps.ClimbController.enabled || ps.Controller.isPushing || (!ps.Controller.IsGrounded() && !ps.Controller.IsAttacking) )
        {
            WeaponInHand.SetActive(false);
            WeaponInTheBack.SetActive(true);
        }
        else
        {
            WeaponInHand.SetActive(true);
            WeaponInTheBack.SetActive(false);
        }
    }
    void EmptyHandsAndBack()
    {
        AxeInHand.SetActive(false);
        AxeInTheBack.SetActive(false);
        SwordInHand.SetActive(false);
        SwordInBack.SetActive(false);
    }



    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Axe") && !ps.Controller.IsAttacking)
        {
            AxeEquiped = true;
            playerIsWaitingForTheAxeToReturn = false;
            PlayerHaveTheAxe = true;
            Destroy(collision.gameObject);
        }
    }

}
