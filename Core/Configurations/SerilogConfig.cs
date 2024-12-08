using Domain.Constants;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.PostgreSQL;

namespace Core.Configurations;

public static class SerilogConfig
{
    public static void AddSerilog(this ConfigureHostBuilder host)
    {
        Serilog.Debugging.SelfLog.Enable(Console.Error);
        host.UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Logger(loggerConfiguration => loggerConfiguration
                    .Filter.ByIncludingOnly(Matching.WithProperty(LoggerConstant.PROPERTY_SAVE_LOG))
                    .WriteTo.PostgreSQL(
                        DatabaseConfig.GetConnectionString(),
                        LoggerConstant.REQUEST_TABLE_NAME,
                        LoggerConfig.ColumnWriters,
                        restrictedToMinimumLevel: LogEventLevel.Information
                    )
                )
                .WriteTo.Logger(loggerConfiguration => loggerConfiguration
                    .Filter.ByExcluding(Matching.WithProperty(LoggerConstant.PROPERTY_SAVE_LOG))
                    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                );
        });
    }
}

public static class LoggerConfig
{
    public static IDictionary<string, ColumnWriterBase> ColumnWriters = new Dictionary<string, ColumnWriterBase>
    {
        {LoggerConstant.PROPERTY_REQUEST_ID, new SinglePropertyColumnWriter(LoggerConstant.PROPERTY_REQUEST_ID, PropertyWriteMethod.Raw, NpgsqlDbType.Uuid)},
        {LoggerConstant.PROPERTY_URL, new SinglePropertyColumnWriter(LoggerConstant.PROPERTY_URL, PropertyWriteMethod.Raw, NpgsqlDbType.Varchar, null, 100)},
        {LoggerConstant.PROPERTY_REQUEST_DATA, new SinglePropertyColumnWriter(LoggerConstant.PROPERTY_REQUEST_DATA, PropertyWriteMethod.Raw, NpgsqlDbType.Text)},
        {LoggerConstant.PROPERTY_RESPONSE_DATA, new SinglePropertyColumnWriter(LoggerConstant.PROPERTY_RESPONSE_DATA, PropertyWriteMethod.Raw, NpgsqlDbType.Text)},
        {LoggerConstant.PROPERTY_LOG_DATE, new SinglePropertyColumnWriter(LoggerConstant.PROPERTY_LOG_DATE, PropertyWriteMethod.Raw, NpgsqlDbType.Timestamp)},
        {LoggerConstant.PROPERTY_HTTP_CODE, new SinglePropertyColumnWriter(LoggerConstant.PROPERTY_HTTP_CODE, PropertyWriteMethod.Raw, NpgsqlDbType.Integer)},
        {LoggerConstant.PROPERTY_REQUEST_TYPE_ID, new SinglePropertyColumnWriter(LoggerConstant.PROPERTY_REQUEST_TYPE_ID, PropertyWriteMethod.Raw, NpgsqlDbType.Uuid)}
    };
}