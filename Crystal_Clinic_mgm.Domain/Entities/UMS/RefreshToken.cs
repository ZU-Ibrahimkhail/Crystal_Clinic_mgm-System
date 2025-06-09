using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class RefreshToken
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; } = string.Empty;
        public DateTime? Revoked { get; set; }
        public string? RevokedByIp { get; set; } = string.Empty;
        public string? ReplacedByToken { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
        //[NotMapped]
        //[JsonIgnore]
        //private bool _isActive => !IsExpired == true && Revoked == null;
        //public bool IsActive
        //{
        //    set
        //    {
        //        IsActive = _isActive;
        //    }  
        //    get { return _isActive; }
        //}
        //[NotMapped]
        //[JsonIgnore]
        //private bool _isExpired => DateTime.UtcNow >= Expires;
        //public bool IsExpired
        //{
        //    set
        //    {
        //        IsExpired = _isExpired;
        //    }
        //    get { return _isExpired; }
        //}
    }
}
