using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Helpers
{
    public class ReplyModel
    {
        /// <summary>
        /// 需要解析的数据
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 0 失败
        /// 1 成功
        /// </summary>
        public int Success { get; set; }
        public string Message { get; set; }
    }
}
