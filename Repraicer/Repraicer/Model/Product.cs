using System;

namespace Repraicer.Model
{
    public class Product
    {
        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        public string ListingId { get; set; }

        public string SellerSku { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public DateTime OpenDate { get; set; }

        public Uri ImageUrl { get; set; }

        public string ItemIsMarketplace { get; set; }

        public string ProductIdType { get; set; }

        public string ZshopShippingFee { get; set; }

        public string ItemNote { get; set; }

        public string ItemCondition { get; set; }

        public string ZshopCategory1 { get; set; }

        public string ZshopBrowsePath { get; set; }

        public string ZshopStorefrontFeature { get; set; }

        public string Asin1 { get; set; }

        public string Asin2 { get; set; }

        public string Asin3 { get; set; }

        public string WillShipInternationally { get; set; }

        public string ExpeditedShipping { get; set; }

        public string ZshopBoldface { get; set; }

        public string ProductId { get; set; }

        public string BidForFeaturedPlacement { get; set; }

        public string AddDelete { get; set; }

        public string PendingQuantity { get; set; }

        public string FulfillmentChannel { get; set; }

        public string Condition { get; set; }

        public double NewPrice { get; set; }
    }
}