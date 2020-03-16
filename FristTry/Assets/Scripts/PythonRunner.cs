using System.Collections.Generic;
using UnityEngine;
using IronPython.Hosting;
using System.IO;
using System.Text;
using System;

public class PythonRunner : MonoBehaviour
{
    public void Start()
    {
        var engine = Python.CreateEngine();

        var libs = new[]{Application.dataPath + "/Plugins/Lib"};
        engine.SetSearchPaths(libs);

        var script = Application.dataPath + "/Python/converter.py";
        var source = engine.CreateScriptSourceFromFile(script);

        var argv = new List<string>();
        argv.Add("");
        argv.Add("/Users/Oceswil83/Desktop/p1/");//the dir
        argv.Add("p1"); //projectName

        engine.GetSysModule().SetVariable("argv", argv);

        var eIO = engine.Runtime.IO;

        var errors = new MemoryStream();
        eIO.SetErrorOutput(errors, Encoding.Default);

        var results = new MemoryStream();
        eIO.SetOutput(results, Encoding.Default);

        var scope = engine.CreateScope();
 
        source.Execute(scope);

        string str(byte[] x) => Encoding.Default.GetString(x);
        Console.WriteLine("ERRORS:");
        Console.WriteLine(str(errors.ToArray()));
        Console.WriteLine();
        Console.WriteLine(str(results.ToArray()));
    }
}
