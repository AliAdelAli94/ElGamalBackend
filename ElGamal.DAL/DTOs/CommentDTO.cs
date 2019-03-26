using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.DAL.DTOs
{
    public class CommentDTO
    {
        public Guid ID { get; set; }

        public string commentText { get; set; }

        public decimal? ratingValue { get; set; }

        public Guid? userID { get; set; }

        public string userName { get; set; }

        public Guid? productID { get; set; }

    }
}
