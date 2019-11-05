using SuperSocket.Helpers;
using SuperSocket.ProtoBase;
using SuperSocket.SendTool;
//using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperSocket.ClientSend
{
    public partial class Form1 : Form
    {
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

        public Form1()
        {
            InitializeComponent();
        }

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
                  #endregion

              };

            this.Client = new SocketClient(this.IP, this.Port, this.DeviceId, action);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //模拟发送其它命令，，带回复
            this.Client.SendToClient("move", new MoveModel { X = 1, Y = 2, Z = 3 }, ConfigurationManager.AppSettings["toDeviceId"], Guid.NewGuid().ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //模拟发送重启
            this.Client.SendToClient("restart", "", ConfigurationManager.AppSettings["toDeviceId"]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Client.SendToClient("send", "", "");
        }
    }
}
