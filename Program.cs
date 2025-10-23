//Колекції: можуть мати об'єкти різного типу, не обов'язково послідовні, варіативний розмір
using Sharp_231.Library;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
Console.OutputEncoding = Encoding.UTF8;


Library library = new();
library.PrintCatalog();

// Problem1();
void Collections()
{
        //\ед ліст більш корисний для модифікаці, встановлуються, видаляються елеиенти
        List < String > list = [];
    list.Add("str1");
    list.Add("str2");
    list.Add("str3");

    list.RemoveAt(0);
    foreach (string s in list)
    {
        Console.WriteLine(s);
};
    LinkedList<string> list2 = [];
    list.Add("str1");
    list.Add("str2");
    list.Add("str3");

    foreach (string s in list)
    {
        Console.WriteLine(s);
    }
    //Асоціативні масиви, словники 
    Dictionary<string, string> map = [];
    map.Add("key 1", "valiue 1");

    map["key 2"] = "value 2";
    foreach (var pair in map)
    {
        Console.WriteLine("{0} {1}", pair.Key, pair.Value);
    }


    //String - immutable
    string str = "";
    foreach (string s in list)
    {
        str += 5;
    }

    StringBuilder sb = new();
    foreach (string s in list)
    {
        sb.Append(s);
    }
    Console.WriteLine(sb.ToString());


    //Масиви:однотипні, послідовні, фісований розмір
    int[] arr = new int[10];
    foreach (int el in arr)
    {
        Console.WriteLine(el);

    }
    Console.WriteLine(arr[1]);
    for (int i = 0; i < arr.Length; i++)
    {
        Console.WriteLine(arr[i]);
    }

    arr[0] = default;
    int[][] arr2 = new int[5][];
    for (int i = 0; i < 5; i++)
    {
        arr2[i] = new int[i + 1];
    }
    foreach (var el in arr2)
    {
        foreach (var w in el)
        {
            Console.Write(w + " ");
        }
        Console.WriteLine();
    }
    int[,] arr3 = new int[3, 4];
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            Console.Write(arr3[i, j]);
        }
    }
    string[] strings = { "s1", "s2" };
    String[] s_2 = { "s1, s2" };

    Console.Write("Enter your name: ");   // Виведення без переводу рядка

    // NULL-safety — традиція у сучасних мовах програмування, згідно з якою
    // розрізняються типи даних, які дозволяють значення NULL, та ті,
    // які не дозволяють
    string? name = Console.ReadLine();    // Введення з консолі

    if (String.IsNullOrEmpty(name))
    {
        Console.WriteLine("Bye");
    }
    else
    {
        Console.WriteLine("Hello, " + name);   // Виведення з переводом рядка
    }

    // Типи даних
    // Усі типи даних є нащадками загального типу Object
    // Через це вони мають ряд спільних методів: GetType, ToString, GetHashCode, ...
    // Типи данних належать простору імен Systemб для скорочення нструкцій
    //Існують псевдоніми типів:
    int x; // псевдонім для System.Int32
    System.Int32 y;
    string s1;// псевдонім для System.String  
    System.String s2;
    float f; // псевдонім для System.Single -- 32-бітa
    double g; // псевдонім для System.Double -- 64-бітa
              //Nullable - версії
    Nullable<int> v;
    int? a; // скорочена форма Nullable<int>

    Console.Write("How old are you: ");
    String ageInput = Console.ReadLine()!;//!(в кінці) - null-checker 
    int age = int.Parse(ageInput); // перетворення рядка в ціле число
    Console.WriteLine("Next year you`ll be " + (age + 1));

    Console.Write("Previous ages: ");
    for (int i = 0; i < age; i++)
    {
        Console.Write(i + " ");
    }
    int rem = 10 % 3;
    Console.WriteLine();
    char c = ageInput[0]; // отримання першого символу рядка
    ageInput = "A" + ageInput.Substring(1); // заміна першого символу рядка

}

void Problem1()
{
    int even = 0;
    int odd = 0;
    int unique = 0;
    int[] arr = new int[20];
    Random rnd = new();
    for (int i = 0; i < arr.Length; i++)
    {
        arr[i] = rnd.Next(10);
    }
    for (int i = 0; i < arr.Length; i++)
    {
        Console.WriteLine(arr[i]);
    }

    foreach (int el in arr)
    {
        if (el % 2 == 0)
        {
            even += 1;
        }
        else 
        {
            odd += 1;
        }
    }
    for (int i = 0; i < arr.Length; i++)
    {
        bool isUnique = true;
        for (int j = 0; j < arr.Length; j++)
        {
            if (i != j && arr[i] == arr[j])
            {
                isUnique = false;
                break;
            }
        }
        if (isUnique)
        {
            unique++;
        }

    }
    Console.WriteLine("Even: " + even);
    Console.WriteLine("Odd: " + odd);
    Console.WriteLine("Unique: " + unique);
}