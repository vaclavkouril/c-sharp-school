using System;

namespace OHledaniBugu {
	class Program {
		static void Main(string[] args) {
			try {
				var line = Console.ReadLine();
				if (line == null) {
					Console.WriteLine("Error!");
					return;
				}

				var a = Convert.ToInt64(line);
				if (a < 0) {
					Console.WriteLine("Error!");
					return;
				}

				line = Console.ReadLine();
				if (line == null) {
					Console.WriteLine("Error!");
					return;
				}
				long b = Convert.ToInt64(line);				if (b < 0) {
					Console.WriteLine("Error!");
					return;
				}
				long result;
				if (a > b)
					result = a - b;
				else result = b - a;
				Console.WriteLine("Result: {0}", result);
			} catch (FormatException) {
				Console.WriteLine("Error!");
			}
		}
	}
}

