using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmlProject
{
    public class Fridge
    {
        Product[] fridgeShelves;

        static int size = 5;

        public Fridge()
        {
            fridgeShelves = new Product[size];
            for (int i = 0; i < 5; i++)
            {
                fridgeShelves[i] = new Product("empty", "no");
            }
        }

        public void addProductToFridge(Product newProduct, int slot)
        {
            if (0 <= slot && slot < size)
            {
                fridgeShelves[slot] = newProduct;
            }

        }

        public Product removeProductFromFridge(int slot)
        {
            Product product;
            if (0 <= slot && slot < size)
            {
                product = fridgeShelves[slot];
                fridgeShelves[slot] = new Product("empty", "no");
            }
            else
            {
                product = new Product("error", "no");
            }
            return product;
        }


        public int getSize()
        {
            return size;
        }

        public Product checkSlot(int slot)
        {
            Product product;
            if (0 <= slot && slot < size)
            {
                product = fridgeShelves[slot];
            }
            else
            {
                product = new Product("error", "no");
            }
            return product;
        }
    }
}
