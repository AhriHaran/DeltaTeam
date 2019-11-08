using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRaycast : MonoBehaviour
{
    [Header("HitDataCheck")]
    public string m_LayerMask = string.Empty;
    public float m_fRange = 10.0f;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_fRange, 1 << LayerMask.NameToLayer(m_LayerMask));

        if (colliders.Length > 0 && m_fRange > 0.0f)
        {
            Debug.Log("Hit");
            try
            {
                EnemyManager Manager = transform.parent.GetChild((int)BLOCK_COMPONENT.COMPONENT_ENEMY_MANAGER).GetComponent<EnemyManager>();
                CHARACTER_TYPE FirstType = CHARACTER_TYPE.CHAR_NONE;
                CHARACTER_TYPE BattleType = Manager.EnemyType;

                int iRnum = UnityEngine.Random.Range(0, 5);

                if (BattleType == CHARACTER_TYPE.CHAR_ENEMY)
                {
                    if (iRnum >= 4 && iRnum <= 5)
                        FirstType = CHARACTER_TYPE.CHAR_ENEMY;
                    else
                        FirstType = CHARACTER_TYPE.CHAR_PLAYER;
                    //1~4 우리 턴 게이지 풀
                    //5~6 상대 턴 게이지 풀
                }
                else if (BattleType == CHARACTER_TYPE.CHAR_BOSS)
                {
                    if (iRnum >= 3 && iRnum <= 5)
                        FirstType = CHARACTER_TYPE.CHAR_ENEMY;
                    else
                        FirstType = CHARACTER_TYPE.CHAR_PLAYER;
                    //1~3 우리 턴 게이지 풀
                    //4~6 상대 턴 게이지 풀
                }
                else if (BattleType == CHARACTER_TYPE.CHAR_DANGEROUS)
                {
                    FirstType = CHARACTER_TYPE.CHAR_ENEMY;
                }


                GameManager.instance.FirstAttacker = FirstType;
                GameManager.instance.WhoBattleType = BattleType; //누구랑 맞붙는가?
                Manager.EnemyPartySet();
                ////공격을 히트 시킨 오브젝트의 이벤트 콜
                ////씬 넘어가기 전에 현재 위치를 저장
                ////배틀 씬으로 넘어간다.
                ///
                //맥스 타임 셋팅
                ////현재 위치로 되돌려 줘야한다.s
                LoadScene.SceneLoad("BattleScene");
                Debug.Log("BattlePhage");
            }
            catch
            {
                Debug.Log("Battle Scene Load Error");
            }
        }
    }
}