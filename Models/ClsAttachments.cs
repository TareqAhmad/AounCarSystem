
namespace AounCarSystem.Models
{
    public class ClsAttachments
    {

        public int Attachment_Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileType { get; set; }

        public string UploadDate { get; set; }

        public string Description { get; set; }

        public ClsCars ChassisNumber { get; set; }


    }
}
