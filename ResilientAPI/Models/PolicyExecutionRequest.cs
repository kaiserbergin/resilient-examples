namespace ResilientAPI.Models
{
    public class PolicyExecutionRequest 
    {        
        public PolicyExecutionType PolicyExecution { get; set; }
        public GenerateResponseRequestWaitWhat InnerHttpCall { get; set; }
    }
}
