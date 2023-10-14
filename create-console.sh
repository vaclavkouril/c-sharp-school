#!/bin/bash
set -ueo pipefail

name=$1

template="""using System;

namespace $name
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("$name created!");
        }
    }
}
"""

dotnet new console -n $name

echo $template > ./$name/Program.cs
