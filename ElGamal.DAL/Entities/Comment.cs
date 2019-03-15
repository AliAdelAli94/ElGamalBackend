namespace ElGamal.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public Guid ID { get; set; }

        [Column(TypeName = "ntext")]
        public string commentText { get; set; }

        public decimal? ratingValue { get; set; }

        public Guid? userID { get; set; }

        public Guid? productID { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
