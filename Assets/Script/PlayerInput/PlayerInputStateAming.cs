using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputStateAming : IPlayerInputSystem
{
    public IPlayerInputSystem Mode { get => playerInput; set
        {
            Extinction();
            player.PlayerInput = value;
        }
    }
    private Player player;
    private IPlayerInputSystem playerInput;

    //쏠 투사체의 숫자
    private readonly ProjectileObject[] projectileObjects;
    //조준할 상대의 숫자 (한번의 발사로 모든 투사체를 발사함)
    private int count;

    private Rect rect;

    private Camera mainCamera;

    private Image aim;
    private List<Image> aimList = new();

    private List<Substance> enemy = new();
    public Substance[] Enemy { get { return enemy.ToArray(); } }

    public PlayerInputStateAming(Camera camera, Player player, Canvas canvas, ProjectileObject[] projectileObjects)
    {
        mainCamera = camera;

        this.player = player;
        playerInput = player.PlayerInput;

        rect.width = 1000;
        rect.height = 1000;
        rect.x = (Screen.width - rect.width) / 2;
        rect.y = (Screen.height - rect.height) / 2;

        this.projectileObjects = projectileObjects;
        count = 1;

        aim = Resources.Load<Image>("UI/AimStateImage");
        CreateAimImage(canvas.transform);
    }

    public PlayerInputStateAming(Camera camera, Player player, Canvas canvas, ProjectileObject[] projectileObjects, int count, float width = 1000, float height = 1000)
    {
        mainCamera = camera;

        this.player = player;
        playerInput = player.PlayerInput;

        rect.width = width;
        rect.height = height;
        rect.x = (Screen.width - rect.width) / 2;
        rect.y = (Screen.height - rect.height) / 2;

        this.projectileObjects = projectileObjects;
        this.count = count;

        aim = Resources.Load<Image>("UI/AimStateImage");
        CreateAimImage(canvas.transform);
    }

    public virtual void LeftClick() { }
    public virtual void RightClick() { }
    public virtual void LeftDown()
    {
        //발사
        Fire();
        Extinction();
    }
    public virtual void LeftUp() { }
    public virtual void RightDown() 
    {
        //캔슬
        Extinction();
    }
    public virtual void RightUp() { }
    //지금은 모든 유닛이 타겟임 (팀이 나눠있지 않기에)
    public virtual void Update()
    {
        List<(Substance enemy, float distance)> preliminary = new();
        //모든 유닛중에 팀이 enemy인것만
        Substance[] substances = GameManager.Instance.Substances.Where(x => x.Unit.STAT.Team == ETeam.Enemy).ToArray();
        //범위 안에 있는지
        for (int i = 0; i < substances.Length; i++)
        {
            Vector3 screenPosition = WorldToCamera(substances[i].transform.position);
            if (screenPosition.z > 0 && rect.Contains(screenPosition))
            {
                float distance = Vector2.Distance(screenPosition, new Vector2(rect.x, rect.y));
                preliminary.Add((substances[i], distance));
            }
        }
        //범위에서 중앙에 가장 가까운 순으로 정렬
        enemy = preliminary.OrderBy(e => e.distance).Take(count).Select(e => e.enemy).ToList();
        Aim();
    }

    //소멸자가 원하는 타이밍에 호출되지 않기에 만듬
    private void Extinction()
    {
        for (int i = 0; i < aimList.Count; i++)
            Object.Destroy(aimList[i].gameObject);
        player.PlayerInput = playerInput;
    }

    private void Fire()
    {
        for(int i = 0; i < projectileObjects.Length; i++)
        {
            projectileObjects[i].transform.position = player.Aming.position;
            projectileObjects[i].gameObject.SetActive(true);
            projectileObjects[i].transform.LookAt(enemy[i % enemy.Count].transform.position);
        }
    }

    private void Aim()
    {
        for (int i = 0; i < aimList.Count; i++)
        {
            if (i <= enemy.Count - 1)
            {
                aimList[i].gameObject.SetActive(true);
                aimList[i].transform.position = WorldToCamera(enemy[i].transform.position);
            }
            else
            {
                aimList[i].gameObject.SetActive(false);
            }
        }
    }

    private void CreateAimImage(Transform canvas)
    {
        for (int i = 0; i < count; i++)
        {
            aimList.Add(Object.Instantiate(aim, canvas));
            aimList[^1].gameObject.SetActive(false);
        }
    }

    private Vector3 WorldToCamera(Vector3 position)
    {
        return mainCamera.WorldToScreenPoint(position);
    }
}
