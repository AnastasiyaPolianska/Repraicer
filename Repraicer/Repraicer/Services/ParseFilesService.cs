using Repraicer.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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

            var products = lines.Skip(1).Select(x =>
            {
                var itemsString = x.Split('\t');
                return new Product
                {
                    SellerSku = itemsString[3],
                    ItemName = itemsString[0],
                    OpenDate = GetDate(itemsString[6]),
                    ItemCondition = itemsString[12],

                    ImageUrl = string.IsNullOrEmpty(itemsString[7])
                        ? new Uri("http://www.codeodor.com/images/Empty_set.png", UriKind.Absolute)
                        : new Uri(itemsString[7], UriKind.Absolute),
                    Price = double.Parse(itemsString[4], CultureInfo.InvariantCulture),
                    NewPrice = double.Parse(itemsString[4], CultureInfo.InvariantCulture),
                    Quantity = string.IsNullOrEmpty(itemsString[5]) ? 0: int.Parse(itemsString[5], CultureInfo.InvariantCulture),
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

                    product.Quantity = item.QuantityAvailable;
                    product.Condition = item.ConditionType;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <param name="income">The income.</param>
        /// <returns></returns>
        private DateTime GetDate(string income)
        {
            var datePart = ((string)income)?.Split(' ')[0];
            var startDate = DateTime.Parse(datePart);

            return startDate;
        }
    }
}