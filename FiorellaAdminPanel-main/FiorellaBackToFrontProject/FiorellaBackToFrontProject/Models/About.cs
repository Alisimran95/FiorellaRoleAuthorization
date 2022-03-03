namespace FiorellaBackToFrontProject.Models
{
    public class About
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string VideoSource { get; set; }

        public AboutIcon AboutIcons  { get; set; }

        public int  AboutIconId { get; set; }
        
    }
}
