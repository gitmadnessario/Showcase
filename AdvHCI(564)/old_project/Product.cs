using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmlProject
{
    public class Product
    {
        String name;
        String expDate;
        String type=null;
        String picturePath;
        public Product(String name,String expDate)
        {
            this.name = name;
            this.expDate = expDate;
        }

        public Product(String name, String expDate, String type, String picturePath)
        {
            this.name = name;
            this.expDate = expDate;
            this.type = type;
            this.picturePath = picturePath;

        }

       public String getProductName(){
            return this.name;
        }

       public String getProductExpDate()
        {
            return this.expDate;
        }

       public String getProductType()
       {
           return this.type;
       }
       public String getPicturePath()
       {
           return this.picturePath;
       }



    }
}
