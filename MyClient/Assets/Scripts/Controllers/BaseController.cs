using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public int Id { get; set; }

    PositionInfo _positionInfo = new PositionInfo();
    AudioManager audioManager;

    public PositionInfo PosInfo
    {
        get { return _positionInfo; }
        set
        {
            if (_positionInfo.Equals(value))
                return;
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateController();
    }

    protected virtual void Init()
    {

    }

    protected virtual void UpdateController()
    {

    }

    public virtual void OnDead()
    {
        print("ondead");
        GameObject effect = Managers.Resource.Instantiate("Effect/DieEffect");
        effect.transform.position = transform.position;
        effect.GetComponent<Animator>().Play("START");
        GameObject.Destroy(effect, 0.5f);
    }
    public virtual void Sound_Dead()
    {
        //audioManager = AudioManager.instance;
        //audioManager.PlayGlobalSound(audioManager.soundEffects[1]);
    }

    public virtual void Sound_Shoot()
    {
        //audioManager = AudioManager.instance;
        //audioManager.PlayGlobalSound(audioManager.soundEffects[0]);
    }
}
