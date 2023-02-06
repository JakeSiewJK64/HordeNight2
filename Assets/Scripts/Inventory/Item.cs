public class Item
{
    string name { get; set; }
    string description { get; set; }
    ItemType itemType { get; set; }
    float price { get; set; }

    public Item(string name, string description, ItemType itemType, float price)
    {
        this.name = name;
        this.description = description;
        this.itemType = itemType;
        this.price = price;
    }
}
