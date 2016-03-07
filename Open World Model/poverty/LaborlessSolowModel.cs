using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class SolowModel
	{
		public SolowModel()
		{
		}

		public static double[,] Run() {
			Stock capital = new Stock("Capital (K)", 1000, GlobalDimensions.get("k"));
			Stock technology = new Stock("Technology (A)", 4, GlobalDimensions.get("a"));
			RandomVariable shock = new RandomVariable("Shock (epsilon)", .9, 1.1, Dimensionless.Instance);

			TemporalVariable production = new Function("Production (Y)", x => x[0] * Math.Pow(x[1], x[2]) * x[3],
			                                           GlobalDimensions.get("k"), technology, capital, new Constant("Preference (alpha)", .3, Dimensionless.Instance), shock);
			Constant savingsRate = new Constant("Savings Rate (s)", .1, GlobalDimensions.Time.RaisedTo(-1));
			capital.IncreasesBy(production * savingsRate
			                		- (capital * (new Constant("Depreciation Rate (delta)", .1, GlobalDimensions.Time.RaisedTo(-1)))));

			double[,] result = new double[100, 2];
			for (int tt = 0; tt < 100; tt++) {
				result[tt, 0] = capital.Evaluate(tt);
				result[tt, 1] = (1 - savingsRate.Evaluate(tt)) * production.Evaluate(tt);
			}

			return result;
		}
	}
}

