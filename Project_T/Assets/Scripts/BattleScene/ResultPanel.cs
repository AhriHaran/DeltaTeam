using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    private UISprite m_ResultSprite;
    private UIPanel m_Panel;
    private UIScrollView m_ScrollView;
    private GameObject m_Grid;


    private void Awake()
    {
        m_ResultSprite = transform.GetChild(1).GetComponent<UISprite>();

        GameObject Object = transform.GetChild(2).gameObject;
        m_Panel = Object.GetComponent<UIPanel>();
        m_ScrollView = Object.GetComponent<UIScrollView>();
        m_Grid = Object.transform.GetChild(0).gameObject;

    }

    private void OnEnable()
    {
        //그리드 생성
        if (m_Grid.transform.childCount > 0) //그리드 초기화
        {
            while (m_Grid.transform.childCount != 0)
            {
                GameObject game = m_Grid.transform.GetChild(0).gameObject;
                game.transform.SetParent(null);
                NGUITools.Destroy(game);
            }
            m_Grid.transform.DetachChildren();
        }

        if (GameManager.instance.WhatPhase == GAME_PHASE.PHASE_RESULT_LOSE)
            m_ResultSprite.spriteName = MISSION_STATUS.MISSION_BATTLE_LOSE.ToString();
        else if(GameManager.instance.WhatPhase == GAME_PHASE.PHASE_RESULT_WIN)
        {
            m_ResultSprite.spriteName = MISSION_STATUS.MISSION_COMPLETE.ToString();
            int iMaxEXP = GameManager.instance.EnemyEXP;
            var PlayerParty = GameManager.instance.PlayerParty;
            var UserParty = UserInfo.instance.PartyList;
            int iCount = PlayerParty.Count;

            float fCurEXP = Mathf.Round((float)(iMaxEXP / iCount));
            //파티 숫자대로 엔빵

            var ExcelEXP = EXCEL.ExcelLoad.Read("Excel/Class/CLASS_EXP");   //경험치 테이블(공용)
            string ExcelRoute = string.Empty;
            for (int i = 0; i < iCount; i++)
            {
                //게임 매니저의 캐릭터 데이터를 업데이트 하고,
                //유저의 데이터도 업데이트 한다.
                int iCurEXP = Util.ConvertToInt(UserParty[i].CharData.ReturnData(CHARACTER_DATA.CHAR_CUR_EXP));
                iCurEXP += Util.ConvertToInt(fCurEXP);
                UserParty[i].CharData.Update(CHARACTER_DATA.CHAR_CUR_EXP, fCurEXP); //참조 복사이므로 한쪽만 해주자
                                                                                    //각각 레벨업의 체크 해준다.
                ExcelRoute = "Excel/Class/" + UserParty[i].CharData.ReturnData(CHARACTER_DATA.CHAR_CLASS_TYPE).ToString();
                var ExcelClass = EXCEL.ExcelLoad.Read(ExcelRoute);  //클래스 EXCEL

                ExcelRoute = "Excel/Character/" + UserParty[i].CharData.ReturnData(CHARACTER_DATA.CHAR_CLASS_TYPE).ToString() +
                    Util.ConvertToString(UserParty[i].CharData.ReturnData(CHARACTER_DATA.CHAR_INDEX));
                var ExcelChar = EXCEL.ExcelLoad.Read(ExcelRoute);   //캐릭터 EXCEL

                if (UserParty[i].CharData.ifLevelUp(ExcelClass, ExcelEXP, ExcelChar))
                {
                    //레벨업 하였는가 그렇다면 HP를 맥스로 채워줘라, 각각의 데이터를 리셋팅
                    UserParty[i].MaxHP = float.Parse(Util.ConvertToString(UserParty[i].CharData.ReturnData(CHARACTER_DATA.CHAR_MAX_HP)));
                    UserParty[i].CurHP = float.Parse(Util.ConvertToString(UserParty[i].CharData.ReturnData(CHARACTER_DATA.CHAR_MAX_HP)));
                    PlayerParty[i].MaxHP = float.Parse(Util.ConvertToString(PlayerParty[i].CharData.ReturnData(CHARACTER_DATA.CHAR_MAX_HP)));
                    PlayerParty[i].CurHP = float.Parse(Util.ConvertToString(PlayerParty[i].CharData.ReturnData(CHARACTER_DATA.CHAR_MAX_HP)));
                    Debug.Log("LevelUP");
                }

                GameObject CharInfo = ResourceLoader.CreatePrefab("Prefabs/UI/PlayerCard");
                CharInfo.transform.SetParent(m_Grid.transform, false);
                CharInfo.GetComponent<PlayerCard>().Setting(i);
            }
            GameManager.instance.EnemyEXP = 0;
            m_Grid.GetComponent<UIGrid>().Reposition(); //리 포지셔닝으로 그리드 재정렬
            m_ScrollView.ResetPosition();
            m_Panel.Refresh();
        }
    }

    public void OnClick()
    {
        //클릭, 이전 씬으로 전환
        //승리 시 경험치 누적, 패배 시 경험치 누적x 및
        UserInfo.instance.UserCompanionSave();
        UserInfo.instance.UserPartySave();
        LoadScene.SceneLoad("GameScene");
    }
}
