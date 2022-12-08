// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Security.Principal;
using System.Security;
using System.Text;

Console.WriteLine("Cración de archivos de controllers y servicios par alas entidades faltantes");

// Lista de entidades a las que se les crearán los archivos

string[] entitiesList = new string[22]
{
    "AppPayment",
    "Client",
    "Discount",
    "Holder",
    "Log",
    "ModelBase",
    "MoneyTransaction",
    "OwedDetail",
    "Owner",
    "Payment",
    "Peripheral",
    "PeripheralsType",
    "Permission",
    "PettyCash",
    "Record",
    "RentalComplex",
    "RentalComplexDebtState",
    "RentalUnitDebtState",
    "RentalUnitPayment",
    "Resident",
    "User",
    "UserType"
};

string entityNameUpperCase = string.Empty;
string entityNameLowerCase = string.Empty;
// Definición de template del contenido para archivos


// Se itera el arreglo para crear los archivos Servicios
Console.WriteLine("Comienza creación de archivos Servicios:");
foreach (string entity in entitiesList)
{
    try
    {

        Console.WriteLine($"Creación de archivo {entity}Service.");
        entityNameLowerCase = entity[0].ToString().ToLower() + entity.Substring(1);
        entityNameUpperCase = entity;
        string serviceFileContent = $"using AARP_BE.Models;\n" +
                                $"using Microsoft.Extensions.Options;\n" +
                                $"using MongoDB.Driver;\n" +
                                $"\n" +
                                $"namespace AARP_BE.Services\n" +
                                $"{{\n" +
                                $"    public class {entityNameUpperCase}Service\n" +
                                $"    {{\n" +
                                $"        private readonly IMongoCollection<{entityNameUpperCase}> _{entityNameLowerCase}sCollection;\n" +
                                $"\n" +
                                $"        // <snippet_ctor>\n" +
                                $"        public {entityNameUpperCase}Service(\n" +
                                $"            IOptions<ResidenceAdministratorDatabaseSettings> residenceAdministratorDatabaseSettings)\n" +
                                $"        {{\n" +
                                $"            var mongoClient = new MongoClient(\n" +
                                $"                residenceAdministratorDatabaseSettings.Value.ConnectionString);\n" +
                                $"\n" +
                                $"            var mongoDatabase = mongoClient.GetDatabase(\n" +
                                $"                residenceAdministratorDatabaseSettings.Value.DatabaseName);\n" +
                                $"\n" +
                                $"            _{entityNameLowerCase}sCollection = mongoDatabase.GetCollection<{entityNameUpperCase}>(\n" +
                                $"                residenceAdministratorDatabaseSettings.Value.RentalUnitsCollectionName);\n" +
                                $"        }}\n" +
                                $"        // </snippet_ctor>\n" +
                                $"\n" +
                                $"        public async Task<List<{entityNameUpperCase}>> GetAsync() =>\n" +
                                $"            await _{entityNameLowerCase}sCollection.Find(_ => true).ToListAsync();\n" +
                                $"\n" +
                                $"        public async Task<{entityNameUpperCase}?> GetAsync(string id) =>\n" +
                                $"            await _{entityNameLowerCase}sCollection.Find(x => x.Id == id).FirstOrDefaultAsync();\n" +
                                $"\n" +
                                $"        public async Task CreateAsync({entityNameUpperCase} new{entityNameUpperCase}) =>\n" +
                                $"            await _{entityNameLowerCase}sCollection.InsertOneAsync(new{entityNameUpperCase});\n" +
                                $"\n" +
                                $"        public async Task UpdateAsync(string id, {entityNameUpperCase} updated{entityNameUpperCase}) =>\n" +
                                $"            await _{entityNameLowerCase}sCollection.ReplaceOneAsync(x => x.Id == id, updated{entityNameUpperCase});\n" +
                                $"\n" +
                                $"        public async Task RemoveAsync(string id) =>\n" +
                                $"            await _{entityNameLowerCase}sCollection.DeleteOneAsync(x => x.Id == id);\n" +
                                $"    }}\n" +
                                $"}}";

        string path = @$"C:/AARP/aarpbe/BookStoreApi/Services/{entity}Service.cs";

        try
        {
            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(serviceFileContent);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"Mensaje de error\n{e}");
        throw;
    }
}



