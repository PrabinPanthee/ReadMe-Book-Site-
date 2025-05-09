using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.Models.Models
{
    public class KhaltiResponse
    {
        public string pidx { get; set; }
        public string payment_url { get; set; }
        public DateTime expires_at { get; set; }
        public int expires_in { get; set; }
    }
}
