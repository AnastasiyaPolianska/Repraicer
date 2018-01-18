using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Repraicer.Model;

namespace Repraicer.Services
{
    public class ParseFilesService
    {
        public List<Item> ReadItemsFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            List<Item> items = new List<Item>();
            for(int i = 1; i < lines.Length; i++)
            {
                var itemsString =  lines[i].Split('\t');
                items.Add(new Item{SellerSku = itemsString[0], conditionType = itemsString[3], QuantityAvailable = Int32.Parse(itemsString[itemsString.Length - 1])});


            }
            return items.Where(q => q.QuantityAvailable > 0).ToList();
        }


        public List<Product> ReadProductsFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            List<Product> products = new List<Product>();
            for (int i = 1; i < lines.Length; i++)
            {
                var itemsString = lines[i].Split('\t');
                products.Add(
                    new Product
                    {
                        SellerSku = itemsString[3],
                        ItemName = itemsString[0],
                        OpenDate = itemsString[6],
                        ItemCondition = itemsString[12],
                        
                        ImageUrl = String.IsNullOrEmpty(itemsString[7]) ? new Uri("http://www.codeodor.com/images/Empty_set.png", UriKind.Absolute) : new Uri(itemsString[7], UriKind.Absolute) ,
                        Price = itemsString[4],
                        Quantity = itemsString[5],
                        asin1 = itemsString[16]
                    });


            }
            List<Item> items = ReadItemsFile("AmazonFulfilled.txt");
            List<Product> result = new List<Product>();

            foreach (Item item in items)
            {
                result.AddRange(products.Where(q => q.SellerSku == item.SellerSku));
            }
            foreach (var item in items)
            {
                foreach (var product in result)
                {
                    if (product.SellerSku == item.SellerSku)
                    {
                        product.Quantity = item.QuantityAvailable.ToString();
                        product.Condition = item.conditionType;
                    }
                }
            }

            return result;
        }
    }
}
