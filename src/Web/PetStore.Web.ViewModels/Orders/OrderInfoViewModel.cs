using System.ComponentModel.DataAnnotations;

namespace PetStore.Web.ViewModels.Orders
{
    public class OrderInfoViewModel
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public string ProductId { get; set; }
    }
}
