using System;
namespace OpenWorldModel
{
	public class NamedProperty : IProperty
	{
		protected string name;
		protected Measurement value;
		protected ISystem context;
		
		public NamedProperty(string name, Measurement value, ISystem context)
		{
			this.name = name;
			this.value = value;
			this.context = context;
		}
		
		public string Name {
			get {
				return name;
			}
		}
		
		public Measurement Value {
			get {
				return value;
			}
		}
		
		public ISystem Context {
			get {
				return context;
			}
		}
		
		public IProperty Merge(IProperty prop, double overlap) {
			return new NamedProperty(name, value.Merge(prop.Value, overlap), new AmbiguousSystem(context, prop.Context, overlap));
		}
		
		public IProperty Clone(double confidence) {
			Measurement clone = new Measurement(value.value, value.confidence * confidence, value.dimensions);
			return new NamedProperty(name, clone, context);
		}
	}
}

