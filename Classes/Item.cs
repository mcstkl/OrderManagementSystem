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

        public abstract int Update(int itemID, string name, double price, int instock);
        public abstract int Add();
        public abstract int Delete(int itemID);
        public abstract int Get();
    }
}
