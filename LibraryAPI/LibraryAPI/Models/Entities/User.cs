﻿namespace LibraryAPI.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public int UserCredentialId { get; set; }
        public Guid PersonId { get; set; }
        public bool IsLocked { get; set; }
        public bool IsConfirmed { get; set; }
        public virtual Person Person { get; set; } = new Person();
        public virtual ICollection<UserCredential> UserCredentials { get; set; } = new HashSet<UserCredential>();
        public virtual ICollection<UserBookRented> UserBookRented { get; set; } = new HashSet<UserBookRented>();
    }
}
