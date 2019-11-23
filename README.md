# LogMonkey
The core idea behind this project is to provide a flexible logging mechanism to .NET developers.

#Key Features
1) Configurable Logging options allows you to - configure once & use through out your project.
2) Supports logging in both database (MSSQL as of now) & file.
3) Fallback mode - if primary logging mode fails, you can configure a secondary mode.
4) Switch between two configurations.

Firstly - to use the database, create the following table -
```
CREATE SCHEMA LMK
CREATE TABLE [LMK].[LOG_HISTORY] (
    [EXCEPTION_ID]      INT             IDENTITY (1, 1) NOT NULL,
    [EXCEPTION]         NVARCHAR (MAX)  NULL,
    [INNER_EXCEPTION]   NVARCHAR (MAX)  NULL,
    [STACK_TRACE]       NVARCHAR (MAX)  NULL,
    [EXCEPTION_METHOD]  NVARCHAR (100)  NULL,
    [EXCEPTION_CLASS]   NVARCHAR (100)  NULL,
    [EXCEPTION_DATE]    DATE            NULL,
    [EXCEPTION_TIME]    TIME (7)        NULL,
    [EXCEPTION_TYPE]    NVARCHAR (30)   NULL,
    [EXTRA_INFORMATION] NVARCHAR (3000) NULL,
    CONSTRAINT [LH_PRIMARY_KEY] PRIMARY KEY CLUSTERED ([EXCEPTION_ID] ASC)
);
```
Example usage of the library -

```
class Program
{
    static void Main(string[] args)
    {
        //Dummy SqlConnection, to be replaced by an actual initialized connection for use.
        String sqlConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=workdb;Integrated Security=True";

        //Build the configuration
        LoggerConfiguration configuration = LoggerConfiguration
           .Builder()
           .SetPrimaryLoggingMode(LogMode.Database)
           .SetFallbackLoggingMode(LogMode.File)
           .SetDatabaseConnectionString(sqlConnectionString)
           .SetLogInnerException(true)
           .SetFilePath("D://")
           .Build();

        //Build the alternate configuration
        LoggerConfiguration alternateConfiguration = LoggerConfiguration
           .Builder()
           .SetPrimaryLoggingMode(LogMode.File)
           .SetFallbackLoggingMode(LogMode.Database)
           .SetDatabaseConnectionString(sqlConnectionString)
           .SetLogInnerException(false)
           .SetFilePath("D://Logs")
           .Build();

        //Initialization of the logger
        Logger.Initialize(configuration, alternateConfiguration);

        //Get a singleton instance.
        Logger logger = Logger.Instance();

        //switch logger configurations.
        logger.SwitchConfiguration();

        try
        {
            throw new DivideByZeroException();
        }
        catch (Exception ex)
        {
            logger.Write(LogType.Information, ex, "test", "Main", "Additional information");
        }

        //Make the console wait.
        Console.ReadKey();
    }
}

```

Note that,  ``` .SetFilePath("D://Logs") ```, ensure the directory Logs exist, the library will try to write to that directory directory without 
creating it.
