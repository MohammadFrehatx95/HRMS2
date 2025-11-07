namespace HRMS2.Models
{
    public class Lookup
    {
        public long Id { get; set; }
        public int MajorCode { get; set; } // 0 or 1 
        public int MinorCode { get; set; }
        public string Name { get; set; }
     }
}
