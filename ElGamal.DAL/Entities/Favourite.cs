namespace ElGamal.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Favourite
    {
        public Guid ID { get; set; }

        public Guid? productID { get; set; }

        public Guid? userID { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
