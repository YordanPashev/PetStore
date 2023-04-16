namespace PetStore.Web.ViewModels.Pets
{
    public class PetTypeViewModel
    {
        public PetTypeViewModel(string name, string imageUrl, int petsCount)
        {
            this.Name = name;
            this.ImageUrl = imageUrl;
            this.PetsCount = petsCount;
        }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int PetsCount { get; set; }
    }
}
