using System.ComponentModel;

namespace SmartBooking.Application.Enums
{
    public enum RoomType : int
    {
        [Description("Single")]
        Single = 1,
        [Description("Double")]
        Double = 2, 
        [Description("Deluxe")]
        Deluxe = 3 
    }

    public static class StringEnum
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
