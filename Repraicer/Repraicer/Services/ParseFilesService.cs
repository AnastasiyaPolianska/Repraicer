using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Repraicer.Model;

namespace Repraicer.Services
{
    public class ParseFilesService
    {
        /// <summary>
        /// Reads the items file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public List<Item> ReadItemsFile(string path)
        {
            var lines = File.ReadAllLines(path);
            
            //List<Item> items = new List<Item>();
            //for(int i = 1; i < lines.Length; i++)
            //{
            //    var itemsString =  lines[i].Split('\t');
            //    items.Add(new Item{SellerSku = itemsString[0], ConditionType = itemsString[3], QuantityAvailable = Int32.Parse(itemsString[itemsString.Length - 1])});
            // }

            //converted into LINQ expression
            var items = lines.Skip(1).Select(x =>
            {
                var itemsString = x.Split('\t');
                return new Item
                {
                    SellerSku = itemsString[0],
                    ConditionType = itemsString[3],
                    QuantityAvailable = int.Parse(itemsString[itemsString.Length - 1])
                };
            });

            return items.Where(q => q.QuantityAvailable > 0).ToList();
        }

        /// <summary>
        /// Reads the products file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public List<Product> ReadProductsFile(string path)
        {
            var lines = File.ReadAllLines(path);

            //var products = new List<Product>();

            //for (int i = 1; i < lines.Length; i++)
            //{
            //    var itemsString = lines[i].Split('\t');
            //    products.Add(
            //        new Product
            //        {
            //            SellerSku = itemsString[3],
            //            ItemName = itemsString[0],
            //            OpenDate = itemsString[6],
            //            ItemCondition = itemsString[12],

            //            ImageUrl = String.IsNullOrEmpty(itemsString[7]) ? new Uri("http://www.codeodor.com/images/Empty_set.png", UriKind.Absolute) : new Uri(itemsString[7], UriKind.Absolute) ,
            //            Price = itemsString[4],
            //            Quantity = itemsString[5],
            //            Asin1 = itemsString[16]
            //        });
            //}

            //converted into LINQ expression
            var products = lines.Skip(1).Select(x =>
            {
                var itemsString = x.Split('\t');
                return new Product
                {
                    SellerSku = itemsString[3],
                    ItemName = itemsString[0],
                    OpenDate = itemsString[6],
                    ItemCondition = itemsString[12],

                    ImageUrl = string.IsNullOrEmpty(itemsString[7])
                        ? new Uri("http://www.codeodor.com/images/Empty_set.png", UriKind.Absolute)
                        : new Uri(itemsString[7], UriKind.Absolute),
                    Price = itemsString[4],
                    Quantity = itemsString[5],
                    Asin1 = itemsString[16]
                };
            });

            var items = ReadItemsFile("AmazonFulfilled.txt");
            var result = new List<Product>();

            foreach (var item in items)
            {
                result.AddRange(products.Where(q => q.SellerSku == item.SellerSku));
            }

            foreach (var item in items)
            {
                foreach (var product in result)
                {
                    if (product.SellerSku != item.SellerSku)
                    {
                        continue;
                    }

                    product.Quantity = item.QuantityAvailable.ToString();
                    product.Condition = item.ConditionType;
                }
            }

            return result;
        }
    }
}