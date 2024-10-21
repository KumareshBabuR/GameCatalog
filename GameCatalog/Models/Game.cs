namespace GameCatalog.Models
{
    public class Game
    {

        //ID, Title, Genre, Description, Price, Release Date, and Stock Quantity.
                
        //ID is an an identity column.
        public int ID { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int StockQuantity { get; set; }
    }
}
