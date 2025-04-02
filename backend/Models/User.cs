using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public  string? Id  { get; set; }

    [BsonElement("email")]
    public  required string Email { get; set; }

    [BsonElement("passwordHash")]
    public  required string PasswordHash { get; set; }

    [BsonElement("fullName")]
    public  required string FullName { get; set; }
}
