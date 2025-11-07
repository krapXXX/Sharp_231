using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnP321.Events.Notifier
{
    internal class GlobalState
    {
        public double Price { get; set; }
        public DateTime LastSyncMoment { get; set; }
        public String? UserName { get; set; }
        public String ActivePage { get; set; } = "Home";
    }
}

/* Формалізм ChangeNotifier передбачає запуск підписників з передачею
 * до них імені поля/властивості, що зазнало зміни.
 * Ось текст зі зображення, переписаний точно:

* Наприклад, на сторінці Price потрібна інформація про ціну,
* на сторінці Router — про ActivePage
* на сторінці Profile — про UserName
* на всіх сторінках — LastSyncMoment

 */
