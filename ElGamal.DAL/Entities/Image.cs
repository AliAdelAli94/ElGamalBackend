namespace ElGamal.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Image
    {
        public Guid ID { get; set; }

        public string imageUrl { get; set; }

        public Guid? productID { get; set; }

        public virtual Product Product { get; set; }
    }
}
