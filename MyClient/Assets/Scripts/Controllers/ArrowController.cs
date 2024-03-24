using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : BaseController
{
    // �ı��� �ð�
    public float destructionTime = 2.0f;
    private float elapsedTime = 0.0f;

    private void Start()
    {
        // ���� ���� �ð� ���
    }

    void Update()
    {
        // ����� �ð� ����
        elapsedTime += Time.deltaTime;

        // ������ �ð��� �ı��� �ð��� �ʰ��ϸ� ������Ʈ �ı�
        if (elapsedTime >= destructionTime)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //���� �� ȭ���� ����
        if (collision.gameObject.tag == "obstacle")
        {
            Destroy(gameObject);
        }
    }
}
