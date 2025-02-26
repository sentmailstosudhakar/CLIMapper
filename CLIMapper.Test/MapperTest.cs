namespace CLIMapper.Test;

public class MapperTest
{
    /// <summary>
    /// CLI Mapper should get parse and map command line arguments into respective properties of the given Type.
    /// CLI Mapper should set true for bool property if the standalone flag is true.
    /// CLI Mapper should set command for string property if the standalone flag is true.
    /// </summary>
    /// <param name="args"></param>
    [Theory]
    [InlineData("num", "100", "float", "10.10", "decimal", "123.123", "char", "c", "string", "s t r i n g", "--b", "true", "--alone", "--aloneString")]
    public void Map_ShouldBindArgs(params string[] args)
    {
        //Arrange 
        var expectedResult = Test.GetExpectedInstance(100, 10.10f, 123.123M, 'c', "s t r i n g", true, true, "--aloneString");
        //Act
        var actualResult = Mapper.Map<Test>(args);
        //Assert
        Assert.Equal(expectedResult, actualResult);
    }

    /// <summary>
    /// CLI Mapper should not process when there is no argument given.
    /// </summary>
    [Fact]
    public void Map_ShouldReturnEmptyObject()
    {
        //Arrange 
        var expectedResult = Test.GetExpectedInstance();
        //Act
        var actualResult = Mapper.Map<Test>(Array.Empty<string>());
        //Assert
        Assert.Equal(expectedResult, actualResult);
    }

    /// <summary>
    /// CLI Mapper should accept both long and short name for commands.
    /// </summary>
    /// <param name="args"></param>
    [Theory]
    [InlineData("n", "100", "f", "10.10", "d", "123.123", "c", "c", "s", "s t r i n g", "-b", "true", "-a", "-as")]
    public void Map_ShouldBindArgsForShortHand(params string[] args)
    {
        //Arrange 
        var expectedResult = Test.GetExpectedInstance(100, 10.10f, 123.123M, 'c', "s t r i n g", true, true, "-as");
        //Act
        var actualResult = Mapper.Map<Test>(args);
        //Assert
        Assert.Equal(expectedResult, actualResult);
    }

    /// <summary>
    /// CLI Mapper should overwrite value for non collection types if similar commands present multiple times in the argument.
    /// </summary>
    /// <param name="args"></param>
    [Theory]
    [InlineData("num", "100", "n", "150")]
    public void Map_ShouldOverwriteValueForSimilarCommands(params string[] args)
    {
        //Arrange 
        var expectedResult = Test.GetExpectedInstance(150);
        //Act
        var actualResult = Mapper.Map<Test>(args);
        //Assert
        Assert.Equal(expectedResult, actualResult);
    }

    /// <summary>
    /// CLI Mapper should add values for collection types if similar commands present multiple times in the argument.
    /// </summary>
    /// <param name="args"></param>
    [Theory]
    [InlineData("nums", "100", "ns", "150", "list", "Value1", "list", "Value2", "-l", "Value3", "-l", "Value4")]
    public void Map_ShouldAddValueForSimilarCommands(params string[] args)
    {
        //Arrange 
        var expectedResult = Test.GetExpectedInstance(_list: new List<string> { "Value1", "Value2", "Value3", "Value4" }, _numbers: [100, 150]);
        //Act
        var actualResult = Mapper.Map<Test>(args);
        //Assert
        Assert.Equal(expectedResult, actualResult);
    }

    /// <summary>
    /// CLI Mapper shoud not throw exception when there is no value for given command.
    /// </summary>
    /// <param name="args"></param>
    [Theory]
    [InlineData("num")]
    public void Map_ShouldNotThrowExceptionForNullValues(params string[] args)
    {
        //Arrange 
        var expectedResult = Test.GetExpectedInstance();
        //Act
        var actualResult = Mapper.Map<Test>(args);
        //Assert
        Assert.Equal(expectedResult, actualResult);
    }

    /// <summary>
    /// CLI Mapper should throw exception when value and datatype of the property not matches.
    /// </summary>
    /// <param name="args"></param>
    [Theory]
    [InlineData("num", "string")]
    public void Map_ShouldThrowExceptionForTypeMismatch(params string[] args)
    {
        //Arrange //Act //Assert
        Assert.Throws<FormatException>(() => Mapper.Map<Test>(args));
    }

    /// <summary>
    /// CLI Mapper should throw exception when the datatype is not string/bool and standalone flag is true.
    /// </summary>
    /// <param name="args"></param>
    [Theory]
    [InlineData("num", "12.12")]
    public void Map_ShouldThrowExceptionForStandAlone_NonStringBool(params string[] args)
    {
        //Arrange //Act //Assert
        Assert.Throws<MapperException>(() => Mapper.Map<StadnAloneValidation>(args));
    }

    /// <summary>
    /// CLI Mapper should throw exception when there is no property annotated with command.
    /// </summary>
    /// <param name="args"></param>
    [Theory]
    [InlineData("num")]
    public void Map_ShouldThrowExceptionForNoCommandAttribute(params string[] args)
    {
        //Arrange
        var instance = NoAttribute.GetInstance();
        // //Act //Assert
        Assert.Throws<MapperException>(() => Mapper.Map<NoAttribute>(args));
    }
    
    /// <summary>
    /// CLI Mapper should throw exception when there are duplicate commands.
    /// </summary>
    /// <param name="args"></param>
    [Theory]
    [InlineData("num", "10", "num", "12")]
    public void Map_ShouldThrowExceptionForDuplicateCommandName(params string[] args)
    {
        //Arrange //Act //Assert
        Assert.Throws<ArgumentException>(() => Mapper.Map<DuplicateCommand>(args));
    }
}