using tyme.lunar;
using tyme.solar;

namespace Heluo.Services.ChineseCalendar;

/// <summary>
/// 农历服务
/// </summary>
public class ChineseCalendarService : IChineseCalendarService
{
	/// <summary>
	/// 获取农历日
	/// </summary>
	/// <param name="dateTime"></param>
	/// <returns></returns>
	public LunarDay GetChineseCalendarDay(DateTime dateTime)
	{
		// 公历2024年2月9日
		SolarDay solarDay = SolarDay.FromYmd(dateTime.Year,dateTime.Month,dateTime.Day);
		// 农历癸卯年十二月三十
		LunarDay lunarDay = solarDay.GetLunarDay();
		return lunarDay;
	}
}
