namespace PetStore.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Models.Enums;

    public class OrderInfoViewModel
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public string ProductId { get; set; }
    }
}
