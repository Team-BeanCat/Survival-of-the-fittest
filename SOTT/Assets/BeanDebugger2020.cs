using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeanDebugger2020 : MonoBehaviour
{
    public Toggle toggle;
    public static bool _beanDemandsDebug = false;
    
    public void Change(bool state)
    {
        _beanDemandsDebug = state;
    }
}
