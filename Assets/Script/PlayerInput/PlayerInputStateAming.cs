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

    //�� ����ü�� ����
    private readonly ProjectileObject[] projectileObjects;
    //������ ����� ���� (�ѹ��� �߻�� ��� ����ü�� �߻���)
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
        //�߻�
        Fire();
        Extinction();
    }
    public virtual void LeftUp() { }
    public virtual void RightDown() 
    {
        //ĵ��
        Extinction();
    }
    public virtual void RightUp() { }
    //������ ��� ������ Ÿ���� (���� �������� �ʱ⿡)
    public virtual void Update()
    {
        playerInput?.Update();

        sight.SightOn();
        enemy = sight.Values.Take(count).Select(x => x.enemy).ToList();

        Aim();
    }

    //�Ҹ��ڰ� ���ϴ� Ÿ�ֿ̹� ȣ����� �ʱ⿡ ����
    private void Extinction()
    {
        for (int i = 0; i < aimList.Count; i++)
        {
            Debug.Log("������Ʈ Ǯ���� ������� �ʴ� destroy");
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
            Debug.Log("������Ʈ Ǯ���� ������� �ʴ� ����");
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
        //ĵ��
        for (int i = 0; i < aimList.Count; i++)
        {
            Debug.Log("������Ʈ Ǯ���� ������� �ʴ� destroy");
            Debug.Log(aimList[i].gameObject);
            Object.Destroy(aimList[i].gameObject);
        }
        aimList.Clear();
    }
}
