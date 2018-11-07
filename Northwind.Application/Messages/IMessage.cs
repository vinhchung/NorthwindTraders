using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Application.Messages
{
    public interface IMessage
    {
        string Text { get; }
        DateTime Created { get; }
    }
}
