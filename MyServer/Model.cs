using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RetroDRY;

namespace MyServer
{
    //TODO: Using these samples and the documentation, create your model. It does not need to be in a file called Model.cs - large models might be
    //divided among many files.

    /*
    /// <summary>
    /// Lookup table of phone types (like "cell", "work")
    /// </summary>
    [Prompt("Phone types")]
    public class PhoneTypeLookup : Persiston
    {
        public List<PhoneTypeRow> PhoneType;

        [Prompt("Type")]
        public class PhoneTypeRow : Row
        {
            [PrimaryKey(true)]
            [Prompt("ID")]
            public short? PhoneTypeId;

            [StringLength(20), MainColumn, SortColumn]
            [Prompt("Phone type")]
            public string TypeOfPhone;
        }
    }

    /// <summary>
    /// Lookup table of sale statuses (like Confirmed, Shipped etc)
    /// </summary>
    [Prompt("Sale statuses")]
    public class SaleStatusLookup : Persiston
    {
        public List<SaleStatusRow> SaleStatus;

        [Prompt("Status")]
        public class SaleStatusRow : Row
        {
            [PrimaryKey(true)]
            [Prompt("ID")]
            public short? StatusId;

            [StringLength(20, MinimumLength = 1), MainColumn, SortColumn]
            [Prompt("Sale status")]
            public string Name;

            [StringLength(20), WireType(Constants.TYPE_NSTRING)]
            public string Note;
        }
    }

    /// <summary>
    /// One employee with their contacts
    /// </summary>
    [SingleMainRow]
    public class Employee : Persiston
    {
        [PrimaryKey(true)]
        [Prompt("ID")]
        public int? EmployeeId;

        [StringLength(50, MinimumLength = 1), Prompt("First name")]
        public string FirstName;

        [StringLength(50, MinimumLength = 1), Prompt("Last name"), MainColumn]
        public string LastName;

        [ForeignKey(typeof(Employee))]
        [Prompt("Supervisor ID")]
        [SelectBehavior(typeof(EmployeeList))]
        public int? SupervisorId;

        [LeftJoin("SupervisorId", "LastName"), Prompt("Supervisor")]
        public string SupervisorLastName;

        [Prompt("Hired on")]
        public DateTime HireDate;

        public bool IsToxic;

        public List<EContactRow> EContact;

        [SqlTableName("EmployeeContact"), ParentKey("EmployeeId")]
        [Prompt("Employee contact")]
        public class EContactRow : Row
        {
            [PrimaryKey(true)]
            [Prompt("ID")]
            public int? ContactId;

            [ForeignKey(typeof(PhoneTypeLookup)), Prompt("Phone type")]
            public short PhoneType;

            [StringLength(50), Prompt("Phone number")]
            public string Phone;
        }
    }

    /// <summary>
    /// One customer
    /// </summary>
    [SingleMainRow]
    public class Customer : Persiston
    {
        [PrimaryKey(true)]
        [Prompt("ID")]
        public int? CustomerId;

        [StringLength(200, MinimumLength = 1)]
        public string Company;

        [ForeignKey(typeof(Employee))]
        [SelectBehavior(typeof(EmployeeList))]
        [Prompt("Sales rep ID")]
        public int SalesRepId;

        [LeftJoin("SalesRepId", "LastName"), Prompt("Sales rep")]
        public string SalesRepLastName;

        [StringLength(4000), WireType(Constants.TYPE_NSTRING)]
        public string Notes;
    }

    /// <summary>
    /// One item that the company sells which can have multiple variants
    /// </summary>
    [SingleMainRow]
    public class Item : Persiston
    {
        [PrimaryKey(true)]
        [Prompt("ID")]
        public int? ItemId;

        [Prompt("I-code"), RegularExpression("^[A-Z]{2}-[0-9]{4}$"), MainColumn]
        public string ItemCode;

        [StringLength(200, MinimumLength = 3)]
        public string Description;

        public double? Weight;

        public decimal? Price;

        public List<ItemVariantRow> ItemVariant;

        [ParentKey("ItemId")]
        [Prompt("Variant")]
        public class ItemVariantRow : Row
        {
            [PrimaryKey(true)]
            [Prompt("Var-ID")]
            public int? ItemVariantId;

            [StringLength(20, MinimumLength = 1), Prompt("Sub-code"), MainColumn, SortColumn]
            public string VariantCode;

            [StringLength(200, MinimumLength = 3)]
            public string Description;
        }
    }

    /// <summary>
    /// One sale including its line items and notes
    /// </summary>
    [SingleMainRow]
    public class Sale : Persiston
    {
        [PrimaryKey(true), MainColumn]
        [Prompt("ID")]
        public int? SaleId;

        [ForeignKey(typeof(Customer))]
        [SelectBehavior(typeof(CustomerList))]
        [Prompt("Customer ID")]
        public int CustomerId;

        [Prompt("Sale date"), WireType(Constants.TYPE_DATETIME)]
        public DateTime SaleDate;

        [Prompt("Shipped on"), WireType(Constants.TYPE_DATETIME)]
        public DateTime? ShippedDate;

        [ForeignKey(typeof(SaleStatusLookup)), Prompt("Status")]
        public short Status;

        public List<SaleItemRow> SaleItem;

        [ParentKey("SaleId")]
        [Prompt("Item sold")]
        public class SaleItemRow : Row
        {
            [PrimaryKey(true), SortColumn]
            [Prompt("ID")]
            public int? SaleItemId;

            [ForeignKey(typeof(Item))]
            [Prompt("Item-ID")]
            [SelectBehavior(typeof(ItemList), UseDropdown = true)] //UseDropdown here is ok because the company has only a few items
            public int ItemId;

            [LeftJoin("ItemId", "Description")]
            public string ItemDescription;

            [Range(1, 999)]
            public int Quantity;

            [Prompt("Var-ID")]
            [SelectBehavior(typeof(ItemVariantList), AutoCriterionName = "ItemId", AutoCriterionValueColumnName = "ItemId", ViewonValueColumnName = "ItemVariantId", UseDropdown = true)]
            public int? ItemVariantId;

            [Prompt("Ext Price")]
            public decimal ExtendedPrice;

            public List<SaleItemNoteRow> SaleItemNote;

            [ParentKey("SaleItemId")]
            [Prompt("Sale note")]
            public class SaleItemNoteRow : Row
            {
                [PrimaryKey(true)]
                [Prompt("ID")]
                public int? SaleItemNoteId;

                [StringLength(4000), WireType(Constants.TYPE_NSTRING)]
                public string Note;
            }
        }
    }

    /// <summary>
    /// List of employees
    /// </summary>
    public class EmployeeList : Viewon
    {
        public List<TopRow> Employee;

        [InheritFrom("Employee")]
        public class TopRow : Row
        {
            [PrimaryKey(true), ForeignKey(typeof(Employee)), SortColumn(false)]
            public int EmployeeId;

            [SortColumn(false)]
            public string FirstName;

            [SortColumn(true)]
            public string LastName;

            [ForeignKey(typeof(Employee))]
            public int SupervisorId;

            [LeftJoin("SupervisorId", "LastName"), Prompt("Supervisor")]
            public string SupervisorLastName;

            public bool IsToxic;
        }

        [Criteria]
        public abstract class Criteria
        {
            [Prompt("Last name starts with")]
            public string LastName;

            [Prompt("Toxic human?")]
            public bool IsToxic;
        }
    }

    /// <summary>
    /// List of customers
    /// </summary>
    public class CustomerList : Viewon
    {
        [Prompt("Customer List")]
        public List<TopRow> Customer;

        [InheritFrom("Customer")]
        public class TopRow : Row
        {
            [PrimaryKey(true), ForeignKey(typeof(Customer)), SortColumn(false)]
            public int CustomerId;

            [SortColumn(true)]
            public string Company;

            [ForeignKey(typeof(Employee))]
            public int SalesRepId;

            [LeftJoin("SalesRepId", "LastName"), Prompt("Sales rep.")]
            public string SalesRepLastName;
        }

        [Criteria]
        public abstract class Criteria
        {
            [Prompt("Company starts with")]
            public string Company;

            [InheritFrom("Customer.SalesRepId")]
            public int SalesRepId;

            [InheritFrom("Customer.SalesRepLastName")]
            public string SalesRepLastName;
        }
    }

    /// <summary>
    /// List of items
    /// </summary>
    public class ItemList : Viewon
    {
        public List<TopRow> Item;

        [InheritFrom("Item")]
        public class TopRow : Row
        {
            [PrimaryKey(true), ForeignKey(typeof(Item))]
            public int ItemId;

            [SortColumn(true)]
            public string ItemCode;

            [SortColumn(false)]
            public string Description;

            public double? Weight;

            public decimal Price;
        }

        [Criteria]
        public abstract class Criteria
        {
            [Prompt("I-code starts with")]
            public string ItemCode;

            [Prompt("Item description")]
            public string Description;

            public double? Weight;
        }
    }

    /// <summary>
    /// List of item variants (used only as source of dropdown select for Sale.ItemVariantId)
    /// </summary>
    public class ItemVariantList : Viewon
    {
        public List<TopRow> ItemVariant;

        public class TopRow : Row
        {
            [PrimaryKey(true)]
            public int ItemVariantId;

            public int ItemId;

            [MainColumn, SortColumn(true)]
            public string VariantCode;

            [VisibleInDropdown]
            public string Description;
        }

        [Criteria]
        public abstract class Criteria
        {
            public int? ItemId;
        }
    }

    /// <summary>
    /// List of sales
    /// </summary>
    public class SaleList : Viewon
    {
        public List<TopRow> Sale;

        [InheritFrom("Sale", IncludeCustom = true)]
        public class TopRow : Row
        {
            [PrimaryKey(true), ForeignKey(typeof(Sale))]
            public int SaleId;

            [ForeignKey(typeof(Customer))]
            public int CustomerId;

            [SortColumn, WireType(Constants.TYPE_DATETIME)]
            public DateTime SaleDate;

            [ForeignKey(typeof(SaleStatusLookup)), SortColumn(false)]
            public short Status;
        }

        [Criteria]
        public abstract class Criteria
        {
            [InheritFrom("Sale.CustomerId")]
            [ForeignKey(typeof(Customer))]
            [SelectBehavior(typeof(CustomerList))]
            public int? CustomerId;

            public DateTime? SaleDate;

            [ForeignKey(typeof(SaleStatusLookup))]
            public short? Status;
        }
    }*/
}
