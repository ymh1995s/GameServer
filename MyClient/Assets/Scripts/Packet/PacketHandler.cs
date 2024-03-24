﻿using Google.Protobuf.Protocol;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MyServerCore;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Collections;

class PacketHandler :MonoBehaviour
{
    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterGamePacket = packet as S_EnterGame;
        Managers.Object.Add(enterGamePacket.Player, myPlayer: true);
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leaveGameHandler = packet as S_LeaveGame;
        print("S_LeaveGameHandler Received");
        Managers.Object.Clear();
    }
    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;
        foreach (ObjectInfo obj in spawnPacket.Objects)
        {
            Managers.Object.Add(obj, myPlayer: false);
        }
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawnPacket = packet as S_Despawn;
        print("S_DespawnHandler Received");
        foreach (int id in despawnPacket.ObjectIds)
        {
            Managers.Object.Remove(id);
        }
    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move movePacket = packet as S_Move;

        GameObject go = Managers.Object.FindById(movePacket.ObjectId);
        if (go == null)
            return;

        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return;

        MyPlayerController MPC = bc as MyPlayerController;
        if (MPC == null)
        { 
            //TO DO
        }
        else
        {
            MPC.canIMove = true;
        }

        bc.transform.position = new Vector3(movePacket.PosInfo.PosX, movePacket.PosInfo.PosY, 0);
    }

    public static void S_DieHandler(PacketSession session, IMessage packet)
    {
        S_Die diePacket = packet as S_Die;

        //텍스트 이펙트
        string attacker =diePacket.AttackerId.ToString();
        string death = diePacket.ObjectId.ToString();   
        string styledText = "<color=blue>" + attacker + "</color> <color=black>kill</color> <color=red>"+ death + "</color>";
        GameManager.instance.text.text = styledText;

        GameObject go = Managers.Object.FindById(diePacket.ObjectId);
        if (go == null)
            return;

        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return;


        bc.OnDead();

        MyPlayerController mpc = bc.GetComponent<MyPlayerController>();
        if (mpc != null)
        {
            print("사망처리(나) " + bc.Id);
            bc.Sound_Dead();
        }
    }

    public static void S_ArrowHandler(PacketSession session, IMessage packet)
    {
        S_Arrow arrowPacket = packet as S_Arrow;

        // Owner의 정보 뽑기
        GameObject go = Managers.Object.FindById(arrowPacket.Owner);
        if (go == null)
            return;

        GameObject arrow = Managers.Resource.Instantiate("Creature/Bullet");
        BaseController bc = arrow.GetComponent<BaseController>();
        bc.Id = arrowPacket.Owner; //화살 소유주 결정

        MyPlayerController mpc = go.GetComponent<MyPlayerController>();
        if (mpc == null)
        {
            //TO DO
        }
        else
        {
            mpc.canIShoot = true;
            bc.Sound_Shoot();
        }

        arrow.transform.position = new Vector2(go.transform.position.x, go.transform.position.y);

        Vector2 shootDir = new Vector2( arrowPacket.XVec, arrowPacket.YVec);
        float targetRotation = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        targetRotation += 270;
        arrow.transform.rotation = Quaternion.Euler(0f,0f, targetRotation);

        arrow.gameObject.GetComponent<Rigidbody2D>().AddForce(shootDir.normalized * 10, ForceMode2D.Impulse);

        print("Arrow Received Owner " +  bc.Id);
    }
}