namespace Organization_Management.Api.Entites
{
    public class Personel
    {
        public int PersonelId { get; set; } // Anahtar
        public string SicilNo { get; set; } // Personel sicil numarası
        public string Ad { get; set; } // Personel adı
        public string Soyad { get; set; } // Personel soyadı
        public int OrganizationId { get; set; } // Bağlı olduğu organizasyonun ID'si

        // Navigation property
        public virtual Organization Organization { get; set; } // İlişkili organizasyon
    }
}
