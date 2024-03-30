using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : BaseController
{
    protected Transform flashTransform;
    protected Coroutine shootEffect;

    protected override void Init()
    {
        base.Init();

        flashTransform = transform.Find("flash");
        flashTransform.gameObject.SetActive(false);
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

    public void PlayShootEffect()
    {
        shootEffect = StartCoroutine("ShootEffect");
    }

    IEnumerator ShootEffect()
    {
        flashTransform.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flashTransform.gameObject.SetActive(false);
    }
}
