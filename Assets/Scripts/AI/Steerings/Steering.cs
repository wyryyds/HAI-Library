using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    /// <summary>
    ///�ٿ�����Ȩ��
    /// </summary>
    public float Weight = 1;
    /// <summary>
    /// �ṩ�ٿ���
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 Force()
    {
        return Vector3.zero;
    }
}
