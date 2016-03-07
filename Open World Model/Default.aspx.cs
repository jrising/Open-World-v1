
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace OpenWorldModel
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e) {
			/*double[,] result = SOCPovertyTraps.Run();
			resultDiv.InnerHtml = "<table><tr><th>K / AL</th><th>(1 - s) Y / L</th><th>L</th><th>Collapses</th></tr>";
			for (int ii = 0; ii < 1000; ii++)
				resultDiv.InnerHtml += "<tr><td>" + result[ii, 0] + "</td><td>" + result[ii, 1] + "</td><td>" + result[ii, 2] + "</td><td>" + result[ii, 3] + "</td></tr>";
			resultDiv.InnerHtml += "</table>";*/

			/*double[,] result = SolowModel.Run();
			resultDiv.InnerHtml = "<table><tr><th>K / AL</th><th>(1 - s) Y / L</th><th>L</th></tr>";
			for (int ii = 0; ii < 100; ii++)
				resultDiv.InnerHtml += "<tr><td>" + result[ii, 0] + "</td><td>" + result[ii, 1] + "</td><td>" + result[ii, 2] + "</td></tr>";
			resultDiv.InnerHtml += "</table>";*/
			
			/*CollectCountry collector = new CollectCountry("Peru");
			collector.AddDirectory("/Users/jrising/projects/Open World Model/indicators/indicators");
			collector.NormalizeYearChanges();
			resultDiv.InnerHtml = collector.HtmlDisplay();*/
			
			/*IndicatorDevelopmentScaling scaling = new IndicatorDevelopmentScaling();
			scaling.AddDirectory("/Users/jrising/projects/Open World Model/indicators/indicators");
			scaling.NormalizeYearChanges();
			Dictionary<string, List<double>[]> byIndicator = scaling.GetHDIBins();
			double[,] points = scaling.ValuesToPDF(byIndicator["CO2 emissions (kg per 2005 PPP $ of GDP)"][5]);
			resultDiv.InnerHtml = "<table><tr><th>X</th><th>Y</th></tr>";
			for (int ii = 0; ii < points.GetLength(0); ii++) {
				try {
					resultDiv.InnerHtml += "<tr><td>" + points[ii, 0] + "</td><td>" + points[ii, 1] + "</td></tr>";
				} catch (Exception ex) {
					throw ex;
				}
			}
			resultDiv.InnerHtml += "</table>";*/

			IndicatorDevelopmentScaling scaling = new IndicatorDevelopmentScaling();
			scaling.AddDirectory("/Users/jrising/projects/Open World Model/indicators/indicators");
			scaling.NormalizeYearChanges();
			Dictionary<string, List<double>[]> byIndicator = scaling.GetHDIBins();
			resultDiv.InnerHtml = "<table><tr><th>Indicator</th><th>0</th><th>1</th><th>2</th><th>3</th><th>4</th><th>5</th><th>6</th><th>7</th><th>8</th><th>9</th></tr>";
			foreach (KeyValuePair<string, List<double>[]> indicatorvp in byIndicator) {
				resultDiv.InnerHtml += "<tr><td>" + indicatorvp.Key + "</td>";
				double[] bins = scaling.GetPowerLawRSqr(indicatorvp.Value);
				for (int bin = 0; bin < 10; bin++)
					resultDiv.InnerHtml += "<td bgcolor=\"" + ColorMap(bins[bin], indicatorvp.Value[bin].Count) + "\">" + bins[bin].ToString("0.00") + " (" + indicatorvp.Value[bin].Count + ")</td>";
				resultDiv.InnerHtml += "</tr>";					
			}
			resultDiv.InnerHtml += "</table>";
		}

		public virtual void button1Clicked (object sender, EventArgs args)
		{
			button1.Text = "You clicked me";
		}
			
		public string ColorMap(double value, double count) {
			if (double.IsNaN(value) || count < 3)
				return "#FFFFFF";
			
			double[,] rgb = new double[43, 3] {{    1.0000,         0,         0},
{    1.0000,    0.0938,         0},
{    1.0000,    0.1875,         0},
{    1.0000,    0.2812,         0},
{    1.0000,    0.3750,         0},
{    1.0000,    0.4688,         0},
{    1.0000,    0.5625,         0},
{    1.0000,    0.6562,         0},
{    1.0000,    0.7500,         0},
{    1.0000,    0.8438,         0},
{    1.0000,    0.9375,         0},
{    0.9688,    1.0000,         0},
{    0.8750,    1.0000,         0},
{    0.7812,    1.0000,         0},
{    0.6875,    1.0000,         0},
{    0.5938,    1.0000,         0},
{    0.5000,    1.0000,         0},
{    0.4062,    1.0000,         0},
{    0.3125,    1.0000,         0},
{    0.2188,    1.0000,         0},
{    0.1250,    1.0000,         0},
{    0.0312,    1.0000,         0},
{         0,    1.0000,    0.0625},
{         0,    1.0000,    0.1562},
{         0,    1.0000,    0.2500},
{         0,    1.0000,    0.3438},
{         0,    1.0000,    0.4375},
{         0,    1.0000,    0.5312},
{         0,    1.0000,    0.6250},
{         0,    1.0000,    0.7188},
{         0,    1.0000,    0.8125},
{         0,    1.0000,    0.9062},
{         0,    1.0000,    1.0000},
{         0,    0.9062,    1.0000},
{         0,    0.8125,    1.0000},
{         0,    0.7188,    1.0000},
{         0,    0.6250,    1.0000},
{         0,    0.5312,    1.0000},
{         0,    0.4375,    1.0000},
{         0,    0.3438,    1.0000},
{         0,    0.2500,    1.0000},
{         0,    0.1562,    1.0000},
{         0,    0.0625,    1.0000}};

			int row = (int) (value * 43);
			if (row == 43)
				return "#0000FF";
			StringBuilder sb = new StringBuilder();
			sb.Append("#");
			double whitewash = 1.0 / Math.Sqrt(count - 2);
			for (int ii = 0; ii < 3; ii++)
				sb.AppendFormat("{0:x2}", (byte) (255 * (rgb[row, ii] + whitewash) / (1 + whitewash)));
				
			return sb.ToString();
		}
	}
}
