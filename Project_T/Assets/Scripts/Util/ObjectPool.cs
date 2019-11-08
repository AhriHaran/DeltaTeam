using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    //하나의 오브젝트를 대응하는 풀
    private string m_strName;
    private int m_iPoolCount = 0;
    public List<GameObject> m_ListPool = new List<GameObject>();

    public void Init(string strName, int iCount, Transform Parent)
    {
        m_strName = strName;
        m_iPoolCount = iCount;
        //초기화
        for (int i = 0; i < m_iPoolCount; i++)
        {
            m_ListPool.Add(CreateItem(Parent));
        }
    }

    public void PushToPool(GameObject item, Transform Parent)    //순서가 중요하지 않음
    {
        //오브젝를 사용후 반환 할 때
        item.transform.SetParent(Parent, false);
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one;
        //로컬 트랜스폼 리셋
        item.transform.rotation = Quaternion.identity;
        item.transform.position = Vector3.zero;

        item.SetActive(false);
        m_ListPool.Add(item);
    }

    public GameObject PopFromPoll(Transform parent)  //순서가 중요하지 않은 오브젝트
    {
        if (m_ListPool.Count == 0)
            m_ListPool.Add(CreateItem(parent));
        GameObject item = m_ListPool[0];
        m_ListPool.RemoveAt(0);
        return item;
    }

    private GameObject CreateItem(Transform parent)
    {
        //크리에이트
        GameObject Item = ResourceLoader.CreatePrefab(m_strName, parent);
        Item.SetActive(false);
        return Item;
    }

    public void Clear()
    {
        m_ListPool.Clear();
    }
}
