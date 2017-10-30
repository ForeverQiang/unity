/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 10/26/2017 1:09:07 PM
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


namespace Common.Tools
{
    public class DictTool
    {
        public static T2 GetValue<T1,T2>(Dictionary<T1,T2> dict,T1 key)
        {
            T2 value;
            bool isSuccess =  dict.TryGetValue(key, out value);
            if(isSuccess)
            {
                return value;
            }
            else
            {
                return default(T2);
            }
        }
    }
}
