// See https://aka.ms/new-console-template for more information

using CS2Dumper;
using System.Numerics;
using System.Reflection;
using System.Text;

var allFields = AppDomain.CurrentDomain
    .GetAssemblies()
    .SelectMany(assembly => assembly.GetTypes())
    .Where(type => type.IsClass && type.Namespace != null && type.Namespace.StartsWith("CS2Dumper"))
    .SelectMany(type => type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic));

var sb = new StringBuilder();
foreach (var field in allFields)
{
    if (field.DeclaringType != null)
        sb.Append(field.DeclaringType.FullName).Append(": ");

    var objValue = field.GetValue(null);
    var hexValue = objValue switch
    {
        nint ni => $"0x{ni:X}",
        nuint nui => $"0x{nui:X}",
        byte b => $"0x{b:X}",
        ushort us => $"0x{us:X}",
        uint ui => $"0x{ui:X}",
        ulong ul => $"0x{ul:X}",
        sbyte bs => $"0x{bs:X}",
        short s => $"0x{s:X}",
        int i => $"0x{i:X}",
        long l => $"0x{l:X}",
        float f => f.ToString(),
        double d => d.ToString(),
        _ => throw new NotSupportedException()
    };

    sb.Append(field.Name).Append(" = ").Append(hexValue).AppendLine();
}

var allEnums = AppDomain.CurrentDomain
    .GetAssemblies()
    .SelectMany(assembly => assembly.GetTypes())
    .Where(type => type.IsEnum && type.Namespace != null && type.Namespace.StartsWith("CS2Dumper"));
foreach (var enumType in allEnums)
{
    var values = Enum.GetValuesAsUnderlyingType(enumType);
    foreach (var v in values)
    {
        var name = Enum.GetName(enumType, v);
        var hexValue = v switch
        {
            nint ni => $"0x{ni:X}",
            nuint nui => $"0x{nui:X}",
            byte b => $"0x{b:X}",
            ushort us => $"0x{us:X}",
            uint ui => $"0x{ui:X}",
            ulong ul => $"0x{ul:X}",
            sbyte bs => $"0x{bs:X}",
            short s => $"0x{s:X}",
            int i => $"0x{i:X}",
            long l => $"0x{l:X}",
            float f => f.ToString(),
            double d => d.ToString(),
            _ => throw new NotSupportedException()
        };
        sb.Append(enumType.FullName).Append(": ").Append(name).Append(" = ").Append(hexValue).AppendLine();
    }
}

Console.WriteLine(sb.ToString());