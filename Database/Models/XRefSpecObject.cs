using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseLib.Models
{
    [Table("UidTable", Schema = "dbo")]
    public class XRefSpecObject
    {
        [Key]
        public string HashedUid { get; set; }

        public string Uid { get; set; }

        [Required]
        public string XRefSpecJson { get; set; }
    }
}