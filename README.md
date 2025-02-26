# CLIMapper

## Overview
CLIMapper is a tool designed to parse and map command line arguments into C# types, making it easier to handle user inputs in a structured and type-safe manner.

## Features
- **Type Safety**: Automatically converts command line arguments to specified C# types.
- **Ease of Use**: Simplifies the process of handling command line inputs.
- **Extensible**: Easily extendable to support custom types.
- **Collection Support**: Provide support to add more commands for collection types.
- **No Dependency**: No need of any other packages.

## Usage

``` Shell

dotnet add package CLIMapper --version 1.0.0
```

## Example
``` Csharp
using CLIMapper;

class Program
{
    public class User
    {
        [Command("--name|-n")]
        public string Name { get; set; }

        [Command("--email|-e")]
        public string Email { get; set; }

        [Command("--phone|-p")]
        public List<int> PhoneNumbers { get; set; }

        [Command("--active|-a", true)]
        public bool Active { get; set; }
    }
    static void Main(string[] args)
    {
        var arguments = Mapper.Map<User>(args);
        Console.WriteLine(arguments.Name);
    }
}

```