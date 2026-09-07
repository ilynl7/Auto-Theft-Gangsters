using System;
using System.Collections.Generic;
using System.Text;

public class TimeTools
{
	private const string strMinFormate = "{0:D2}:{1:D2}";

	private const string strformate = "{0:D2}:{1:D2}:{2:D2}";

	private static StringBuilder sb = new StringBuilder(512);

	public static long SECONDS_TO_TICKS = 10000000L;

	public static long DAY_SECOND = 86400L;

	public static TimeSpan GetLocalShowTime(long startTime, long offsetTime)
	{
		TimeSpan timeSpan = new TimeSpan(startTime * SECONDS_TO_TICKS);
		TimeSpan timeSpan2 = new TimeSpan(offsetTime * SECONDS_TO_TICKS * 3600);
		timeSpan += timeSpan2;
		return new TimeSpan((864000000000L + (timeSpan + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now)).Ticks) % 864000000000L);
	}

	public static TimeSpan GetLocalShowTime(long[] startTime, long offsetTime)
	{
		TimeSpan timeSpan = new TimeSpan(startTime[0] * SECONDS_TO_TICKS);
		TimeSpan timeSpan2 = new TimeSpan(offsetTime * SECONDS_TO_TICKS * 3600);
		timeSpan += timeSpan2;
		return new TimeSpan((864000000000L + (timeSpan + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now)).Ticks) % 864000000000L);
	}

	public static int GetOffsetDay(long startTime, long offsetTime)
	{
		TimeSpan timeSpan = new TimeSpan(startTime * SECONDS_TO_TICKS);
		TimeSpan timeSpan2 = new TimeSpan(offsetTime * SECONDS_TO_TICKS * 3600);
		timeSpan += timeSpan2;
		timeSpan += TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
		if (timeSpan.Ticks > 0)
		{
			return (int)(timeSpan.Ticks / 864000000000L);
		}
		return -1 + (int)(timeSpan.Ticks / 864000000000L);
	}

	public static List<TimeSpan> GetLocalShowTime(long[] startTime, long offsetTime, long durationtime, int next, bool isshownext = false)
	{
		if (startTime == null || startTime.Length == 0)
		{
			return null;
		}
		List<TimeSpan> list = new List<TimeSpan>();
		TimeSpan timeSpan = new TimeSpan(offsetTime * SECONDS_TO_TICKS * 3600);
		int num = 0;
		if (next > 0)
		{
			num = next - 1;
		}
		if (isshownext)
		{
			num = (num + 1) % startTime.Length;
		}
		TimeSpan timeSpan2 = new TimeSpan(startTime[num] * SECONDS_TO_TICKS);
		timeSpan2 += timeSpan;
		timeSpan2 = new TimeSpan((864000000000L + (timeSpan2 + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now)).Ticks) % 864000000000L);
		list.Add(timeSpan2);
		timeSpan2 = new TimeSpan((startTime[num] + durationtime) * SECONDS_TO_TICKS);
		timeSpan2 += timeSpan;
		timeSpan2 = new TimeSpan((864000000000L + (timeSpan2 + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now)).Ticks) % 864000000000L);
		list.Add(timeSpan2);
		return list;
	}

	public static TimeSpan GetShopItemTime(int[] endtimes)
	{
		if (endtimes == null || endtimes.Length != 6)
		{
			return new TimeSpan(0, 0, 0);
		}
		long num = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.TimeOffset * 3600;
		long curServerTime = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime();
		return new TimeSpan(new DateTime(endtimes[0], endtimes[1], endtimes[2], endtimes[3], endtimes[4], endtimes[5]).Subtract(new DateTime(1970, 1, 1)).Ticks - curServerTime * SECONDS_TO_TICKS + num * SECONDS_TO_TICKS);
	}

	public static bool IsTimeRange(int[] starttimes, int[] endtimes)
	{
		if (endtimes == null || endtimes.Length != 6 || starttimes == null || starttimes.Length != 6)
		{
			return false;
		}
		DateTime dateTime = new DateTime(starttimes[0], starttimes[1], starttimes[2], starttimes[3], starttimes[4], starttimes[5]);
		DateTime dateTime2 = new DateTime(endtimes[0], endtimes[1], endtimes[2], endtimes[3], endtimes[4], endtimes[5]);
		TimeSpan timeSpan = dateTime.Subtract(new DateTime(1970, 1, 1));
		TimeSpan timeSpan2 = dateTime2.Subtract(new DateTime(1970, 1, 1));
		long num = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.TimeOffset * 3600;
		long num2 = (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime() - num) * SECONDS_TO_TICKS;
		if (timeSpan.Ticks <= num2 && num2 < timeSpan2.Ticks)
		{
			return true;
		}
		return false;
	}

	public static bool IsTimeRange(int[] starttimes)
	{
		if (starttimes == null || starttimes.Length != 6)
		{
			return false;
		}
		TimeSpan timeSpan = new DateTime(starttimes[0], starttimes[1], starttimes[2], starttimes[3], starttimes[4], starttimes[5]).Subtract(new DateTime(1970, 1, 1));
		long num = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.TimeOffset * 3600;
		long num2 = (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime() - num) * SECONDS_TO_TICKS;
		if (timeSpan.Ticks <= num2)
		{
			return true;
		}
		return false;
	}

	public static bool IsTimeRange(long starttime, long endtime)
	{
		TimeSpan timeSpan = new TimeSpan((864000000000L + new TimeSpan(starttime * SECONDS_TO_TICKS).Ticks) % 864000000000L);
		TimeSpan timeSpan2 = new TimeSpan((864000000000L + new TimeSpan(endtime * SECONDS_TO_TICKS).Ticks) % 864000000000L);
		long num = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.TimeOffset * 3600;
		long num2 = (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime() - num) * SECONDS_TO_TICKS;
		TimeSpan timeSpan3 = new TimeSpan((864000000000L + num2) % 864000000000L);
		if (timeSpan.Ticks <= timeSpan3.Ticks && timeSpan3.Ticks <= timeSpan2.Ticks)
		{
			return true;
		}
		return false;
	}

	public static string GetLocalShowTime_HM(long startTime, long offsetTime)
	{
		TimeSpan timeSpan = new TimeSpan(startTime * SECONDS_TO_TICKS);
		TimeSpan timeSpan2 = new TimeSpan(offsetTime * SECONDS_TO_TICKS * 3600);
		timeSpan += timeSpan2;
		timeSpan = new TimeSpan((864000000000L + (timeSpan + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now)).Ticks) % 864000000000L);
		return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}";
	}

	public static int GetPassDays(long GetTime, long GetCurServerTime)
	{
		long num = GetCurServerTime - GetTime;
		return (int)new TimeSpan(0, 0, (int)num).TotalDays;
	}

	public static string GetLocalTime(long GetTime, long GetCurServerTime)
	{
		long num = GetTime - GetCurServerTime;
		TimeSpan timeSpan = new TimeSpan(0, 0, (int)num);
		DateTime dateTime = DateTime.Now.AddSeconds(num);
		return $"{dateTime.Hour:D2}:{dateTime.Minute:D2}";
	}

	public static string GetFormateTime(long time)
	{
		TimeSpan timeSpan = new TimeSpan(time * SECONDS_TO_TICKS);
		if (timeSpan.Days > 0 && timeSpan.Hours > 0)
		{
			return string.Format("{0} {1} {2} {3}", timeSpan.Days, StrDictionary.GetDictionaryString("#{100644}"), timeSpan.Hours, StrDictionary.GetDictionaryString("#{100645}"));
		}
		if (timeSpan.Days > 0)
		{
			return string.Format("{0} {1}", timeSpan.Days, StrDictionary.GetDictionaryString("#{100644}"));
		}
		if (timeSpan.Hours > 0)
		{
			return string.Format("{0} {1}", timeSpan.Hours, StrDictionary.GetDictionaryString("#{100645}"));
		}
		return string.Format("{0} {1}", 1, StrDictionary.GetDictionaryString("#{100645}"));
	}

	public static string GetMinuteSecondStr(int second)
	{
		int num = second % 60;
		int num2 = second / 60;
		sb.Length = 0;
		sb.AppendFormat("{0:D2}:{1:D2}", num2, num);
		return sb.ToString();
	}

	public static string GetMinuteSecondStr(long second)
	{
		return GetMinuteSecondStr((int)second);
	}

	public static string GetCentiSecondStr(int centisecond)
	{
		int num = centisecond % 100;
		int num2 = centisecond / 100 % 60;
		int num3 = centisecond / 100 / 60;
		sb.Length = 0;
		sb.AppendFormat("{0:D2}:{1:D2}:{2:D2}", num3, num2, num);
		return sb.ToString();
	}

	public static string GetHourMinSecStr(long times)
	{
		long num = times / 3600;
		times %= 3600;
		long num2 = times / 60;
		long num3 = times % 60;
		sb.Length = 0;
		sb.AppendFormat("{0:D2}:{1:D2}:{2:D2}", num, num2, num3);
		return sb.ToString();
	}

	public static string GetDaySecondStr(long times)
	{
		long num = times / DAY_SECOND;
		if (num > 0)
		{
			sb.Length = 0;
			sb.AppendFormat("{0} {1}", num, StrDictionary.GetDictionaryString("#{100644}"));
			return sb.ToString();
		}
		times %= DAY_SECOND;
		long num2 = times / 3600;
		times %= 3600;
		long num3 = times / 60;
		long num4 = times % 60;
		sb.Length = 0;
		sb.AppendFormat("{0:D2}:{1:D2}:{2:D2}", num2, num3, num4);
		return sb.ToString();
	}

	public static string GetFullTime(long times)
	{
		TimeSpan timeSpan = new TimeSpan(0, 0, (int)times);
		if (timeSpan.Days > 0)
		{
			return string.Format("{0}{1} {2:d2}:{3:d2}:{4:d2}", timeSpan.Days, StrDictionary.GetDictionaryString("#{100644}"), timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}
		return $"{timeSpan.Hours:d2}:{timeSpan.Minutes:d2}:{timeSpan.Seconds:d2}";
	}
}
