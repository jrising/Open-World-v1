using System;
using System.Collections.Generic;

namespace OpenWorldModel
{
	public class HistoricalGeography
	{
		public List<IHistoricalGeographicMap> maps;
		public List<KeyValuePair<SpaceTime, IProperty>> properties;

		public HistoricalGeography()
		{
			maps = new List<IHistoricalGeographicMap>();
			properties = new List<KeyValuePair<SpaceTime, IProperty>>();
		}
		
		public void addMap(IHistoricalGeographicMap map) {
			maps.Add(map);
		}
		public void addProperty(SpaceTime st, IProperty property) {
			properties.Add(new KeyValuePair<SpaceTime, IProperty>(st, property));
		}
		
		public IEnumerable<IProperty> getProperties(SpaceTime st) {
			Dictionary<string, IProperty> result = new Dictionary<string, IProperty>();
			foreach (IHistoricalGeographicMap map in maps) {
				Measurement value = map.get(st);
				if (value != null)
					result.Add(map.Name, new NamedProperty(map.Name, value, null));
			}
			
			foreach (KeyValuePair<SpaceTime, IProperty> kvp in properties) {
				double overlap = st.Overlap(kvp.Key);
				if (overlap == 0)
					continue;
				
				if (result.ContainsKey(kvp.Value.Name))
					result[kvp.Value.Name] = result[kvp.Value.Name].Merge(kvp.Value, overlap);
				else
					result[kvp.Value.Name] = kvp.Value.Clone(overlap);
			}
			
			return result.Values;
		}
	}
}

