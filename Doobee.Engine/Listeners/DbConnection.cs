using System;
using System.Transactions;

namespace Doobee.Engine.Listeners;

public class DbConnection
{
    public string Id { get; }
    public Transaction? CurrentTransaction { get; set; }

    public DbConnection()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string[] SqlStatements { get; set; }
}
