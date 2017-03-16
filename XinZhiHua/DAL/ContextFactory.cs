using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

public class ContextFactory
{
    /// <summary>
    /// 获取当前数据上下文
    /// </summary>
    /// <returns></returns>
    public static MyDbContext GetCurrentContext()
    {
        MyDbContext context = CallContext.GetData("MyContext") as MyDbContext;
        if (context == null)
        {
            context = new MyDbContext();
            CallContext.SetData("MyContext", context);
        }
        return context;
    }
}
