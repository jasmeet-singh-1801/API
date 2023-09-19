     // Ignore Spelling: APICUSTOMERS

    using System.ComponentModel.DataAnnotations;

    namespace APICUSTOMERS.Models
    {
        public class Customers
        {
            [Key]
            public string? Customer_ID { get; set; }
            public string? F_Name { get; set; }
            public string? L_Name { get; set; }
            public string? C_Add { get; set; }
            public string? City { get; set; }
        }
    }
