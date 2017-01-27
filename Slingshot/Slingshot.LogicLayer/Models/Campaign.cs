using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slingshot.Data.Models
{
    public class Campaign
    {
        [Key]
        public long Id { get; set; }
        public string creatorId { get; set; }
        public string name { get; set; }
        public string description { set; get; }
        public string status { get; set; }
        public string thumbnail { get; set; }
        [ForeignKey("Id")]
        public virtual ICollection<Email> email { get; set; }
    }
}
