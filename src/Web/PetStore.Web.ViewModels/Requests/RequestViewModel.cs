namespace PetStore.Web.ViewModels.Requests
{
    using System;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class RequestViewModel : IMapFrom<Request>, IMapTo<Request>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string SenderEmail { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
