/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 10/25/2017 1:35:21 PM
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

namespace Common
{
    public enum ParameterCode :byte //区分传送数据的时候，参数的类型
    {
        Username,
        Password,
        Position,
        X,Y,Z,
        UsernameList,
        PlayerDataList
    }
}
