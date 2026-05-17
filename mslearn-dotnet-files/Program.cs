using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotal");
Directory.CreateDirectory(salesTotalDir);   
var reportOutput = Path.Combine(salesTotalDir, "summaryReport.txt"); 

var salesFiles = FindFiles(storesDirectory);

GenerateAndSaveSalesSummary(salesFiles, reportOutput); 

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();
    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        if (file.EndsWith("sales.json"))
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

void GenerateAndSaveSalesSummary(IEnumerable<string> salesFiles, string outputFilePath)
{
    double salesTotal = 0;
    string details = "";
    
    foreach (var file in salesFiles)
    {      
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonSerializer.Deserialize<SalesData>(salesJson);
    
        if (data != null)
        {
            salesTotal += data.Total;
            string fileName = Path.GetFileName(file);
            details += $"{fileName}: {data.Total:C}\n";
        }
    }
    
    string reportHeader = "Sales Summary\n----------------------------\n";
    string reportTotal = $"Total Sales: {salesTotal:C}\n\n";
    string reportDetails = "Details:\n" + details;

    string finalReport = reportHeader + reportTotal + reportDetails;

    File.WriteAllText(outputFilePath, finalReport);
}

public class SalesData 
{
    public double Total { get; set; }
}