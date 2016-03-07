using System;
namespace OpenWorldModel
{
	public interface IProperty
	{
		string Name { get; }
		Measurement Value { get; }
		ISystem Context { get; }
		
		IProperty Merge(IProperty prop, double overlap);
		IProperty Clone(double confidence);
	}
}

