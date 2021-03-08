using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Discord.WebSocket
{
    [DataContract]
    public struct GatewayMessage
    {
        [DataMember(Name = "op", Order = 1)]
        public int? OpCode { get; set; }
        
        [DataMember(Name = "d", Order = 2)]
        public object Data { get; set; }
        
        [DataMember(Name = "s", Order = 3)]
        public int? SequenceNumber { get; set; }
        
        [DataMember(Name = "t", Order = 4)]
        public string EventName { get; set; }
    }
}
