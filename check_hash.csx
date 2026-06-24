#r "nuget: BCrypt.Net-Next, 4.0.3"
using BCrypt.Net;
Console.WriteLine(BCrypt.Net.BCrypt.Verify("admin123", "$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy"));
