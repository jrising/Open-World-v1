using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class Measurement
	{
		public double value;
		public double confidence;
		public IDimensions dimensions;

		public Measurement(double value, double confidence, IDimensions dimensions)
		{
			this.value = value;
			this.confidence = confidence;
			this.dimensions = dimensions;
		}
		
		public Measurement Merge(Measurement m, double overlap) {
			if (dimensions.Equals(m.dimensions))
				return new Measurement((confidence * value + overlap * m.confidence * m.value) / (confidence + overlap * m.confidence),
				                       1 - (1 - confidence) * (1 - overlap * m.confidence), dimensions);
			throw new ArgumentException();
		}
		
		public double Value {
			get {
				return value;
			}
			set {
				this.value = value;
			}
		}
		
		public double Confidence {
			get {
				return confidence;
			}
			set {
				confidence = value;
			}
		}
		
		public IDimensions Dimensions {
			get {
				return dimensions;
			}
		}
	}
}

