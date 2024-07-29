using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.UI.Image;

public class MyPlayerController : PlayerController
{
    //�Һ��� ���� TODO
    public bool canIMove { get; set; } = true;
    public bool canIShoot { get; set; } = true;
    bool isObstacle { get; set; } = false;

    protected Coroutine ShootCorountine;
    bool shootDelay { get; set; } = true;

    Vector2 shootDir;
    Camera cam;

    protected Coroutine moveDelay;

    float dis = 0.1f; // ������ �ִ� �Ÿ�
    int addspeed = 2;
    const float moveVecSt = 0.1f * 2;
    const float moveVecDi = 0.07f * 2 ;

    protected override void Init()
    {
        base.Init();
        cam = Camera.main;
    }

    protected override void UpdateController()
    {
        if (canIMove && !isObstacle)
            GetDirInput();

        if (canIShoot && shootDelay)
            Shoot();

        base.UpdateController();
    }

    void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        BaseController bc = go.GetComponent<BaseController>();

        //���� �� ȭ���� ���ø�
        if (collision.gameObject.tag == "Bullet" && bc.Id != Id)
        {
            print("�����ſ� ����");
            Destroy(go);
            OnHit(bc.Id, Id);
        }
    }

    IEnumerator MoveDelay(float valueX, float valueY, bool isLeft)
    {
        canIMove = false;
        yield return new WaitForSeconds(0.01f);
        C_Move movePacket = new C_Move();

        PosInfo.PosX = transform.position.x + valueX;
        PosInfo.PosY = transform.position.y + valueY;

        movePacket.PosInfo = PosInfo;
        movePacket.IsLeft = isLeft;
        Managers.Network.Send(movePacket);
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shootDir = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position);

            //��Ŷ ����
            C_Arrow arrowPacket = new C_Arrow();
            arrowPacket.Owner = Id;
            arrowPacket.XVec = shootDir.x;
            arrowPacket.YVec = shootDir.y;
            Managers.Network.Send(arrowPacket);

            canIShoot = false;
            shootDelay = false;
            ShootCorountine = StartCoroutine("ShootDelay");
            //ShootCorountine = StartCoroutine("ShootEffect");
        }
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.5f);
        shootDelay = true;
    }

    public void OnHit(int attacker, int target)
    {
        OnDead(attacker, target);
    }

    public void OnDead(int attacker, int target)
    {
        C_Die diePacket = new C_Die();

        diePacket.ObjectId = target;
        diePacket.AttackerId = attacker;

        Managers.Network.Send(diePacket);
    }

    // Ű���� �Է�
    void GetDirInput()
    {
        if (!canIMove) return;

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && SRay(0.35f, 0.35f) && SRay(0, 0.5f) && SRay(0.5f, 0))
            ReqMove(moveVecDi, moveVecDi);
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && SRay(-0.35f, 0.35f) && SRay(0, 0.5f) && SRay(-0.5f, 0))
            ReqMove(-moveVecDi, moveVecDi);
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && SRay(0.35f, -0.35f) && SRay(0.5f, 0) && SRay(0, -0.5f))
            ReqMove(moveVecDi, -moveVecDi);
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) && SRay(-0.35f, -0.35f) && SRay(-0.5f, 0) && SRay(0, -0.5f))
            ReqMove(-moveVecDi, -moveVecDi);
        else if (Input.GetKey(KeyCode.W) && SRay(0, 0.5f))
            ReqMove(0, moveVecSt);
        else if (Input.GetKey(KeyCode.S) && SRay(0, -0.5f))
            ReqMove(0, -moveVecSt);
        else if (Input.GetKey(KeyCode.A) && SRay(-0.5f, 0))
            ReqMove(-moveVecSt, 0);
        else if (Input.GetKey(KeyCode.D) && SRay(0.5f, 0))
            ReqMove(moveVecSt, 0);
        else { }
    }

    protected void ReqMove(float valueX, float valueY)
    {
        bool isLeft=false;
        if (valueX == 0) { }
        else if (valueX > 0) isLeft = true;

        ShootCorountine = StartCoroutine(MoveDelay(valueX, valueY, isLeft));
    }

    bool SRay(float x, float y)
    {
        // ����ĳ��Ʈ�� ���� �������� dis��ŭ�� �Ÿ���ŭ ���̸� ���ϴ�.
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(x, y, 0), new Vector3(x, y, 0), dis);

        if (hit.collider != null && hit.collider.CompareTag("obstacle"))
            return false;
        else
            return true;
    }

    //����� ����׿� �Լ�
    void OnDrawGizmos()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0, 0), Vector2.left, dis);
        //Gizmos.color = Color.red;
        //// �浹�� ���
        //if (hit.collider != null)
        //{
        //    // �浹 ���������� �Ÿ� ���
        //    float distance = hit.distance;

        //    // �浹 ���������� ���̸� ������ ǥ��
        //    Gizmos.DrawRay(transform.position, Vector2.left * dis);
        //}
        //else
        //{
        //    // �浹���� ���� ���, �ִ� �Ÿ����� ���̸� ������ ǥ��
        //    Gizmos.DrawSphere(hit.point, 0.1f);
        //}
    }
}
