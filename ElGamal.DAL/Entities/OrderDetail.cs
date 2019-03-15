namespace ElGamal.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        public Guid ID { get; set; }

        public decimal? buyingPrice { get; set; }

        public int? quantity { get; set; }

        public Guid? productID { get; set; }

        public Guid? orderID { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
