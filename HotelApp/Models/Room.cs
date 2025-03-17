namespace HotelApp.Models
{
    public class Room
    {
        public int Id { get; set; }  
        public string RoomNumber { get; set; }  // Rumsnummer
        public string RoomType { get; set; }  // Rumstyp (t.ex. Enkelrum, Dubbelrum)
        public int Capacity { get; set; }  // Kapacitet (antal personer)
    }
}
