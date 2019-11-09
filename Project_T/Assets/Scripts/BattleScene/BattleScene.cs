using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    /* 여기서 배틀 씬에서 필요한 캐릭터와 적 캐릭터를 배치
     * 
     * 배치 후 배틀 애니메이션에 관련된 것은 이벤트 매니저로 처리
     */
    public GameObject m_PlayerPoint;   //플레이어 스파인 위치
    public GameObject m_EnemyPoint;    //에너미 스파인 위치

    private CallBackUI m_AttackUICall = null;
    
    private CHARACTER_TYPE m_eFirstAttacker;
    private GameObject m_DemageLabel;   //데미지 라벨
    private GameObject m_AttakcUI;   //플레이어 UI
    private UILabel m_TurnText;
    private GameObject m_ResultPanel;   //결과 창

    private void Awake()
    {
        //파티 스파인 셋팅 후
        m_eFirstAttacker = GameManager.instance.FirstAttacker;//첫공격자

        GameObject game = GameObject.Find("UI Root");
        m_DemageLabel = game.transform.GetChild(1).gameObject;    //데미지라벨
        m_AttakcUI = game.transform.GetChild(2).gameObject; //캐릭터 UI
        m_AttackUICall = m_AttakcUI.GetComponent<AttackUI>().UISetting; //콜백 셋팅
        m_TurnText = game.transform.GetChild(4).GetComponent<UILabel>();
        m_ResultPanel = game.transform.GetChild(5).gameObject;
    }

    IEnumerator PhaseAction()
    {
        //각각의 페이즈 별 액션
        while (true)
        {
            switch (GameManager.instance.WhatPhase)
            {
                case GAME_PHASE.PHASE_TURN_START:
                    TurnStart();
                    break;
                case GAME_PHASE.PHASE_TARGET_SELECT:
                    if(GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_ENEMY)
                    {
                        m_EnemyPoint.GetComponent<SpineManager>().EnemyAI();
                    }
                    break;
                case GAME_PHASE.PHASE_ATTACK:
                    AttackCheck();  //어택 체크
                    break;
                case GAME_PHASE.PHASE_TURN_END:
                    TurnEnd();
                    break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Start()
    {
        StartCoroutine("AnimationCheck"); //애니메이션 체크
        StartCoroutine("ResultCheck");
        
        if(m_eFirstAttacker == CHARACTER_TYPE.CHAR_PLAYER)  //첫공이면 보정치를 준다.
        {
            var PlayerParty = GameManager.instance.PlayerParty;

            foreach (CharPartyData Party in PlayerParty)
            {
                Party.CurStemina = Party.MaxStemina;
            }
        }
        else if(m_eFirstAttacker != CHARACTER_TYPE.CHAR_NONE)
        {
            var EnemyParty = GameManager.instance.EnemyParty;
            foreach (CharPartyData Party in EnemyParty)
            {
                Party.CurStemina = Party.MaxStemina;
            }
        }
        //첫 공격이면 턴 게이지 보정치
        GameManager.instance.WhatPhase = GAME_PHASE.PHASE_TURN_START;
        StartCoroutine("PhaseAction");
    }

    private void TurnStart()
    {
        //여기가 첫번째 수행
        //공격할 수 있는 것이 있는 지 확인하는 페이즈
        List<CharPartyData> PartyList = null;

        if (GameManager.instance.GlobalTurn == 1 || GameManager.instance.GlobalTurn % 2 != 0)  //홀수턴(선공)
        {//홀수턴
            if (m_eFirstAttacker == CHARACTER_TYPE.CHAR_PLAYER)  //
            {
                PartyList = GameManager.instance.PlayerParty;
                GameManager.instance.WhoAttakerType = CHARACTER_TYPE.CHAR_PLAYER;
                GameManager.instance.WhoTargetType = CHARACTER_TYPE.CHAR_ENEMY;
            }
            else if (m_eFirstAttacker != CHARACTER_TYPE.CHAR_NONE)
            {
                PartyList = GameManager.instance.EnemyParty;
                GameManager.instance.WhoAttakerType = CHARACTER_TYPE.CHAR_ENEMY;
                GameManager.instance.WhoTargetType = CHARACTER_TYPE.CHAR_PLAYER;
            }
        }
        else if (GameManager.instance.GlobalTurn % 2 == 0)   //짝수턴(후공)
        {
            if (m_eFirstAttacker == CHARACTER_TYPE.CHAR_PLAYER)  //
            {
                PartyList = GameManager.instance.EnemyParty;
                GameManager.instance.WhoAttakerType = CHARACTER_TYPE.CHAR_ENEMY;
                GameManager.instance.WhoTargetType = CHARACTER_TYPE.CHAR_PLAYER;
            }
            else if (m_eFirstAttacker != CHARACTER_TYPE.CHAR_NONE)  //플레이어 외의 모든 종류
            {
                PartyList = GameManager.instance.PlayerParty;
                GameManager.instance.WhoAttakerType = CHARACTER_TYPE.CHAR_PLAYER;
                GameManager.instance.WhoTargetType = CHARACTER_TYPE.CHAR_ENEMY;
            }
        }
        //턴 시작시 현재 턴 스피드에 따라서 공격 가능한 캐릭터가 있는가를 확

        if (CanAttack(PartyList))    //어택 하는 것이 가능한가?
        {
            GameManager.instance.WhatPhase = GAME_PHASE.PHASE_TARGET_SELECT; //타겟 셀렉트 페이즈
            m_AttackUICall?.Invoke(GameManager.instance.WhoAttakerType, GameManager.instance.WhoAttackerIndex);
            //공격가능 한 아이로 UI를 셋팅
            m_PlayerPoint.GetComponent<SpineManager>().TargetSelect();
            m_EnemyPoint.GetComponent<SpineManager>().TargetSelect();
            if (GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_PLAYER)
                m_TurnText.text = "Player Turn";
            else
                m_TurnText.text = "Enemy Turn";
        }
        else
        {
            GameManager.instance.WhatPhase = GAME_PHASE.PHASE_TURN_END; //턴엔드 페이즈
        }
    }

    private bool CanAttack(List<CharPartyData> PartyList)
    {
        for (int i = 0; i < PartyList.Count; i++)
        {
            if (PartyList[i].MaxStemina <= PartyList[i].CurStemina && PartyList[i].Stay == false)  //현재 공격 수행 가능
            {
                //그렇다면 스피드를 빼주고 현재 공격 가능한 아이로 셋팅
                GameManager.instance.WhoAttackerIndex = i;   //현재 해당 파티의 캐릭터가 공격가능
                Debug.Log(i + "Attack OK");
                return true;
            }
        }
        return false;
    }

    private void AttackCheck()
    {
        //어택 체크, 플레이어 스테이터스에 따라서 데미지 계산
        CHARACTER_TYPE eAttakerType = GameManager.instance.WhoAttakerType;  //어느 타입이
        int iAttackerIndex = GameManager.instance.WhoAttackerIndex; //어느 인덱스가
        //공격자

        CHARACTER_TYPE eTargetType = GameManager.instance.WhoTargetType;
        int iTargetIndex = GameManager.instance.WhoTargetIndex;
        //타겟

        List<CharPartyData> AttakcerParty = null;
        List<CharPartyData> TargetParty = null;

        if(eAttakerType == CHARACTER_TYPE.CHAR_PLAYER)  //공격하는것이 플레이어 파티라면?
        {
            AttakcerParty = GameManager.instance.PlayerParty;
            TargetParty = GameManager.instance.EnemyParty;
        }
        else if(eAttakerType != CHARACTER_TYPE.CHAR_NONE)
        {
            AttakcerParty = GameManager.instance.EnemyParty;
            TargetParty = GameManager.instance.PlayerParty;
        }

        float fAtk = float.Parse(Util.ConvertToString(AttakcerParty[iAttackerIndex].CharData.ReturnData(CHARACTER_DATA.CHAR_ATK)));
        float fHP = TargetParty[iTargetIndex].CurHP; //현재 HP

        if (GameManager.instance.HowAttacker == CHARACTER_SKILL_TYPE.CHAR_STAY)
        {
            //스테이라면 해당 캐릭터의 스테이를 만족
            AttakcerParty[iAttackerIndex].Stay = true;
            //스테이
            GameManager.instance.WhatPhase = GAME_PHASE.PHASE_TURN_START;
        }
        else
        {
            if (GameManager.instance.HowAttacker == CHARACTER_SKILL_TYPE.CHAR_SKILL)
            {
                float fSkill = float.Parse(Util.ConvertToString(AttakcerParty[iAttackerIndex].CharData.ReturnData(CHARACTER_DATA.CHAR_SKILL_DEMAGE)));
                fAtk *= fSkill;
                Mathf.Round(fAtk);
                //소수점 반올림
            }

            fHP -= fAtk;
            if (fHP < 0)
                fHP = 0.0f;
            TargetParty[iTargetIndex].CurHP = fHP;

            //어택 체크 후 HP Refresh
            m_PlayerPoint.GetComponent<SpineManager>().HPRefresh();
            m_EnemyPoint.GetComponent<SpineManager>().HPRefresh();
            m_AttakcUI.GetComponent<AttackUI>().HPRefresh();    //
                                                                //타겟 UI도 리셋팅, SpineManager에서 셋
                                                                //UI호출
            int iAtk = Util.ConvertToInt(fAtk);
            m_DemageLabel.GetComponent<DemageLabel>().Setting(Util.ConvertToString(iAtk));//데미지 표기
                                                                                          //데미지 라벨 호출
            m_DemageLabel.SetActive(true);

            m_PlayerPoint.GetComponent<SpineManager>().BattleAnimation();
            m_EnemyPoint.GetComponent<SpineManager>().BattleAnimation();
            //어택 애니메이션 호출
            GameManager.instance.WhatPhase = GAME_PHASE.PHASE_ATTACK_END;
        }
    }

    /// <summary>
    /// 배틀 씬 플레이어 패배
    /// </summary>
    /// <returns></returns>
    IEnumerator ResultCheck()
    {
        while(true)
        {
            int iPlayerMax = GameManager.instance.PlayerParty.Count;
            int iEnemyMax = GameManager.instance.EnemyParty.Count;

            if(iPlayerMax <=  0)
            {
                //플레이어 패배
                if (GameManager.instance.WhoBattleType == CHARACTER_TYPE.CHAR_ENEMY)
                {
                    GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_BATTLE_LOSE;
                }
                else if (GameManager.instance.WhoBattleType == CHARACTER_TYPE.CHAR_DANGEROUS)
                {
                    GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_BATTLE_LOSE;
                }
                else if (GameManager.instance.WhoBattleType == CHARACTER_TYPE.CHAR_BOSS)
                {
                    //미션 실패
                    GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_FAILED;
                }

                //파티 전멸시 새롭게 리셋해준다.
                GameManager.instance.WhatPhase = GAME_PHASE.PHASE_RESULT_LOSE;
                UserInfo.instance.UserPartyCopy();  //유저파티 카피
                m_ResultPanel.SetActive(true);
                StopCoroutine("PhaseAction");
                break;
            }
            else if(iEnemyMax <= 0)
            {
                //플레이어 승리 시에는 현재 남아있는 턴수와 HP를 그대로 저장한다.
                
                if (GameManager.instance.WhoBattleType == CHARACTER_TYPE.CHAR_ENEMY)
                {
                    GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_BATTLE_WIN;
                }
                else if (GameManager.instance.WhoBattleType == CHARACTER_TYPE.CHAR_DANGEROUS)
                {
                    GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_BATTLE_WIN;
                }
                else if (GameManager.instance.WhoBattleType == CHARACTER_TYPE.CHAR_BOSS)
                {
                    //보스 킬
                    GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_BOSS_WIN;
                }

                int iBlock = GameManager.instance.BlockIndex;
                GameManager.instance.BlockList[iBlock].m_bActive = false;   //비활성화
                GameManager.instance.WhatPhase = GAME_PHASE.PHASE_RESULT_WIN;
                m_ResultPanel.SetActive(true);
                StopCoroutine("PhaseAction");
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    IEnumerator AnimationCheck()
    {
        while(true)
        {
            if (GameManager.instance.WhatPhase == GAME_PHASE.PHASE_ATTACK_END)
            {
                if (GameManager.instance.TargetAnimeEnd && GameManager.instance.AttackAnimeEnd)
                {
                    //애니메이션 둘다 끝남
                    //그러면 애니메이션 제자리로 돌려주고 데미지 계산 및
                    //애니메이션 끝났다는 콜과 함께 데미지 계산 및 다음 턴 계산
                    m_PlayerPoint.GetComponent<SpineManager>().BattleAnimationEnd();
                    m_EnemyPoint.GetComponent<SpineManager>().BattleAnimationEnd(); //배틀 애니메이션엔드
                    
                    CHARACTER_TYPE eTargetType = GameManager.instance.WhoTargetType;
                    int iTargetIndex = GameManager.instance.WhoTargetIndex;
                    int iAttackIndex = GameManager.instance.WhoAttackerIndex;
                    //타겟
                    List<CharPartyData> TargetParty = null;
                    List<CharPartyData> AttackParty = null;

                    if (eTargetType == CHARACTER_TYPE.CHAR_PLAYER)  //공격 받는 것이 플레이어 파티라면?
                    {
                        AttackParty = GameManager.instance.EnemyParty;
                        TargetParty = GameManager.instance.PlayerParty;
                    }
                    else if(eTargetType != CHARACTER_TYPE.CHAR_NONE)
                    {
                        AttackParty = GameManager.instance.PlayerParty;
                        TargetParty = GameManager.instance.EnemyParty;
                    }

                    if (GameManager.instance.HowAttacker == CHARACTER_SKILL_TYPE.CHAR_ATTACK)
                        AttackParty[iAttackIndex].CurStemina -= 1;
                    else if( GameManager.instance.HowAttacker == CHARACTER_SKILL_TYPE.CHAR_SKILL)
                        AttackParty[iAttackIndex].CurStemina -= 3;
                    //공격한 애의 스피드를 뺸다.

                    float fHp = TargetParty[iTargetIndex].CurHP;
                    bool bDie = false;

                    if (fHp <= 0.0f)    //사망에 대한 처리
                    {
                        //target 캐릭터가 사망 상태이다 그렇다면 사망 모션을 호출 하고 리스트를 삭제
                        //누가 죽었는가?
                        GameManager.instance.WhoDeathType = eTargetType;
                        GameManager.instance.WhoDeathIndex = iTargetIndex;

                        if (eTargetType == CHARACTER_TYPE.CHAR_ENEMY || eTargetType == CHARACTER_TYPE.CHAR_DANGEROUS || eTargetType == CHARACTER_TYPE.CHAR_BOSS)
                        {
                            int EXP = Util.ConvertToInt(Util.ConvertToString(TargetParty[iTargetIndex].CharData.ReturnData(CHARACTER_DATA.CHAR_MAX_EXP)));
                            //타겟이 적국일때만 해당 캐릭터의 경험치를 저장
                            GameManager.instance.EnemyEXP += EXP;   //경험치 저장
                            //나중에 결과창에서 결산
                        }

                        TargetParty.RemoveAt(iTargetIndex);
                        
                        if (eTargetType == CHARACTER_TYPE.CHAR_ENEMY)
                        {
                            m_EnemyPoint.GetComponent<SpineManager>().DeathAnimationStart();
                        }
                        else if (eTargetType == CHARACTER_TYPE.CHAR_PLAYER)
                        {
                            m_PlayerPoint.GetComponent<SpineManager>().DeathAnimationStart();
                        }
                        //데스 애니메이션 콜   
                        bDie = true;
                    }

                    //죽었을때?
                    m_PlayerPoint.GetComponent<SpineManager>().HPRefresh();
                    m_EnemyPoint.GetComponent<SpineManager>().HPRefresh();
                    m_AttakcUI.GetComponent<AttackUI>().HPRefresh();

                    //죽음에 대한 것을 처리 하고 더 공격할 것이 있는가를체크
                    GameManager.instance.TargetAnimeEnd = false;
                    GameManager.instance.AttackAnimeEnd = false;

                    if (!bDie)
                    {
                        GameManager.instance.WhatPhase = GAME_PHASE.PHASE_TURN_START;
                        //죽지 않았다.
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private void TurnEnd()
    {
        //더 공격 가능한지 체크 후 턴 상승
        //캐릭터 HP를 계산하여 죽은 캐릭터를 처리
        GameManager.instance.GlobalTurn += 1;//글로벌 턴을 상승시켜주고
        //다시 턴 스타트
        //파티 리스트에 각각의 턴 게이지 만큼 +

        var PlayerParty = GameManager.instance.PlayerParty;
        foreach (CharPartyData Party in PlayerParty)
        {
            Party.CurFillStemina += 1;

            if (Party.CurFillStemina >= Party.FillStemina)  //턴 차는 게이지가 되는가?
            {
                Party.CurStemina += Party.SteminaCurFill;
                Party.CurFillStemina = 1;

                if (Party.CurStemina >= Party.MaxStemina)
                    Party.CurStemina = Party.MaxStemina;

                Party.Stay = false;
            }
        }

        PlayerParty = GameManager.instance.EnemyParty;
        foreach (CharPartyData Party in PlayerParty)
        {
            Party.CurFillStemina += 1;

            if (Party.CurFillStemina >= Party.FillStemina)  //턴 차는 게이지가 되는가?
            {
                Party.CurStemina += Party.SteminaCurFill;
                Party.CurFillStemina = 1;

                if (Party.CurStemina >= Party.MaxStemina)
                    Party.CurStemina = Party.MaxStemina;
                Party.Stay = false;
            }
        }

        //턴엔드 시 Refresh
        m_PlayerPoint.GetComponent<SpineManager>().HPRefresh();
        m_EnemyPoint.GetComponent<SpineManager>().HPRefresh();
        m_AttakcUI.GetComponent<AttackUI>().HPRefresh();    //
        //

        GameManager.instance.ResetBattleData();//데이터 리셋하고
    }
}
