﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// classe metier identifiant les differents services ammanés à utiliser l'application
    /// </summary>
    public class Service
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
    }

}
