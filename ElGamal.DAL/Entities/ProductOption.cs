namespace ElGamal.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductOption
    {
        public Guid ID { get; set; }

        [Column(TypeName = "ntext")]
        public string optionText { get; set; }

        public Guid? productID { get; set; }

        public virtual Product Product { get; set; }
    }
}
