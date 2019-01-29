# Pangu.cs

Paranoid text spacing for good readability, to automatically insert whitespace between CJK (Chinese, Japanese, Korean), half-width English, digit and symbol characters.

## Usage

```csharp
using System;

public class Test
{
    public static void Main(string[] args)
    {
        var newText = Pangu.SpacingText("请问Jackie的鼻子有几个？123个！");
        Console.WriteLine(newText); // will be "请问 Jackie 的鼻子有几个？123 个！"
    }
}
```
