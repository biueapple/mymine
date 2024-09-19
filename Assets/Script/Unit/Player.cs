using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    private IPlayerInputSystem playerInput;
    public IPlayerInputSystem PlayerInput 
    { get { return playerInput; } 
        set 
        {
            playerInput?.Exit();
            playerInput = value;
            playerInput?.Enter();
        } 
    }

    [SerializeField]
    private Transform hand;
    public Transform Hand { get { return hand; } }
    [SerializeField]
    private Transform head;
    public Transform Head { get { return head; } }
    [SerializeField]
    private Transform aming;
    public Transform Aming { get {  return aming; } }

    [SerializeField]
    private Camera cam;
    public Camera Camera { get { return cam; } }

    private InventorySystem inventory;
    public InventorySystem InventorySystem { get { return inventory; } }
    private EquipmentSystem equipment;
    public EquipmentSystem EquipmentSystem { get { return equipment; } }

    private SkillSystem skillSystem;
    public SkillSystem SkillSystem { get { return skillSystem; } }

    private Animator animator;
    public Animator Animator { get { return animator; } }

    private PlayerInputStateNomal stateNomal;
    public PlayerInputStateNomal StateNomal { get { return stateNomal; } }
    private PlayerInputStateBattle stateBattle;
    public PlayerInputStateBattle StateBattle { get { return stateBattle; } }
    private readonly RunStateShift runStateShift = new();

    [SerializeField]
    private AttackMotionInterface attackMotionInterface;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        inventory = GetComponent<InventorySystem>();
        equipment = GetComponent<EquipmentSystem>();
        skillSystem = GetComponent<SkillSystem>();

        moveSystem.AddMoveMode(new InputKeyMove(stat));
        moveSystem.AddMoveMode(new InputMouseMove(head, transform, -70, 70));
        JumpInputMove jumpSystem = new(moveSystem.Machine);

        moveSystem.AddMoveMode(new DoubleJumpInputMove(moveSystem.Machine, jumpSystem));

        //inputLimit.Add(new Provoke2());

        moveSystem.AddMoveMode(new WalkStateShift());

        moveSystem.AddMoveMode(new GravityForce(moveSystem.Machine));
        //new DisposableExternal(new Vector3(0, 6, -3)),
        //externalForces.Add(new TimerExternal(new Vector3(-1, 0, 0), 3, 3));
        //externalForces.Add(new ContinuedExternal(new Vector3(1, 0, 0), 3));

        moveSystem.AddMoveMode(new WorldMovePhysicsShift(moveSystem.Machine, GetComponent<Unit>()));


        //플레이어 노말모드
        stateNomal = new PlayerInputStateNomal(this, inventory);
        stateBattle = new PlayerInputStateBattle(this, equipment, attackMotionInterface);
        playerInput = stateNomal;
    }

    // Update is called once per frame
    void Update()
    {

        playerInput?.Update();

        if(Input.GetMouseButton(0))
        {
            playerInput.LeftClick();
        }
        else if(Input.GetMouseButton(1))
        {
            playerInput.RightClick();
        }

        if(Input.GetMouseButtonDown(0))
        {
            playerInput?.LeftDown();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            playerInput?.RightDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerInput?.LeftUp();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            playerInput?.RightUp();
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            ModeChange();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Aiming();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSystem.AddMoveMode(runStateShift);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSystem.RemoveMoveMode(runStateShift);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject gameObject = UIManager.Instance.CloseUI();
            if (gameObject == inventory.Inventory.gameObject)
            {
                SlotManager.Instance.ItemCatchState.Another();
            }
            else if(gameObject == equipment.StaticInterface.gameObject)
            {
                SlotManager.Instance.ItemCatchState.Another();
            }
            else if(gameObject == null)
            {
                Debug.Log("설정창 열기");
            }
        }
    }

    ///

    private void ModeChange()
    {
        if (PlayerInput.Mode == stateNomal)
        {
            PlayerInput = stateBattle;
            inventory.HotkeyInterface.gameObject.SetActive(false);
            skillSystem.SkillInterface.gameObject.SetActive(true);
        }
        else
        {
            PlayerInput = stateNomal;
            inventory.HotkeyInterface.gameObject.SetActive(true);
            skillSystem.SkillInterface.gameObject.SetActive(false);
        }
    }
    
    public void Aiming(PlayerInputStateAming playerInputStateAming)
    {
        if (playerInput.Mode == stateBattle)
        {
            PlayerInput = playerInputStateAming;
        }
    }

    //test용 
    [SerializeField]
    private ProjectileObject projectile;
    private void Aiming()
    {
        if (playerInput.Mode == stateBattle)
        {
            //플레이어 발사모드
            ProjectileObject[] projectileObjects = new ProjectileObject[3];
            for (int i = 0; i < projectileObjects.Length; i++)
            {
                ProjectileObject projectileObject = Instantiate(projectile);
                //substance와 부딫히고 삭제되는 판정
                DestroyToProjectile destroy = new(this, projectileObject.gameObject);
                //월드에 존재하는 블록과 충돌하면 삭제되는 판정
                WorldCollisionToProjectile collisionToProjectile = new (projectileObject, destroy);
                //움직이는 효과
                MoveToProjectile move = new(projectileObject.transform, 5, collisionToProjectile);
                //대미지를 넣는 효과
                AttackInformation attackInformation = new (STAT, AttackType.SPECIAL);
                attackInformation.Additional.Add(new (10, DamageType.AP, false));
                DamageToProjectile damage = new(this, move, attackInformation);
                //얼리는 효과를 만들고
                IceEffectToProjectile iceEffect = new(this, damage);

                projectileObject.Projectile = iceEffect;
                projectileObject.gameObject.SetActive(false);
                projectileObjects[i] = projectileObject;
            }
            PlayerInput = new PlayerInputStateAming(cam, this, UIManager.Instance.Canvas, projectileObjects, 3);
        }
    }

}
