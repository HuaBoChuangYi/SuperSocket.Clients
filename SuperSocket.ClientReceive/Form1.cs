using SuperSocket.ClientEngine;
using SuperSocket.Helpers;
using SuperSocket.ProtoBase;
using SuperSocket.SendTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperSocket.ClientReceive
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

        }
        public string DeviceId
        {
            get
            {
                return ConfigurationManager.AppSettings["deviceId"];
            }
        }

        public string IP
        {
            get
            {
                return ConfigurationManager.AppSettings["ip"];
            }
        }

        public int Port
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
            }
        }

        public ISend Client { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {

            //信息接收处理
            Action<StringPackageInfo> action = (request) =>
            {

                string key = request.Key;
                string body = request.Body;
                var baseModel = JsonHelper.DeserializeJsonToObject<BaseModel>(body);

                #region 测试接收代码
                string message = key + " " + body;

                if (richTextBox1.InvokeRequired)
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<string> actionDelegate = (x) =>
                    {
                        richTextBox1.AppendText(message);
                        richTextBox1.Select(richTextBox1.Text.Length, 0);
                        richTextBox1.ScrollToCaret();
                    };
                    // =
                    this.richTextBox1.Invoke(actionDelegate, DateTime.Now + " " + message);
                }
                else
                {
                    this.richTextBox1.Text = message;
                }


                if (key.ToLower() == "move")
                {
                    var model = JsonHelper.DeserializeJsonToObject<MoveModel>(baseModel.Content + "");
                    model.X++;
                    model.Y++;
                    model.Z++;
                    baseModel.Content = new ReplyModel
                    {
                        Data = model,
                        Success = 1
                    };
                    this.Client.ReplyToClient(baseModel);
                }
                else if (key.ToLower() == "restart")
                {
                    if (baseModel != null)
                    {
                        baseModel.Content = new ReplyModel
                        {
                            Message = "restart success!",
                            Success = 1
                        };
                        this.Client.ReplyToClient(baseModel);
                    }
                    Application.Restart();
                }
                //else if (key.ToLower() == "changestatus")
                //{
                //    baseModel.Content = new ReplyModel
                //    {
                //        Data = "",
                //        Message = "修改成功",
                //        Success = 1
                //    };
                //    client.Send(Encoding.ASCII.GetBytes("Send Reply " + JsonHelper.SerializeObject(baseModel) + "#"));
                //}
                #endregion

            };

            this.Client = new SocketClient(this.IP, this.Port, this.DeviceId, action);



        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
