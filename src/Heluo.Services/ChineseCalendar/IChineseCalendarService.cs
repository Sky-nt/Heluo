using tyme.lunar;

namespace Heluo.Services.ChineseCalendar;

public interface IChineseCalendarService
{
	/// <summary>
	/// 获取农历日
	/// </summary>
	/// <returns></returns>
	LunarDay GetChineseCalendarDay(DateTime dateTime);

}
