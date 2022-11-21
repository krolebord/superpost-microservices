using System.CommandLine;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Migrator;

var dllFileOption = new Option<string>("--dll", "Dll file name") { IsRequired = true };
var connectionStringOption = new Option<string>("--str", "Connection string") { IsRequired = true };

var migrateCommand = new RootCommand("Migrate db");
migrateCommand.AddOption(connectionStringOption);
migrateCommand.AddOption(dllFileOption);

migrateCommand.SetHandler(async (dll, str) => 
{
    var directory = AssemblyHelpers.GetAssemblyDirectory();
    var path = Path.Join(directory, dll);
    if (!File.Exists(path)) throw new FileNotFoundException();
    
    var assembly = Assembly.LoadFile(path) ?? throw new Exception("Couldn't load target assembly");
    var contextTypes = assembly
        .GetExportedTypes()
        .Where(x => x.IsClass && x.IsAssignableTo(typeof(DbContext)))
        .ToList();

    switch (contextTypes.Count)
    {
        case 0:
            throw new Exception("Couldn't find context type");
        case > 1:
            throw new Exception("Multiple context types found");
    }

    var contextType = contextTypes.First();
    var optionsBuilderType = typeof(DbContextOptionsBuilder<>).MakeGenericType(contextType);
    dynamic optionsBuilder = Activator.CreateInstance(optionsBuilderType) ?? throw new Exception("Couldn't create context options builder");
    NpgsqlDbContextOptionsBuilderExtensions.UseNpgsql(optionsBuilder, str);
    ((DbContextOptionsBuilder) optionsBuilder).LogTo(Console.WriteLine);

    var context = (DbContext)Activator.CreateInstance(contextType, optionsBuilder.Options) ?? throw new Exception("Couldn't create db context");
    await context.Database.MigrateAsync();
}, dllFileOption, connectionStringOption);

await migrateCommand.InvokeAsync(args);
