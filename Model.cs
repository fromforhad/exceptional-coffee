namespace CoffeeModel
{
    public class Coffee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public char Size { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public Coffee(int Id, string Name, char Size, int Quantity, double Price)
        {
            this.Id = Id;
            this.Name = Name;
            this.Size = Size;
            this.Quantity = Quantity;
            this.Price = Price;
        }

    }
}