using System;
namespace OpenWorldModel
{
	public class SpaceTime
	{
		protected DateTime time;
		protected TimeSpan duration;
		protected double longitude;
		protected double eastward;
		protected double latitude;
		protected double northward;
		
		public SpaceTime(DateTime time, TimeSpan duration, double longitude, double eastward, double latitude, double northward)
		{
			this.time = time;
			this.duration = duration;
			this.longitude = longitude;
			this.eastward = eastward;
			this.latitude = latitude;
			this.northward = northward;
		}
		
		public DateTime Time {
			get {
				return time;
			}
			set {
				time = value;
			}
		}
		
		public double Longitude {
			get {
				return longitude;
			}
			set {
				if (longitude > 180 || longitude < -180)
					throw new ArgumentOutOfRangeException();
				longitude = value;
			}
		}
		
		public double Latitude {
			get {
				return latitude;
			}
			set {
				if (latitude > 90 || latitude < -90)
					throw new ArgumentOutOfRangeException();
				latitude = value;
			}
		}
		
		public double Overlap(SpaceTime st) {
			if (longitude + eastward <= st.longitude || longitude >= st.longitude + st.eastward)
				return 0;
			if (latitude + northward <= st.latitude || latitude >= st.latitude + st.northward)
				return 0;
			if (time.Add(duration).CompareTo(st.time) <= 0 || st.time.Add(st.duration).CompareTo(time) >= 0)
				return 0;
			
			double eastwest = Math.Min(Math.Min(eastward, st.eastward),
			                           Math.Min(longitude + eastward - st.longitude, st.longitude + st.eastward - longitude));
			double northsouth = Math.Min(Math.Min(northward, st.northward),
			                             Math.Min(latitude + northward - st.latitude, st.latitude + st.northward - latitude));
			double seconds = Math.Min(Math.Min(duration.TotalSeconds, st.duration.TotalSeconds),
			                          Math.Min(time.Add(duration).Subtract(st.time).TotalSeconds, st.time.Add(st.duration).Subtract(time).TotalSeconds));
			return (eastwest / eastward) * (northsouth / northward) * (seconds / duration.TotalSeconds);
		}
	}
}

