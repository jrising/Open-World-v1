using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class DataReader
	{
		protected string filename;
		protected TextFieldParser parser;
		
		public DataReader(string filename)
		{
			this.filename = filename;
			
			if (filename.EndsWith("tsv") || filename.EndsWith("csv")) {
				parser = new TextFieldParser(new HandleQuotesStream(filename, "\"", ",\t", '-'));
				parser.TextFieldType = FieldType.Delimited;
				//parser.HasFieldsEnclosedInQuotes = true; // doesn't work!
				if (filename.EndsWith("tsv"))
					parser.SetDelimiters("\t");
				else
					parser.Delimiters = new string[] {","}; //parser.SetDelimiters(",");
			} else
				throw new ArgumentException("Unknown file type");
		}
		
		public KeyValuePair<string, double[]>? ReadLabeledRow() {
			if (parser.EndOfData)
				return null;
			
			string[] row = parser.ReadFields();
			if (row == null || row.Length == 0)
				return null;
			string label = row[0];
			double[] values = new double[row.Length - 1];
			for (int ii = 0; ii < row.Length - 1; ii++) {
				if (row[ii + 1].Length == 0)
					values[ii] = double.NaN;
				else {
					if (!double.TryParse(row[ii + 1], out values[ii]))
						values[ii] = double.NaN;
				}
			}
			
			return new KeyValuePair<string, double[]>(label, values);
		}
		
		public double[] FindLabeledRow(string label) {
			while (!parser.EndOfData) {
				KeyValuePair<string, double[]>? row = ReadLabeledRow();
				if (row.HasValue && row.Value.Key == label)
					return row.Value.Value;
			}
			
			return null;
		}
		
		public DataMatrix LoadMatrix(IDimensions dims) {
			List<string[]> rows = new List<string[]>();
			
			while (!parser.EndOfData)
				rows.Add(parser.ReadFields());
			
			double[,] values = new double[rows.Count, rows[0].Length];
			for (int ii = 0; ii < rows.Count; ii++)
				for (int jj = 1; jj < rows[ii].Length; jj++)
					values[ii, jj] = double.Parse(rows[ii][jj]);
			
			return new DataMatrix(filename.Substring(0, filename.Length - 4), dims, values);
		}
	}
}

