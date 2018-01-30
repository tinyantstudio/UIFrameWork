//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2017 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;

public class MinMaxRangeAttribute : PropertyAttribute
{
	public float minLimit, maxLimit;

	public MinMaxRangeAttribute (float minLimit, float maxLimit)
	{
		this.minLimit = minLimit;
		this.maxLimit = maxLimit;
	}
}
