using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputStateAming : IPlayerInputSystem
{
    public IPlayerInputSystem Mode { get => playerInput; }

    private readonly Player player;
    private readonly IPlayerInputSystem playerInput;

    //쏠 투사체의 숫자
    private readonly ProjectileObject[] projectileObjects;
    //조준할 상대의 숫자 (한번의 발사로 모든 투사체를 발사함)
    private readonly int count;

    private readonly Rect rect;

    private readonly Camera mainCamera;

    private readonly Image aim;
    private readonly List<Image> aimList = new();

    private List<Substance> enemy = new();
    public Substance[] Enemy { get { return enemy.ToArray(); } }

    private Sight sight;
    public PlayerInputStateAming(Camera camera, Player player, Canvas canvas, ProjectileObject[] projectileObjects)
    {
        mainCamera = camera;

        this.player = player;
        playerInput = player.PlayerInput.Mode;

        rect.width = 1000;
        rect.height = 1000;
        rect.x = (Screen.width - rect.width) / 2;
        rect.y = (Screen.height - rect.height) / 2;

        this.projectileObjects = projectileObjects;
        count = 1;

        sight = new Sight(camera, rect);

        aim = Resources.Load<Image>("UI/AimStateImage");
        CreateAimImage(canvas.transform);
    }

    public PlayerInputStateAming(Camera camera, Player player, Canvas canvas, ProjectileObject[] projectileObjects, int count, float width = 1000, float height = 1000)
    {
        mainCamera = camera;

        this.player = player;
        playerInput = player.PlayerInput.Mode;

        rect.width = width;
        rect.height = height;
        rect.x = (Screen.width - rect.width) / 2;
        rect.y = (Screen.height - rect.height) / 2;

        this.projectileObjects = projectileObjects;
        this.count = count;

        sight = new Sight(camera, rect);

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
        playerInput?.Update();

        sight.SightOn();
        enemy = sight.Values.Take(count).Select(x => x.enemy).ToList();

        Aim();
    }

    //소멸자가 원하는 타이밍에 호출되지 않기에 만듬
    private void Extinction()
    {
        for (int i = 0; i < aimList.Count; i++)
        {
            Debug.Log("오브젝트 풀링을 사용하지 않는 destroy");
            Debug.Log(aimList[i].gameObject);
            Object.Destroy(aimList[i].gameObject);
        }
        aimList.Clear();
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
            Debug.Log("오브젝트 풀링을 사용하지 않는 생성");
            aimList.Add(Object.Instantiate(aim, canvas));
            aimList[^1].gameObject.SetActive(false);
        }
    }

    private Vector3 WorldToCamera(Vector3 position)
    {
        return mainCamera.WorldToScreenPoint(position);
    }

    public void Enter()
    {

    }

    public void Exit()
    {
        //캔슬
        for (int i = 0; i < aimList.Count; i++)
        {
            Debug.Log("오브젝트 풀링을 사용하지 않는 destroy");
            Debug.Log(aimList[i].gameObject);
            Object.Destroy(aimList[i].gameObject);
        }
        aimList.Clear();
    }
}
