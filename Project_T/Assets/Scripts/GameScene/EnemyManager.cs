using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    //해당 에너미가 가지고있는 에너미 파티
    
    private int m_iMapIndex;
    private int m_iBlockIndex;
    private CHARACTER_TYPE m_eType;
    public CHARACTER_TYPE EnemyType
    {
        get { return m_eType; }
        set { m_eType = value; }
    }
    private List<CharPartyData> m_ListEnemyParty = new List<CharPartyData>(); //에너미 파티용 데이터

    public void Init(int iMapIndex, int iBlockIndex,  CHARACTER_TYPE eType)
    {
        m_iMapIndex = iMapIndex;  //에너미 인덱스 숫자
        m_iBlockIndex = iBlockIndex;
        m_eType = eType;

        //파티 데이터만 셋팅
        try
        {
            string Route = "Excel/MapData/" + m_iMapIndex + "/" + m_iBlockIndex + "_" + eType.ToString();
            var EnemyTable = EXCEL.ExcelLoad.Read(Route);

            for (int i = 0; i < EnemyTable.Count; i++)
            {
                Route = "Excel/EnemyClass/" + EnemyTable[i][CHARACTER_DATA.CHAR_CLASS_TYPE.ToString()];
                var EnemyClass = EXCEL.ExcelLoad.Read(Route);
                PartyDataSetting(i, EnemyTable, EnemyClass);
            }
            //에너미 데이터 셋팅
            Debug.Log("Enemy Data Setting");
        }
        catch
        {
            Debug.Log("Enemy Data Setting Error");
        }
    }

    public void PartyDataSetting(int iIndex, List<Dictionary<string, object>> Table, List<Dictionary<string, object>> Class)
    {
        CharPartyData Node = new CharPartyData();
        Node.PartyIndex = iIndex;
        Node.CharType = m_eType;
        Node.CharData = new CharacterData();
        Node.CharData.Init(Table, Class, iIndex);

        Node.MaxHP = float.Parse(Util.ConvertToString(Node.CharData.ReturnData(CHARACTER_DATA.CHAR_MAX_HP)));
        Node.CurHP = Node.MaxHP;

        Node.MaxStemina = 3;

        string[] arr = Util.ConvertToString(Node.CharData.ReturnData(CHARACTER_DATA.CHAR_SPEED)).Split(';');
        Node.FillStemina = Util.ConvertToInt(arr[0]);   //턴당
        Node.CurFillStemina = 1;
        Node.SteminaCurFill = Util.ConvertToInt(arr[1]);  //이러한 게이지가 찬다.
        Node.CurStemina = Node.SteminaCurFill;
        Node.Stay = false;

        m_ListEnemyParty.Add(Node);
    }

    public void EnemyPartySet()
    {
        try
        {
            //에너미 파티 데이터를 게임 매니저로 가져간다.
            GameManager.instance.EnemyParty.Clear();
            //참조 복사로 해도 별 상관은 없음
            GameManager.instance.EnemyParty = m_ListEnemyParty;
        }
        catch
        {
            Debug.Log("Enemy Party Set Error");
        }
    }

    public void EnemyListClear()
    {
        //초기화
        m_ListEnemyParty.Clear();
        m_iMapIndex = -1;  //에너미 인덱스 숫자
        m_iBlockIndex = -1;
        m_eType = CHARACTER_TYPE.CHAR_NONE;
    }
}
