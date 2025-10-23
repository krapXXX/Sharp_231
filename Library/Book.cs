using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Library
{
    public class Book : Literature
    {
        public String Author { get; set; } = null!;
        public int Year { get; set; }

        public override string GetCard()
        {
            return $"{this.Author}, {base.Title} - {base.Publisher} - {this.Year}";
        }
    }

}
/*
CTS – Common Type System
class – reference тип даних, який оперується за посиланням
struct – value тип, оперується за значенням
A = B
за посиланням: утворення другого посилання на той самий об'єкт
за значенням: створення копії об'єкту
record - більше нові типи, призначенні для immutable-щб'єктів, надбудова над класмо
record struct 

доступність типів
internal - в середині збірки
public - загальний доступ 

складові 
поля (fields) - описані змінні всередині класу
методи 
властивості (properties)  
аксесори (get, set)
події (events)

доступність елементів
зrivate - тільки в данному класі
protected - тільки для нашадків
public -загальний доступ


Modeling:          Generalization:          Relation:

Book         \
Journal      ------> Literature   -----<>   Library
Newspaper    /
Booklet   /

Encapsulation ------ Abstraction ------- Polymorphism 
      Inheritance
      Extension

Encapsulation -- 2
Book         Journal       Newspaper     Booklet
Author      Publisher      Publisher      Author
Title         Title         Title         Title
Year         Number         Date          Date
Publisher

        Literature
         Publisher
           Title
    /        |         \         \
 Book     Journal   Newspaper     Booklet
Author     Number      Date        Autor
 Year                               Date
 */
