using yakov.Lab5;

DynamicList<int> list = new DynamicList<int>();

list.Add(1);
list.Add(5);
list.Add(6);

Console.WriteLine(list[2]);
Console.WriteLine(list.Count());
list.Add(10);
list.Remove(11);
list.RemoveAt(1);

foreach (var item in list)
{
    Console.WriteLine(item);
}

list.Clear();

Console.WriteLine(list.Count);