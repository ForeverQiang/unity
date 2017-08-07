/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 8/7/2017 2:14:52 PM
 * 当前版本： 1.0.0.1
 * 编写系统： ASUS-PC
 * 区    域： ASUS-PC
 * 描述说明：
 * 修改历史：
 * ************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Root : MonoBehaviour
{
    private void Update()
    {
        NetMgr.Update();
    }
}