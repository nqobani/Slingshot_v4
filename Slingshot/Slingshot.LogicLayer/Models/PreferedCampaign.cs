using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slingshot.LogicLayer.Models
{
    public class PreferedCampaign
    {
        [Key]
        public long Id { set; get; }
        public string userId { set; get; }
        public long campId { set; get; }
        public Boolean hasFavourited { get; set; }
    }

}
