/*
用户类
*/
using System;
using System.Text.Json.Serialization;
public class User
{
    [JsonPropertyName("id")]
    public string id;
    [JsonPropertyName("name")]
    public string name;
    [JsonPropertyName("password")]
    public string password;

    public User(string name, string password)
    {
        this.id = Guid.NewGuid().ToString();
        this.name = name;
        this.password = password;
    }
}