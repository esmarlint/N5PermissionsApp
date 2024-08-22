namespace N5PermissionsAPI.Core.DTOs
{
    public class KafkaOperationDto
    {
        public Guid Id { get; set; }
        public string OperationName { get; set; }
        public DateTime Timestamp { get; set; }
        public object Data { get; set; }
    }
}
