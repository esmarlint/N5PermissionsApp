namespace N5PermissionsAPI.Infrastructure.Configuration;

public class ElasticsearchOptions
{
    public string Url { get; set; }
    public string IndexName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
