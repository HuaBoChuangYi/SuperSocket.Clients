using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Helpers
{
    public interface ISend
    {
        /// <summary>
        /// 发送给服务端，一般不需要
        /// </summary>
        /// <param name="key">命令头</param>
        /// <param name="content">命令内容</param>
        /// <param name="toDeviceID">对方的</param>
        /// <param name="isReply">是否需要回复</param>
        void SendToServer(string key, object content);

        /// <summary>
        /// 发送给其它客户端
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <param name="toDeviceID"></param>
        /// <param name="commandId">如果需要回复，则自己定,推荐guid</param>
        void SendToClient(string key, object content, string toDeviceID, string commandId = "");

        void ReplyToClient(BaseModel model);

        void ReplyToServer(ReplyModel model);
    }
}
