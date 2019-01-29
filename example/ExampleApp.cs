using System;

public class ExampleApp
{
    public static void Main(string[] args)
    {
        var newText = Pangu.SpacingText("东邪、西毒、南帝、北丐和Ichiro Suzuki五人，为争一部The Old Man and the Sea，约定在板桥Mega City的3楼比武较量。");
        Console.WriteLine(newText);
    }
}