using SuperSocket.ClientEngine;
using SuperSocket.Helpers;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SuperSocket.SendTool
{
    public class SocketClient : ISend
    {
        private string IP { get; set; }
        private int Port { get; set; }
        private string DeviceId { get; set; }
        public bool IsConnected { get; set; }
        public Action<StringPackageInfo> Action { get; set; }

        public EasyClient Client { get; set; }
        public SocketClient(string ip, int port, string deviceId, Action<StringPackageInfo> action)
        {
            this.IP = ip;
            this.Port = port;
            this.DeviceId = deviceId;
            this.Action = action;
            this.Client = new EasyClient();
            this.Client.Connected += Client_Connected;
            this.Client.Error += Client_Error;
            this.Client.Closed += Client_Closed;
            Init();
        }

        private void Client_Closed(object sender, EventArgs e)
        {

        }

        private void Client_Error(object sender, ErrorEventArgs e)
        {
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            var sendData = Encoding.ASCII.GetBytes("ReplyDeviceId " + JsonHelper.SerializeObject(new BaseModel
            {
                Content = new ReplyDeviceIdModel
                {
                    DeviceId = this.DeviceId
                }
            }) + "#");
            this.Client.Send(sendData);
        }

        public async void Init()
        {
            this.Client.Initialize(new MyReceiveFilter(), this.Action);
            this.IsConnected = await this.Client.ConnectAsync(new IPEndPoint(IPAddress.Parse(this.IP), this.Port));

            Timer timer = new Timer
            {
                Interval = 2000
            };
            timer.Elapsed += async (m, n) =>
            {
                if (!this.Client.IsConnected)
                {
                    this.IsConnected = await this.Client.ConnectAsync(new IPEndPoint(IPAddress.Parse(this.IP), this.Port));
                }
            };
            timer.Start();
        }


        /// <summary>
        /// 发送给服务端，一般不需要
        /// </summary>
        /// <param name="key">命令头</param>
        /// <param name="content">命令内容</param>
        /// <param name="toDeviceID">对方的</param>
        /// <param name="isReply">是否需要回复</param>
        public void SendToServer(string key, object content)
        {
            var sendData = Encoding.ASCII.GetBytes(key + " " + JsonHelper.SerializeObject(new BaseModel
            {
                Content = content
            }) + "#");
            this.Client.Send(sendData);
        }

        /// <summary>
        /// 发送给其它客户端
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <param name="toDeviceID"></param>
        /// <param name="commandId">如果需要回复，则自己定,推荐guid</param>
        public void SendToClient(string key, object content, string toDeviceID, string commandId = "")
        {
            var model = new BaseModel();
            if (string.IsNullOrEmpty(commandId))
            {
                model = new BaseModel
                {
                    Content = content,
                    ToDeviceId = toDeviceID
                };
            }
            else
            {
                model = new BaseModel
                {
                    Content = content,
                    ToDeviceId = toDeviceID,
                    CommandId = commandId
                };
            }

            var sendData = Encoding.ASCII.GetBytes("send " + key + " " + JsonHelper.SerializeObject(model) + "#");
            this.Client.Send(sendData);
        }

        public void ReplyToClient(BaseModel model)
        {
            var sendData = Encoding.ASCII.GetBytes("send reply " + JsonHelper.SerializeObject(model) + "#");
            this.Client.Send(sendData);
        }

        public void ReplyToServer(ReplyModel model)
        {
            var sendData = Encoding.ASCII.GetBytes("reply " + JsonHelper.SerializeObject(model) + "#");
            this.Client.Send(sendData);
        }



    }
}
