using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace Nexora.Core.Common.Extensions
{
    public static class SerilogExtension
    {
        public static void AddSerilogService(this WebApplicationBuilder builder, Dictionary<string, string>? customStaticColumnData = null)
        {
            try
            {
                var loggerConfiguration = new LoggerConfiguration();

                if (customStaticColumnData != null)
                {
                    foreach (var customStaticColumn in customStaticColumnData)
                    {
                        loggerConfiguration.Enrich.WithProperty(customStaticColumn.Key, customStaticColumn.Value);
                    }
                }

                loggerConfiguration.AddMssqlLogger(builder.Configuration);

                Log.Logger = loggerConfiguration.CreateLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error configuring Serilog: {ex.Message}");
            }

            builder.Host.UseSerilog();
        }

        private static void AddMssqlLogger(this LoggerConfiguration loggerConfiguration, ConfigurationManager configuration)
        {
            List<SqlColumn> customDataColumn = new List<SqlColumn>();

            customDataColumn.Add(new SqlColumn() { ColumnName = "Application", DataType = SqlDbType.NVarChar, AllowNull = false });
            customDataColumn.Add(new SqlColumn() { ColumnName = "ChannelType", DataType = SqlDbType.TinyInt, AllowNull = true });

            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = customDataColumn
            };

            loggerConfiguration.Enrich.FromLogContext()
                .WriteTo
                .MSSqlServer(
                    connectionString: configuration.GetSection("ConnectionStrings:ApplicationDbContext").Value,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = configuration.GetSection("Log:Mssql:TableName").Value ?? "Logs",
                        AutoCreateSqlTable = true,
                    },
                    columnOptions: columnOptions,
                    restrictedToMinimumLevel: GetLogEventLevel(configuration.GetSection("Log:Mssql:MinimumLogLevel").Value ?? "Error"))
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning);
        }

        private static LogEventLevel GetLogEventLevel(string minimumLogLevel)
        {
            return !string.IsNullOrEmpty(minimumLogLevel) ? (LogEventLevel)Enum.Parse(typeof(LogEventLevel), minimumLogLevel, true) : LogEventLevel.Verbose;
        }
    }
}