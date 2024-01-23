using Aspire.Hosting;
using k8s.Models;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.AspireApp1_ApiService>("apiservice");

builder.AddProject<Projects.AspireApp1_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiService);


#region Autres exemples
#region SQL Server
/* 

    var addressBookDb = builder.AddSqlServerContainer("sqlserver")
        .WithVolumeMount("./sqlserverconfig", "/usr/config", VolumeMountType.Bind) // Mount the init scripts directory into the container.
        .WithVolumeMount("../DatabaseContainers.ApiService/data/sqlserver", 
            "/docker-entrypoint-initdb.d", VolumeMountType.Bind) // Mount the SQL scripts directory into the container so that the init scripts run.
        .WithArgs("/usr/config/entrypoint.sh") // Run the custom entrypoint script on startup.
        .AddDatabase("AddressBook");
*/
#endregion

#region Custom container
/*
    var customerApi = builder
        .AddContainer("customerapi", image: "contoso.com/eshop/customers")
        .WithServiceBinding(containerPort: 8080, scheme: "http");

    builder.AddProject<Projects.AspireApp1_Web>("webfrontend")
        .WithReference(customerApi);

*/
#endregion

#region API existante
/*
    builder.AddProject<Projects.AspireApp30_Web>("webfrontend")
        .WithReference(cache)
        .WithReference("apiservice", new Uri("http://apiservice.v2.dev.contoso.com"));
    // ==> The app can reference this service simply via http://apiservice
*/
#endregion

#region Referencer un projet .CSPROJ
/*
    var pathBasedProject = builder
        .AddProject(name: "customerapi",projectPath: "../../submodules/customerapi/src/CustomerApi/CustomerApi.csproj");  
            // relative to the AppHost project directory
*/

#endregion

#region NodeJS application 
/*
    builder.AddNodeApp("webapp", "../webapp/app.js")
        .WithReference(apiservice)
        .WithReference(cache)
        .WithServiceBinding(scheme: "http", env: "PORT") // Dynamically assign an http port for the the Node.js app, and inject it in the 'PORT' environment variable.
        .AsDockerfileInManifest(); // 'in container' deployment
*/
#endregion

#region NPM application
/*
    builder.AddNpmApp("mynbpmapp","../mynpmapp_folder")
            .WithServiceBinding(scheme: "http", env: "PORT") 
            .AsDockerfileInManifest(); // 'in container' deployment
*/
#endregion

#region MySQL
/*
    var mysqlCatalogDb = builder.AddMySqlContainer("mysql")
        .WithVolumeMount("../DatabaseContainers.ApiService/data/mysql", 
            "/docker-entrypoint-initdb.d", 
            VolumeMountType.Bind) // Mount the SQL scripts directory into the container so that the init scripts run.
        .AddDatabase("catalog");

    builder.AddProject<Projects.MyApiService>("catalogueapi")
        .WithReference(mysqlCatalogDb)
*
*/
/* // catalogapi : on se fait quelques lignes d'api pour le plaisir

    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults(); // Add service defaults & Aspire components.
    builder.AddMySqlDataSource("catalog");

    var app = builder.Build();
    app.MapGet("/catalog", async (MySqlConnection db) =>
    {
        const string sql = "SELECT Id, Name, Description, Price FROM catalog";
        return await db.QueryAsync<CatalogItem>(sql);
    });
    app.MapDefaultEndpoints();
    app.Run();
*/

#endregion

#region MongoDB
/*
        var mongodb = builder
            .AddMongoDBContainer("mongodb")
            .AddDatabase("mydatabase");
        var myService = builder.AddProject<Projects.MyService>()
            .WithReference(mongodb);
*/

/* // le projet API 'MyService'
    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults();     // Add service defaults & Aspire components.
    builder.AddMongoDBClient("mydatabase");
    var app = builder.Build();
    app.MapGet("/", async (IMongoClient client) =>
    {
        // Use the client here and return result
    });

    app.MapDefaultEndpoints();

    app.Run();


*/
#endregion



#endregion

builder.Build().Run();
