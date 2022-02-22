using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.PubSubEvents
{
    public class LoadingTextChangeEvent : PubSubEvent<EventParam>
    {

    }

    public class EventParam
    {
        public EventParam(object _Item)
        {
            Item = _Item;
        }

        public object Item { get; set; }
    }
}
