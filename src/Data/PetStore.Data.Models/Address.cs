// ReSharper disable RedundantNameQualifier
namespace PetStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;

    public class Address : BaseDeletableModel<string>
    {
        public Address()
            => this.Clients = new HashSet<Client>();

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(AddressValidationConstants.TextMaxLength)]
        public string AddressText { get; set; }

        [Required]
        [MaxLength(AddressValidationConstants.TownNameMaxLength)]
        public string TownName { get; set; }

        [Required]
        [ForeignKey(nameof(ClientId))]
        public string ClientId { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
