namespace PetStore.Web.ViewModels.Pets
{
    public class PetTypeViewModel
    {
        public PetTypeViewModel(string name, string imageUrl)
        {
            this.Name = name;
            this.ImageUrl = imageUrl;
        }

        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}
