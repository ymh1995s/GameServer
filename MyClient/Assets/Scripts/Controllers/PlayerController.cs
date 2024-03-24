using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : BaseController
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        base.UpdateController();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        BaseController bc = go.GetComponent<BaseController>();

        //내가 쏜 화살은 무시
        if (collision.gameObject.tag == "Bullet" && bc.Id != Id)
        {
            Destroy(go);
        }
    }
}
