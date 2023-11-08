#!/bin/bash
set -ueo pipefail

name=$1

template=$"""using System; \n
\n
namespace $name\n
{\n
\tinternal class Program\n
\t{\n
\t\tstatic void Main(string[] args)\n
\t\t{\n
\t\t\tConsole.WriteLine(\"Created!\");\n
\t\t}\n
\t}\n
}\n
"""

dotnet new console -n $name

echo -e $template  > ./$name/Program.cs
