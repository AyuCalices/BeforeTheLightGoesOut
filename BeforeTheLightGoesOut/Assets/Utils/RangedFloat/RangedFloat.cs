using System;

namespace Utils.RangedFloat
{
	[Serializable]
	public struct RangedFloat
	{
		public float minValue;
		public float maxValue;

		public RangedFloat(float minValue, float maxValue)
		{
			this.minValue = minValue;
			this.maxValue = maxValue;
		}
	}
}