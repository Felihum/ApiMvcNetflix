﻿using System.Data.SqlTypes;

namespace ApiMvc.Models
{
    public class Subscription
    {
        public int id { get; internal set; }
        public string name { get; set; }
        public float value { get; set; }
        public string period { get; set; }
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
