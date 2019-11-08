using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    private SpineManager m_Spine;
    // Start is called before the first frame update

    private void Start()
    {
        m_Spine = transform.parent.GetComponent<SpineManager>();
    }

    public void DeathAnimationEnd()
    {
        m_Spine.DeathAnimationEnd();
    }
}
