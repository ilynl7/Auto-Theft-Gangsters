using System;
using UnityEngine;

public static class vp_TimeUtility
{
	public struct Units
	{
		public int hours;

		public int minutes;

		public int seconds;

		public int deciSeconds;

		public int centiSeconds;

		public int milliSeconds;
	}

	public static Units TimeToUnits(float timeInSeconds)
	{
		Units result = default(Units);
		result.hours = (int)timeInSeconds / 3600;
		result.minutes = ((int)timeInSeconds - result.hours * 3600) / 60;
		result.seconds = (int)timeInSeconds % 60;
		result.deciSeconds = (int)((timeInSeconds - (float)result.seconds) * 10f) % 60;
		result.centiSeconds = (int)((timeInSeconds - (float)result.seconds) * 100f % 600f);
		result.milliSeconds = (int)((timeInSeconds - (float)result.seconds) * 1000f % 6000f);
		return result;
	}

	public static float UnitsToSeconds(Units units)
	{
		float num = 0f;
		num += (float)(units.hours * 3600);
		num += (float)(units.minutes * 60);
		num += (float)units.seconds;
		num += (float)units.deciSeconds * 0.1f;
		num += (float)(units.centiSeconds / 100);
		return num + (float)(units.milliSeconds / 1000);
	}

	public static string TimeToString(float timeInSeconds, bool showHours, bool showMinutes, bool showSeconds, bool showTenths, bool showHundredths, bool showMilliSeconds, char delimiter = ':')
	{
		Units units = TimeToUnits(timeInSeconds);
		string text = ((units.hours >= 10) ? units.hours.ToString() : ("0" + units.hours));
		string text2 = ((units.minutes >= 10) ? units.minutes.ToString() : ("0" + units.minutes));
		string text3 = ((units.seconds >= 10) ? units.seconds.ToString() : ("0" + units.seconds));
		string text4 = units.deciSeconds.ToString();
		string text5 = ((units.centiSeconds >= 10) ? units.centiSeconds.ToString() : ("0" + units.centiSeconds));
		string text6 = ((units.milliSeconds >= 100) ? units.milliSeconds.ToString() : ("0" + units.milliSeconds));
		text6 = ((units.milliSeconds >= 10) ? text6 : ("0" + text6));
		return (((!showHours) ? string.Empty : text) + ((!showMinutes) ? string.Empty : (delimiter + text2)) + ((!showSeconds) ? string.Empty : (delimiter + text3)) + ((!showTenths) ? string.Empty : (delimiter + text4)) + ((!showHundredths) ? string.Empty : (delimiter + text5)) + ((!showMilliSeconds) ? string.Empty : (delimiter + text6))).TrimStart(delimiter);
	}

	public static string SystemTimeToString(DateTime systemTime, bool showHours, bool showMinutes, bool showSeconds, bool showTenths, bool showHundredths, bool showMilliSeconds, char delimiter = ':')
	{
		return TimeToString(SystemTimeToSeconds(systemTime), showHours, showMinutes, showSeconds, showTenths, showHundredths, showMilliSeconds, delimiter);
	}

	public static string SystemTimeToString(bool showHours, bool showMinutes, bool showSeconds, bool showTenths, bool showHundredths, bool showMilliSeconds, char delimiter = ':')
	{
		return SystemTimeToString(DateTime.Now, showHours, showMinutes, showSeconds, showTenths, showHundredths, showMilliSeconds, delimiter);
	}

	public static Units SystemTimeToUnits(DateTime systemTime)
	{
		Units result = default(Units);
		result.hours = systemTime.Hour;
		result.minutes = systemTime.Minute;
		result.seconds = systemTime.Second;
		result.deciSeconds = (int)((float)systemTime.Millisecond / 100f);
		result.centiSeconds = systemTime.Millisecond / 10;
		result.milliSeconds = systemTime.Millisecond;
		return result;
	}

	public static Units SystemTimeToUnits()
	{
		return SystemTimeToUnits(DateTime.Now);
	}

	public static float SystemTimeToSeconds(DateTime systemTime)
	{
		return UnitsToSeconds(SystemTimeToUnits(systemTime));
	}

	public static float SystemTimeToSeconds()
	{
		return SystemTimeToSeconds(DateTime.Now);
	}

	public static float TimeToDegrees(float seconds, bool includeHours = false, bool includeMinutes = false, bool includeSeconds = true, bool includeMilliSeconds = true)
	{
		Units units = TimeToUnits(seconds);
		if (includeHours && includeMinutes && includeSeconds)
		{
			return HoursToDegreesInternal(units.hours, units.minutes, units.seconds);
		}
		if (includeHours && includeMinutes)
		{
			return HoursToDegreesInternal(units.hours, units.minutes);
		}
		if (includeMinutes && includeSeconds && includeMilliSeconds)
		{
			return MinutesToDegreesInternal(units.minutes, units.seconds, units.milliSeconds);
		}
		if (includeMinutes && includeSeconds)
		{
			return MinutesToDegreesInternal(units.minutes, units.seconds);
		}
		if (includeSeconds && includeMilliSeconds)
		{
			return SecondsToDegreesInternal(units.seconds, units.milliSeconds);
		}
		if (includeHours)
		{
			return HoursToDegreesInternal(units.hours);
		}
		if (includeMinutes)
		{
			return MinutesToDegreesInternal(units.minutes);
		}
		if (includeSeconds)
		{
			return TimeToDegrees(units.seconds);
		}
		if (includeMilliSeconds)
		{
			return MilliSecondsToDegreesInternal(units.milliSeconds);
		}
		Debug.LogError("Error: (vp_TimeUtility.TimeToDegrees) This combination of time units is not supported.");
		return 0f;
	}

	public static Vector3 SystemTimeToDegrees(DateTime time, bool smooth = true)
	{
		return new Vector3(HoursToDegreesInternal(time.Hour, (!smooth) ? 0f : ((float)time.Minute), (!smooth) ? 0f : ((float)time.Second)), MinutesToDegreesInternal(time.Minute, (!smooth) ? 0f : ((float)time.Second), (!smooth) ? 0f : ((float)time.Millisecond)), SecondsToDegreesInternal(time.Second, (!smooth) ? 0f : ((float)time.Millisecond)));
	}

	public static Vector3 SystemTimeToDegrees(bool smooth = true)
	{
		return SystemTimeToDegrees(DateTime.Now, smooth);
	}

	private static float HoursToDegreesInternal(float hours, float minutes = 0f, float seconds = 0f)
	{
		return hours * 30f + minutes * 0.5f + seconds * 0.008333333f;
	}

	private static float MinutesToDegreesInternal(float minutes, float seconds = 0f, float milliSeconds = 0f)
	{
		return minutes * 6f + seconds * 0.1f + milliSeconds * 0.0001f;
	}

	private static float SecondsToDegreesInternal(float seconds, float milliSeconds = 0f)
	{
		return seconds * 6f + milliSeconds * 0.006f;
	}

	private static float MilliSecondsToDegreesInternal(float milliSeconds)
	{
		return milliSeconds * 0.36f;
	}
}
