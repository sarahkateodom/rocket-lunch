namespace RocketLunch.domain.enumerations
{
    public enum MealTime
    {
        all = 0,
        breakfast = 1,
        lunch = 2,
        dinner = 3
    }

    public static class MealTimeExtensions
    {
        public static int GetHoursFromMidnight(this MealTime meal)
        {
            if (meal == MealTime.breakfast) return 8;
            if (meal == MealTime.lunch) return 12;
            if (meal == MealTime.dinner) return 18;
            return 0;
        }
    }
}