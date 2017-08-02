/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 8/2/2017 5:07:59 PM
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
using System.Threading.Tasks;

namespace Serv.core
{
    //协议基类
    public class ProtocolBase
    {
        //解码器，解码readbuff中从start开始的length字节
        public virtual ProtocolBase Decode(byte[] readbuff,int start,int length)
        {
            return new ProtocolBase();
        }

        //编码器
        public virtual byte[] Encode()
        {
            return new byte[] { };
        }

        //协议名称，用于消息分发
        public virtual string GetName()
        {
            return "";
        }

        //描述
        public virtual string GetDesc()
        {
            return "";
        }

       
    }
}
