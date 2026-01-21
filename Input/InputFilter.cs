using System.Text.RegularExpressions;

namespace MrAndMissUniversity;

public static class InputFilter
{
    public static readonly Regex regexFullName = new("^[А-Яа-яЁё]{1,40} [А-Яа-яЁё]{1,40} [А-Яа-яЁё]{1,40}$");
    public static readonly Regex regexYearAndGroup = new
        (@"(^[1-9]{1} )(([0-5]{1}[0-9]{2,3}(\.11|\.9){0,1}( Х){0,1}( М){0,1}( м){0,1}( У){0,1}( ПОНБ){0,1}( О/З){0,1}( (п/ч)){0,1}$)|(МиТПО|ППГПН|РиОЭ|САУиОИ|ССиУТК|ЭБ|ЭВТ ВПСиГ){1}$)");
    public static async Task<bool> IsFullName(this string fullName)
    {
        return regexFullName.IsMatch(fullName);
    }
    public static async Task<bool> IsYearAndGroup(this string yearAndGroup)
    {
        return regexYearAndGroup.IsMatch(yearAndGroup);
    }
}
