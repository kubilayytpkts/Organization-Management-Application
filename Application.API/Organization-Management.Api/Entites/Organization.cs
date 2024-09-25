namespace Organization_Management.Api.Entites
{
    public class Organization
    {
        public int OrganizationId { get; set; } // Anahtar
        public string Name { get; set; } // Organizasyon adı
        public int? ParentOrganizationId { get; set; } // Üst organizasyonun ID'si (opsiyonel)

        // Navigation properties
        public virtual ICollection<Employee> Personels { get; set; } // Organizasyona bağlı personeller
        public virtual Organization ParentOrganization { get; set; } // Üst organizasyon
        public virtual ICollection<Organization> SubOrganizations { get; set; } // Alt organizasyonlar
    }
}
