using Dapper;
using HomeBudgetManagement.Application.EventFeed;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace HomeBudgetManagement.Infrastructure.EventFeed
{
    /// <summary>
    /// Here we use dapper so which is micro ORM that is faster than traditional ORM like EF.
    /// We can implement other database provider here besides sql.
    /// Dapper works by extending IDbConnection
    /// </summary>
    public class EventFeedSql : IEventFeed
    {
        private readonly IConfiguration _configuration;
        IDbConnection _dbConnection;

        public EventFeedSql(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new SqlConnection(_configuration.GetConnectionString("HbmConnectionString"));
            _dbConnection.Open();
        }

        public async Task<List<object>> GetByNameAsync(string eventName)
        {
            var events = await _dbConnection.QueryAsync("SELECT *" +
               "FROM " +
               "   Events " +
               "WHERE " +
               "   EventName = @Name",
               new { Name = eventName });

            return events.ToList();
        }


        public async Task PublishAsync(string eventName, object content)
        {
            var isTableExist = _dbConnection.Query<int>("SELECT " +
                "   COUNT (*) " +
                "FROM " +
                "   information_schema.tables " +
                "WHERE " +
                "   table_name = @table_name", 
                new { table_name = "Events" }).First() == 1;

            if (!isTableExist)
            {
                await _dbConnection.ExecuteAsync(@"CREATE TABLE [dbo].[Events] (
                        [StreamName] VARCHAR (100) NULL,
                        [EventName]  VARCHAR (150) NULL,
                        [Content]    VARBINARY (MAX) NULL,
                        [OccuredAt]  DATETIME      DEFAULT (getdate()) NULL
                    );"
                );
            }

            var resultCount = await _dbConnection.ExecuteAsync
                                 (@"INSERT 
                                        Events(StreamName, EventName, Content)
                                    VALUES 
                                        (@StreamName, @EventName, @Content)",
                                    new EventData("expense-stream",
                                                    eventName,
                                                    Encoding.UTF8.GetBytes(JsonSerializer.Serialize(content)),
                                                    DateTime.Now)
                                  );

            _dbConnection.Close();

            Debug.WriteLine(resultCount);
        }

        public record EventData(string StreamName, string EventName, byte[] Content, DateTime OccuredAt );
    }
}
