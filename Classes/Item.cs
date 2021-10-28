using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Classes
{
    public abstract class Item
    {
        public double Price { get; set; }
        public int Item_ID { get; set; }

        public abstract int Update();
        public abstract int Add();
        public abstract int Delete();
        public abstract int Get();
    }
}
