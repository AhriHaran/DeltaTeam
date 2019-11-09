using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스파인을 셋팅하고 그에따른 행동을하는것 즉, 스파인 애니메이션을 조절하는 코드
/// </summary>
/// 
public class SpineManager : MonoBehaviour
{
    public string m_strRoute;           //스파인 루트
    public Vector3 m_BattlePosition;    //배틀포지션
    public GameObject m_BattleUI;       //배틀 UI
    private string m_PointName;         //포인트 이름
    private int m_iTargetTMP;           //타겟 인덱스

    private GameObject m_TargetUI;
    private GameObject m_SelectImpact;  //셀렉트 임팩트
    private GameObject m_TargetImapct;  //타겟 임팩트
    private Vector3    m_CurPosition;   //이전 위치

    public void Awake()
    {
        m_PointName = transform.name;
        m_iTargetTMP = -1;

        GameObject Effect = GameObject.Find("EffectObject");

        m_SelectImpact = Effect.transform.GetChild(0).gameObject;
        m_TargetImapct = Effect.transform.GetChild(1).gameObject;
        m_TargetUI = GameObject.Find("UI Root").transform.GetChild(3).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        List<CharPartyData> PartyList = null;
        if (m_PointName == "PlayerPoint")   //v플레이어 포인트
        {
            PartyList = GameManager.instance.PlayerParty;
        }
        else if (m_PointName == "EnemyPoint")    //에너미 포인트
        {
            PartyList = GameManager.instance.EnemyParty;
        }
        
        foreach (CharPartyData Member in PartyList)
        {
            string Name = Util.ConvertToString(Member.CharData.ReturnData(CHARACTER_DATA.CHAR_NAME));
            string Route = m_strRoute + Name;
            GameObject Spine = ResourceLoader.CreatePrefab(Route);
            Spine.transform.SetParent(transform, false);
        }

        PositionSetting();
    }

