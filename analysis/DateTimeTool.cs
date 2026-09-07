using System;

public class DateTimeTool
{
	public static int CalculateWorkingDays(DateTime dtStart, DateTime dtEnd)
	{
		return (int)(dtEnd - dtStart).TotalDays;
	}

	public static int CalculateHours(DateTime dtStart, DateTime dtEnd)
	{
		return (int)(dtEnd - dtStart).TotalHours;
	}

	public static DateTime LongToDateTimeLocal(long time)
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		DateTime dateTime2 = new DateTime(time * 10000000, DateTimeKind.Utc).ToLocalTime();
		return new DateTime(time * 10000000 + dateTime.Ticks).ToLocalTime();
	}

	public static double CalculateSeconds(DateTime dtStart, DateTime dtEnd)
	{
		return (dtEnd - dtStart).TotalSeconds;
	}

	public static string GetTimeByLong(long time)
	{
		long num = time / 3600;
		long num2 = time % 3600 / 60;
		time %= 60;
		return $"{num:D2}:{num2:D2}:{time:D2}";
	}
}
