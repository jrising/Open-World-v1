using System;
using System.IO;
using System.Text;

namespace OpenWorldModel
{
	public class Importer
	{
		protected TextReader reader;
		
		public Importer(string filepath)
		{
			this.reader = new StreamReader(filepath);
		}
		
		public Variable ReadVariable() {
			while (true) {
				string line = reader.ReadLine();
				if (line.Trim() == "" || line == "{UTF-8}" || line.StartsWith("*") || line.StartsWith("\t"))
					continue;
				
				// Extract the name
				if (!line.EndsWith("="))
					throw new FormatException("Variable definition doesn't start with <NAME>=");
				string name = line.Substring(0, line.Length - 1);
				if (name.StartsWith("\"")) {
					if (!name.EndsWith("\""))
						throw new FormatException("Quoted variable name is more than one line");
					name = name.Substring(1, name.Length - 1);
				}
				
				// Extract the equation
				VensimEquation equation = ReadEquation(reader);
				
				// Extract the units
				line = reader.ReadLine();
				if (!line.StartsWith("\t~\t"))
					throw new FormatException("Unknown prelude ot the units");
				IDimension dims = ReadDimensions(line.Substring(3));
				
				// Extract the comment
				StringBuilder description = new StringBuilder();
				do {
					line = reader.ReadLine();
					if ((line.StartsWith("\t~\t") && description.Length == 0) ||
					    (line.StartsWith("\t\t") && description.Length > 0)) {
						if (line.EndsWith("|")) {
							description.Append(line.Substring(3, line.Length - 4));
							break;
						} else
							description.Append(line.Substring(3));
					} else
						throw new FormatException("Expected a line of description");
				} while (true);
				
				return new VensimVariable(name, description, units, equation);
			}
		}
	}
}