    public void PositionSetting()
    {
        //파티 스파인 셋팅(캐릭터와 에너미,)
        float fX = 0.0f, fY = -67.0f, fZ = 0.0f;
        int iTmp = 1;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject Game = transform.GetChild(i).gameObject;
            if (Game.activeSelf)
            {
                if (m_PointName == "EnemyPoint")
                {
                    iTmp = 1;
                    Game.GetComponent<EnemySelect>().EnemyIndex = i;
                }
                else
                {
                    iTmp = -1;
                }

                if (i == 0)
                    fX = (iTmp * 15.0f);
                else if (i == 1)
                    fX = (iTmp * 30.0f);
                else if (i == 2)
                    fX = (iTmp * 40.0f);
                else if (i == 3)
                    fX = (iTmp * 60.0f);

                if (i == 0 || i == 2)
                    fZ = -5.0f;
                else if (i == 1 || i == 3)
                    fZ = 5.0f;

                Game.transform.position = new Vector3(fX, fY, fZ);
                //10, 20, 40, 50
            }
            //파티 리스트에 따라서 플레이어 및 에너미 캐릭터 배치
        }
    }

    private void Update()
    {
        //타겟 변경 키
        InputKey();
    }

    private void InputKey()
    {
        if(GameManager.instance.WhatPhase == GAME_PHASE.PHASE_TARGET_SELECT &&
            GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_PLAYER 
            && m_PointName == "EnemyPoint")  //타겟선택 페이즈 및 플레이어 캐릭터 공격 상태
        {
            //마우스와 
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                //1번
                TargetEffect(0);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                //2번
                TargetEffect(1);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                //3번
                TargetEffect(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                //4번
                TargetEffect(3);
            }
        }
    }

    public void TargetSelect()
    {
        //현재 타겟인 녀석과
        bool bTmp = false;
        int iAttackIndex = GameManager.instance.WhoAttackerIndex;
        if (GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_PLAYER && 
            m_PointName == "PlayerPoint")
        {
            bTmp = true;
        }
        else if(GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_ENEMY &&
            m_PointName == "EnemyPoint")
        {
            bTmp = true;
        }

        if(bTmp)
        {
            Vector3 Vec = Vector3.zero;
            //셀렉트 임팩트
            float fX = transform.GetChild(iAttackIndex).transform.position.x;
            float fY = m_SelectImpact.transform.position.y;
            float fZ = transform.GetChild(iAttackIndex).transform.position.z;
            Vec.Set(fX, fY, fZ);
            m_SelectImpact.transform.position = Vec;
            m_SelectImpact.SetActive(true);
        }

        //타겟 임팩트 셋팅
        int iTarget = 0;
        bTmp = false;
        if (GameManager.instance.WhoTargetType == CHARACTER_TYPE.CHAR_PLAYER && m_PointName == "PlayerPoint")
        {
            bTmp = true;
            iTarget = Random.Range(0, GameManager.instance.PlayerParty.Count);
            //적의 경우 랜덤으로 선택하고 에너미 AI
        }
        else if (GameManager.instance.WhoTargetType == CHARACTER_TYPE.CHAR_ENEMY && m_PointName == "EnemyPoint")
        {
            bTmp = true;
        }

        if(bTmp)
        {
            TargetEffect(iTarget);
        }

        //스파인에서 타겟 셀렉트 UI 사용 가능하게 한다.
    }

    public void EnemyAI()
    {
        Debug.Log("EnemyAI Start");
        if (GameManager.instance.WhatPhase == GAME_PHASE.PHASE_TARGET_SELECT &&
            GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_ENEMY &&
            m_PointName == "EnemyPoint")
        {
            //현재 스킬을 선택할 수 있는 캐릭터만 선택 가능
            GameManager.instance.HowAttacker = CHARACTER_SKILL_TYPE.CHAR_ATTACK; //무슨 버튼을 눌렀나
            //스킬 선택 페이즈만 선택가능
            GameManager.instance.WhatPhase = GAME_PHASE.PHASE_ATTACK;    //어택으로 전환
            //에너미 타겟 선택 페이즈
            //스킬 아이콘 클릭
        }
    }

    public void TargetEffect(int iTarget)
    {
        if(transform.childCount > iTarget)
        {
            if(m_TargetImapct.transform.GetChild(iTarget).gameObject.activeSelf)
            {
                Vector3 Vec = Vector3.zero;
                float fX = transform.GetChild(iTarget).transform.position.x;
                float fY = m_TargetImapct.transform.position.y;
                float fZ = transform.GetChild(iTarget).transform.position.z;
                Vec.Set(fX, fY, fZ);
                m_TargetImapct.transform.position = Vec;
                m_TargetImapct.SetActive(true);
                GameManager.instance.WhoTargetIndex = iTarget;//


                m_TargetUI.SetActive(true);

                Vec = Vector3.zero;
                fX *= 10.0f;
                Vec.Set(fX, 200.0f, 0.0f);
                m_TargetUI.transform.localPosition = Vec;

                m_TargetUI.GetComponent<GageUI>().Setting(GameManager.instance.WhoTargetIndex, GameManager.instance.WhoTargetType);
                
                Debug.Log(iTarget + "Enemy");
            }
        }
    }
    //죽으면 리스트 삭제 해버려
    
    public void HPRefresh()
    {
        //HP 리프레쉬
        m_TargetUI.GetComponent<GageUI>().SliderSetting();
    }

    public void BattleAnimation()
    {
        //누가 현재 공격할수있고 누가 현재 때릴 수 있는가. 애니메이션만 콜 하는 함수
        //공격 하는 사람이 플레이어다.
        if (m_PointName == "PlayerPoint")   //플레이어 포인트
        {
            //플레이어 
            if (GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_PLAYER)
            {
                m_iTargetTMP = GameManager.instance.WhoAttackerIndex;
                if (GameManager.instance.HowAttacker == CHARACTER_SKILL_TYPE.CHAR_ATTACK)
                    transform.GetChild(m_iTargetTMP).GetComponent<Animator>().SetTrigger("Attack");
                else if (GameManager.instance.HowAttacker == CHARACTER_SKILL_TYPE.CHAR_SKILL)
                    transform.GetChild(m_iTargetTMP).GetComponent<Animator>().SetTrigger("Skill");
                GameManager.instance.AttackAnimeEnd = false;
            }
            else
            {
                m_iTargetTMP = GameManager.instance.WhoTargetIndex;
                transform.GetChild(m_iTargetTMP).GetComponent<Animator>().SetTrigger("Hit");
                GameManager.instance.TargetAnimeEnd = false;
            }
        }
        else if (m_PointName == "EnemyPoint")
        {
            if (GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_PLAYER)
            {
                m_iTargetTMP = GameManager.instance.WhoTargetIndex;
                transform.GetChild(m_iTargetTMP).GetComponent<Animator>().SetTrigger("Hit");
                GameManager.instance.AttackAnimeEnd = false;
            }
            else
            {
                m_iTargetTMP = GameManager.instance.WhoAttackerIndex;
                if (GameManager.instance.HowAttacker == CHARACTER_SKILL_TYPE.CHAR_ATTACK)
                    transform.GetChild(m_iTargetTMP).GetComponent<Animator>().SetTrigger("Attack");
                else if (GameManager.instance.HowAttacker == CHARACTER_SKILL_TYPE.CHAR_SKILL)
                    transform.GetChild(m_iTargetTMP).GetComponent<Animator>().SetTrigger("Skill");
                GameManager.instance.TargetAnimeEnd = false;
            }
        }

        m_CurPosition = transform.GetChild(m_iTargetTMP).position;
        transform.GetChild(m_iTargetTMP).position = m_BattlePosition;
        //해당 타겟을 배틀 포인트까지 이동
        m_SelectImpact.SetActive(false);
        m_TargetImapct.SetActive(false);
        //에펙트 둘 다 꺼주고 공격 애니메이션이 끝나면 다시 원상복구 및 데미지 계산
        //애니메이션이 꺼졌다는 것을 확인해야 한다.
    }

    public void AnimationEnd()
    {
        if (m_PointName == "PlayerPoint")   //플레이어 포인트
        {
            //플레이어 가 선공
            if (GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_PLAYER)
            {
                GameManager.instance.AttackAnimeEnd = true;
            }
            else if (GameManager.instance.WhoTargetType == CHARACTER_TYPE.CHAR_PLAYER)
            {
                GameManager.instance.TargetAnimeEnd = true;
            }
        }
        else if (m_PointName == "EnemyPoint")
        {
            if (GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_ENEMY)
            {
                GameManager.instance.AttackAnimeEnd = true;
            }
            else if (GameManager.instance.WhoTargetType == CHARACTER_TYPE.CHAR_ENEMY)
            {
                GameManager.instance.TargetAnimeEnd = true;
            }
        }
    }

    public void DeathAnimationEnd()
    {
        //데스 애니메이션 호출이 끝나면 transform의 가장 뒤로 빼주고,꺼주고 포지션 재배열
        int iIndex = GameManager.instance.WhoDeathIndex;
        transform.GetChild(iIndex).gameObject.SetActive(false);
        transform.GetChild(iIndex).SetAsLastSibling();
        PositionSetting();

        GameManager.instance.WhatPhase = GAME_PHASE.PHASE_TURN_START;
    }

    public void DeathAnimationStart()
    {
        int iIndex = GameManager.instance.WhoDeathIndex;
        transform.GetChild(iIndex).GetComponent<Animator>().SetTrigger("Death");
    }

    //여기서 적 턴일때 자동으로 룰렛

    public void BattleAnimationEnd()
    {
        transform.GetChild(m_iTargetTMP).transform.localPosition = m_CurPosition;
        //포지션 원위치
        m_CurPosition = Vector3.zero;
    }
}
