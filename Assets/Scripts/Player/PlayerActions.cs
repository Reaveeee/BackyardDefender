using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class PlayerActions : MonoBehaviour
{
    public int invSlot = 0; // 0 = Schwert, 1 = Gieﬂkanne, 2 = Samen, 3 = Harke
    int lastInvSlot = 3;
    public int seedSlot = 0; // 0 = Standart, 1 = Geld
    public GameObject[] seeds;
    public GameObject seed;
    float mouseScroll;
    float attackDelay = 0.3f;
    float attackDelayTimer;
    float swordTimer;
    float swordLifetime = 0.1f;
    float attackOffset = 1.5f;
    float attackVelocityMultiplier = 0.01f;
    bool altScrolling;
    Vector3 mousePos;
    public Vector3 rotation;

    [SerializeField] Image slot0Image;
    [SerializeField] Image slot1Image;
    [SerializeField] Image slot2Image;
    [SerializeField] GameObject swordHitbox;
    [SerializeField] GameObject fieldPrefab;
    [SerializeField] GameObject fieldParent;
    GameObject swordObject;
    GameObject playerSprite;
    Camera cam;
    Rigidbody2D rigidbody;
    GameObject generatedField;
    GameManager gameManager;
    Vector3 aimedCell;
    PlayerManager playerManager;

    public event Action OnInvSwitch;
    public event Action OnMeleeAttack;
    public event Action OnLeftClick;

    void Start()
    {
        playerSprite = GetComponentInChildren<SortingLayer>().gameObject;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        OnInvSwitch.Invoke();
        OnLeftClick.Invoke();
        playerManager = GetComponent<PlayerManager>();
    }


    void Update()
    {
        if (gameManager.gameOver)
        {
            Destroy(this);
        }

        //Timer
        swordTimer -= Time.deltaTime;
        attackDelayTimer -= Time.deltaTime;

        //Button Inputs
        mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        altScrolling = Input.GetKey(KeyCode.LeftControl);
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftClick.Invoke();
        }

        //Mouse Rotation
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        rotation = mousePos - playerSprite.transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        //Inventory Scrolling
        if (mouseScroll != 0 && !altScrolling)
        {
            HorizontalInvScrolling();

        }
        if (mouseScroll != 0 && altScrolling)
        {
            VerticalInvScrolling();
        }

        //Leftclick
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //MeleeAttack
            if (invSlot == 0 && swordTimer <= 0)
            {
                MeleeAttack(rotZ);
            }

            //Create Field
            if (invSlot == 3 && gameManager.gameState == 0)
            {
                aimedCell = new Vector3(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y), 0);
                CreateField(aimedCell);
            }
        }

        if (swordTimer <= 0)
        {
            Destroy(swordObject);
        }
    }

    void HorizontalInvScrolling()
    {
        if (mouseScroll > 0)
        {
            invSlot--;
            if (invSlot < 0)
            {
                invSlot = lastInvSlot;
            }
            OnInvSwitch.Invoke();
        }
        else if (mouseScroll < 0)
        {
            invSlot++;
            if (invSlot > lastInvSlot)
            {
                invSlot = 0;
            }
            OnInvSwitch.Invoke();
        }
    }

    void VerticalInvScrolling()
    {
        if (invSlot == 2)
        {
            if (mouseScroll > 0)
            {
                seedSlot--;
                if (seedSlot < 0)
                {
                    seedSlot = 1;
                }
                seed = seeds[seedSlot];
                OnInvSwitch.Invoke();
            }
            if (mouseScroll < 0)
            {
                seedSlot++;
                if (seedSlot > 1)
                {
                    seedSlot = 0;
                }
                seed = seeds[seedSlot];
                OnInvSwitch.Invoke();
            }
        }
    }

    void MeleeAttack(float pRotZ)
    {
        if(attackDelayTimer <= 0)
        {
            swordObject = Instantiate(swordHitbox, playerSprite.transform.position + new Vector3(mousePos.x - playerSprite.transform.position.x, mousePos.y - playerSprite.transform.position.y, 0).normalized * attackOffset + new Vector3(rigidbody.velocity.x * attackVelocityMultiplier, rigidbody.velocity.y * attackVelocityMultiplier, 100), Quaternion.Euler(0, 0, pRotZ - 90));
            swordTimer = swordLifetime;
            attackDelayTimer = attackDelay;
            OnMeleeAttack.Invoke();
        }
    }

    void CreateField(Vector3 targetPos)
    {
        generatedField = Instantiate(fieldPrefab, targetPos, Quaternion.identity, fieldParent.transform);

        if (GameObject.Find("Field: " + generatedField.transform.position.x + "_" + generatedField.transform.position.y) == null && (targetPos.x > 1 || targetPos.x < -1 || targetPos.y > 1 || targetPos.y < -1) && gameManager.GetDistance(gameObject, generatedField) <= 5)
        {
            generatedField.name = "Field: " + generatedField.transform.position.x + "_" + generatedField.transform.position.y;  
        }
        else
        {
            Debug.Log("Invalid Field Position");
            Destroy(generatedField);
        }
    }
}