using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
  
        // DTO for creating a new offer (request from Admin to Offers)
        public class CreateOfferRequestContract
        {
            public int SupplierId { get; set; }
            public string ProductTitle { get; set; }
            public string ProductDescription { get; set; }
            public decimal WholesalePrice { get; set; }
            public decimal TotalQuantity { get; set; }
            public string QuantityType { get; set; }
            public decimal CompanyProfitPercentage { get; set; }
            public decimal TotalCustomsDuties { get; set; }
            public decimal TotalOtherAcquisitionTaxes { get; set; }
            public decimal TotalLogisticsAndHandlingCosts { get; set; }
            public decimal CustomerVATRate { get; set; }
            public decimal SupplierInitialPaymentPercentage { get; set; }
        }

        // DTO for updating an existing offer (request from Admin to Offers)
        public class UpdateOfferRequestContract
        {
            public int SupplierId { get; set; } // Can be updated or kept static
            public string ProductTitle { get; set; }
            public string ProductDescription { get; set; }
            public decimal WholesalePrice { get; set; }
            public decimal TotalQuantity { get; set; }
            public string QuantityType { get; set; }
            public decimal CompanyProfitPercentage { get; set; }
            public decimal TotalCustomsDuties { get; set; }
            public decimal TotalOtherAcquisitionTaxes { get; set; }
            public decimal TotalLogisticsAndHandlingCosts { get; set; }
            public decimal CustomerVATRate { get; set; }
            public decimal SupplierInitialPaymentPercentage { get; set; }
            public string Status { get; set; } // Allow updating offer status
        }

        // DTO for an Offer (response from Offers to Admin/Customer)
        public class OfferContract
        {
            public int OfferId { get; set; }
            public int SupplierId { get; set; }
            public string ProductTitle { get; set; }
            public string ProductDescription { get; set; }
            public decimal WholesalePrice { get; set; }
            public decimal TotalQuantity { get; set; }
            public decimal AvailableQuantity { get; set; }
            public string QuantityType { get; set; }
            public decimal CompanyProfitPercentage { get; set; } // Pricing details included
            public decimal CustomerVATRate { get; set; } // VAT rate for the customer
            public string Status { get; set; }
            public string SupplierFinalPaymentStatus { get; set; } // For admin views
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }
    }

