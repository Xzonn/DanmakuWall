using Native.Csharp.App;
using System;
using System.Collections.Generic;
using Native.Csharp.Sdk.Cqp.Model;

namespace Native.Csharp
{
    public class Users
    {
        private Dictionary<long, string> Names = new Dictionary<long, string>();
        public string getName(long qq)
        {
            if (Names.ContainsKey(qq))
            {
                return Names[qq];
            }
            else
            {
                try
                {
                    QQ qqInfo;
                    Common.CqApi.GetQQInfo(qq, out qqInfo);
                    Names[qq] = qqInfo.Nick;
                    return qqInfo.Nick;
                }
                catch
                {
                    return "";
                }
            }
        }
    }
}
