using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : BaseController
{
    // 파괴될 시간
    public float destructionTime = 2.0f;
    private float elapsedTime = 0.0f;

    private void Start()
    {
        // 생성 시작 시간 기록
    }

    void Update()
    {
        // 경과된 시간 누적
        elapsedTime += Time.deltaTime;

        // 누적된 시간이 파괴될 시간을 초과하면 오브젝트 파괴
        if (elapsedTime >= destructionTime)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //내가 쏜 화살은 무시
        if (collision.gameObject.tag == "obstacle")
        {
            Destroy(gameObject);
        }
    }
}
