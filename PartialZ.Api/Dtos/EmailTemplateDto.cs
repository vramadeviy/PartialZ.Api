namespace PartialZ.Api.Dtos
{
    public class EmailTemplateDto
    {
        public int Id { get; set; }

        public string Template { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }
    }
}
