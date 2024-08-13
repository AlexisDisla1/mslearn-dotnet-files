using System.IO;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Newtonsoft.Json;
using System.Data;

var CurrentDirectory = Directory.GetCurrentDirectory();
var StoresDirectory = Path.Combine(CurrentDirectory,"stores");

var salesTotalDir = Path.Combine(CurrentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(StoresDirectory);

var salestotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salestotal}{Environment.NewLine}");

//-------
IEnumerable<string> FindFiles (string Ncarpeta)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(Ncarpeta,"*", SearchOption.AllDirectories);

    foreach(var archi in foundFiles)
    {
        var extension = Path.GetExtension(archi);

        if(extension == ".json")
        {
           salesFiles.Add(archi);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    //Loop sobre la ruta salesFiles
    foreach(var file in salesFiles)
    {
        //Leemos el contenido de los archivos
        string salesJson = File.ReadAllText(file);

        //Unimos los contenidos como un Json
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        //Agregamos la cantidad encontrada en el campo total a la variables salesTotal
        salesTotal += data?.total ?? 0;
    }

    return salesTotal;
}
record SalesData (double total);