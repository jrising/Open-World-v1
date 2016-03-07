using System;
namespace OpenWorldModel
{
	public class SimpleTests
	{
		public SimpleTests()
		{
			/* TODO: Get these working:
			TimeSeries precipitation = new TimeSeries(DataReader.Load("precip.tsv", Dims.mm / Dims.day), new DateTime(1901, 1, 1));
			TimeSeries inletFlow = new TimeSeries(DataReader.Load("bhakra.tsv", Dims.ft^3 / Dims.s), new DateTime(1963, 1, 1));
			
			Estimate estimate = Estimate.Model(inletFlow, MovingAverage(precipitation, new RandomVector("coeffs", 7)) + TimeSeries.WhiteNoise(RandomVariable("sigma_e")));
			Console.Out.WriteLine("Coefficients: " + estimate.get("coeffs"));
			
			estimate.Predict(TimeSpan.FromDays(30));
			*/
		}
	}
}