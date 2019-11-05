using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Helpers
{
    public class BaseModel
    {
        /// <summary>
        /// 发送的内容
        /// </summary>
        public object Content { get; set; }
        public string FromDeviceId { get; set; }
        /// <summary>
        /// IP地址+','+deviceId
        /// </summary>
        public string ToDeviceId { get; set; }
        public string CommandId { get; set; }

        public static string GetDeviceId(string ip, string deviceId)
        {
            return ip + "," + deviceId;
        }
    }
}
