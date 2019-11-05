using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SendTool
{
    public class MyReceiveFilter : TerminatorReceiveFilter<StringPackageInfo>
    {
        public MyReceiveFilter()
        : base(Encoding.ASCII.GetBytes("#")) // two vertical bars as package terminator
        {
        }

        // other code you need implement according yoru protocol details

        public override StringPackageInfo ResolvePackage(IBufferStream b)
        {
            var text = b.ReadString((int)b.Length, Encoding.Default);
            text = text.Substring(0, text.Length - 1).Trim();

            int index = text.IndexOf(" ");
            if (index < 0)
            {
                return new StringPackageInfo(text, "", null);
            }
            else
            {
                string key = text.Substring(0, index);
                string body = text.Substring(index + 1);

                return new StringPackageInfo(key, body, null);
            }


        }
    }
}
