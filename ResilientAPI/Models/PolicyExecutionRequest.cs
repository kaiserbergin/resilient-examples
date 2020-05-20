using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResilientAPI.Models
{
    public class PolicyExecutionRequest 
    {        
        public PolicyExecutionType PolicyExecution { get; set; }
        public GenerateResponseRequestWaitWhat InnerHttpCall { get; set; }
    }
}
