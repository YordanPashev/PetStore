﻿namespace PetStore.Web.ViewModels.ClientCard
{
    using System;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.User;

    public class ClientCardDetaislsViewModel : IMapFrom<ClientCard>, IMapTo<ClientCard>
    {
        public string Id { get; set; }

        public string CardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int Discount { get; set; }

        public virtual UserDetailsViewModel Client { get; set; }
    }
}