Console.WriteLine("Comienza creación de archivos Controllers:");
foreach (string entity in entitiesList)
{
    try
    {
        Console.WriteLine($"Creación de archivo {entity}Controllers.");
        entityNameLowerCase = entity[0].ToString().ToLower() + entity.Substring(1);
        entityNameUpperCase = entity;
        string controllerFileContent = $"using AARP_BE.Models;\n" +
                                    $"using AARP_BE.Services;\n" +
                                    $"using Microsoft.AspNetCore.Mvc;\n" +
                                    $"\n" +
                                    $"\n" +
                                    $"namespace AARP_BE.Controllers\n" +
                                    $"{{\n" +
                                    $"    [ApiController]\n" +
                                    $"    [Route(\"api/[controller]\")]\n" +
                                    $"    public class {entityNameUpperCase}Controller : ControllerBase\n" +
                                    $"    {{\n" +
                                    $"        private readonly {entityNameUpperCase}Service _services{entityNameUpperCase};\n" +
                                    $"\n" +
                                    $"        public {entityNameUpperCase}Controller({entityNameUpperCase}Service {entityNameLowerCase}Service) =>\n" +
                                    $"            _services{entityNameUpperCase} = {entityNameLowerCase}Service;\n" +
                                    $"\n" +
                                    $"        [HttpGet]\n" +
                                    $"        public async Task<List<{entityNameUpperCase}>> Get() =>\n" +
                                    $"            await _services{entityNameUpperCase}.GetAsync();\n" +
                                    $"\n" +
                                    $"        [HttpGet(\"{{ id: length(24)}}\")]\n" +
                                    $"        public async Task<ActionResult<{entityNameUpperCase}>> Get(string id)\n" +
                                    $"        {{\n" +
                                    $"            var {entityNameLowerCase} = await _services{entityNameUpperCase}.GetAsync(id);\n" +
                                    $"\n" +
                                    $"            if ({entityNameLowerCase} is null)\n" +
                                    $"            {{\n" +
                                    $"                return NotFound();\n" +
                                    $"            }}\n" +
                                    $"\n" +
                                    $"            return {entityNameLowerCase};\n" +
                                    $"        }}\n" +
                                    $"\n" +
                                    $"        [HttpPost]\n" +
                                    $"        public async Task<IActionResult> Post({entityNameUpperCase} new{entityNameUpperCase})\n" +
                                    $"        {{\n" +
                                    $"            await _services{entityNameUpperCase}.CreateAsync(new{entityNameUpperCase});\n" +
                                    $"\n" +
                                    $"            return CreatedAtAction(nameof(Get), new {{ id = new{entityNameUpperCase}.Id }}, new{entityNameUpperCase});\n" +
                                    $"        }}\n" +
                                    $"\n" +
                                    $"        [HttpPut(\"{{ id: length(24) }}\")]\n" +
                                    $"        public async Task<IActionResult> Update(string id, {entityNameUpperCase} updated{entityNameUpperCase})\n" +
                                    $"        {{\n" +
                                    $"            var {entityNameLowerCase} = await _services{entityNameUpperCase}.GetAsync(id);\n" +
                                    $"\n" +
                                    $"            if ({entityNameLowerCase} is null)\n" +
                                    $"            {{\n" +
                                    $"                return NotFound();\n" +
                                    $"            }}\n" +
                                    $"\n" +
                                    $"            updated{entityNameUpperCase}.Id = {entityNameLowerCase}.Id;\n" +
                                    $"\n" +
                                    $"            await _services{entityNameUpperCase}.UpdateAsync(id, updated{entityNameUpperCase});\n" +
                                    $"\n" +
                                    $"            return NoContent();\n" +
                                    $"        }}\n" +
                                    $"\n" +
                                    $"        [HttpDelete(\"{{id:length(24)}}\")]\n" +
                                    $"        public async Task<IActionResult> Delete(string id)\n" +
                                    $"        {{\n" +
                                    $"            var {entityNameLowerCase} = await _services{entityNameUpperCase}.GetAsync(id);\n" +
                                    $"\n" +
                                    $"            if ({entityNameLowerCase} is null)\n" +
                                    $"            {{\n" +
                                    $"                return NotFound();\n" +
                                    $"            }}\n" +
                                    $"\n" +
                                    $"            await _services{entityNameUpperCase}.RemoveAsync(id);\n" +
                                    $"\n" +
                                    $"            return NoContent();\n" +
                                    $"        }}\n" +
                                    $"    }}\n" +
                                    $"}}";
        string path = @$"C:/AARP/aarpbe/BookStoreApi/Controllers/{entity}Controller.cs";
        try
        {
            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(controllerFileContent);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"Mensaje de error\n{e}");
        throw;
    }
}