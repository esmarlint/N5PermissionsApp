namespace N5PermissionsAPI.Core.Common;

public class Constans
{
    public static class Kafka
    {
        public const string RequestOperation = "request";
        public const string ModifyOperation = "modify";
        public const string GetOperation = "get";
    }

    public static class Environment
    {

        public static class Kafka
        {
            public const string KafkaBootstrapServers = "Kafka:BootstrapServers";
            public const string Topic = "Topic";
        }

        public static class Database
        {
            public const string Connection = "Database:Connection";
        }

        public static class Elasticsearch
        {
            public const string Index = "Elasticsearch:Index";
            public const string Url = "Url";
            public const string UserName = "Username";
            public const string Password = "Password";
        }

    }
}
