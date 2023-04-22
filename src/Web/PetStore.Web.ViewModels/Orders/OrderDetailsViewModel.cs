namespace PetStore.Web.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PetStore.Web.ViewModels.Products;
    using PetStore.Web.ViewModels.User;

    public class OrderDetailsViewModel
    {
        [Required]
        public UserDetailsViewModel Client { get; set; }

        [Required]
        public ProductShortInfoViewModel Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        public decimal TotalPriceWithoutDiscount => this.Product.Price * this.Quantity;

        public decimal? TotalPriceWithDiscount => this.GetTotalPriceWithDiscount();

        private decimal? GetTotalPriceWithDiscount()
        {
            if (this.Client == null)
            {
                return null;
            }

            decimal discount = ((decimal)this.Client.ClientCard.Discount / 100) * this.TotalPriceWithoutDiscount;
            decimal totalPriceWithDiscount = this.TotalPriceWithoutDiscount - discount;

            return Math.Round(totalPriceWithDiscount, 2);
        }
    }
}
