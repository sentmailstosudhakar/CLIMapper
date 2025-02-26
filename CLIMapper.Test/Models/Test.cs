namespace CLIMapper.Test;
internal sealed  class Test
{
    #region Properties
    [Command("num|n")]
    public int Number { get; set; }
    [Command("float|f")]
    public float Float { get; set; }
    [Command("Decimal|d")]
    public decimal Decimal { get; set; }
    [Command("Char|c")]
    public char Char { get; set; }
    [Command("string|s")]
    public string StringValue { get; set; } = string.Empty;
    [Command("--b|-b")]
    public bool Bool { get; set; }
    [Command("--alone|-a", true)]
    public bool StandAloneBool { get; set; }
    [Command("--aloneString|-as", true)]
    public string StandAloneString { get; set; } = string.Empty;
    [Command("nums|ns")]
    public int[] Numbers { get; set; } = Array.Empty<int>();
    [Command("list|-l")]
    public List<string> List { get; set; } = new();
    #endregion

    #region Public Overriden Methods
    public override bool Equals(object? obj)
    {
        if (obj is Test test)
        {
            return this.Number == test.Number
                && this.Float == test.Float
                && this.Decimal == test.Decimal
                && this.Char == test.Char
                && this.StringValue == test.StringValue
                && this.Bool == test.Bool
                && this.StandAloneBool == test.StandAloneBool
                && this.StandAloneString == test.StandAloneString
                && Equals<int>(this.Numbers, test.Numbers)
                && Equals<string>(this.List, test.List);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion

    #region For Test Methods

    public static Test GetExpectedInstance(
        int _number = default,
        float _float = default,
        decimal _decimal = default,
        char _c = default,
        string? _string = default,
        bool _bool = default,
        bool _alonebool = default,
        string? _aloneString = default,
        int[]? _numbers = default,
        List<string>? _list = default)
    => new Test
    {
        Number = _number,
        Float = _float,
        Decimal = _decimal,
        Char = _c,
        StringValue = _string ?? string.Empty,
        Bool = _bool,
        StandAloneBool = _alonebool,
        StandAloneString = _aloneString ?? string.Empty,
        Numbers = _numbers ?? Array.Empty<int>(),
        List = _list ?? new()
    };
    #endregion

    #region Private Methods
    public bool Equals<T>(ICollection<T> collection1, ICollection<T> collection2)
    {
        if (collection1 == null && collection2 == null)
            return true;
        if (collection1 == null || collection2 == null)
            return false;
        if (collection1.Count != collection2.Count)
            return false;
        return collection1.All(c => collection2.Contains(c));
    }
    #endregion
}

internal sealed class NoAttribute
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public static NoAttribute GetInstance() => new NoAttribute();
}

internal sealed class StadnAloneValidation
{
    [Command("num", true)]
    public int StandAloneNumber { get; set; }
}

internal sealed class DuplicateCommand
{
    [Command("num", true)]
    public int StandAloneNumber { get; set; }

    [Command("num", false)]
    public string StandAloneString { get; set; } = string.Empty;
}